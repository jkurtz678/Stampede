using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Buffaloid : MonoBehaviour
{

    public float maxSpeed;
    public float timeZeroToMax;
    public float rotationSpeed;
    public float separation_radius;
    public float edge_separation;
    public float friend_radius;
    public float forwardVelocity;
    public float avoidWeight;
    public float avoidEdgeWeight;
    public float alignWeight;
    public float cohesionWeight;

    private Rigidbody2D rb;
    private Vector2 move;
    private float acceleration;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        acceleration = maxSpeed / timeZeroToMax;
        //forwardVelocity = 0f;
    }

    //returns vector pointing away from average concentration of nearby objects,
    //or a zero vector if no nearby objects
    Vector2 getAvoid()
    { 
        Collider2D[] hitColliders = Physics2D.OverlapCircleAll(transform.position, separation_radius);

        //if more nearby objects than self
        if (hitColliders.Length > 1)
        {
            //Debug.Log("objects within radius: " + hitColliders.Length);

            //compute average avoid vector
            Vector2 average = Vector2.zero;
            for (int i = 0; i < hitColliders.Length; i++)
            {
                //if object is not self
                if( hitColliders[i].gameObject.transform.root != transform)
                {
                    average += (Vector2)hitColliders[i].gameObject.transform.position;
                }
            }

            //get inverted vector relative to self
            //Debug.Log("average avoid: " + average);
            //Debug.Log("transpos: " + transform.position );
            Vector2 averageInverted = (Vector2)transform.position - average;

            //Vector2 averageInverted = ((Vector2)transform.position - average) + (Vector2)transform.position;
            //Debug.DrawRay(transform.position, averageInverted, Color.red);
            return averageInverted.normalized;
        }
        else
        {
            //return transform.position;
            return Vector2.zero;
        }
    }

    // returns vector pointing towards screen center if object is too close to edge, otherwise returns zero
    Vector2 getAvoidEdges()
    {

        Vector3 screenPos = Camera.main.WorldToScreenPoint(this.transform.position);
        
        if (screenPos.x < edge_separation || screenPos.y < edge_separation ||
         screenPos.x > Screen.width - edge_separation || screenPos.y > Screen.height - edge_separation)
        {
            Vector2 avoidEdgeVector = (-1 * (Vector2)transform.position).normalized;
            return avoidEdgeVector;
        }
        else
        {
            //Debug.Log("on screen");
            return Vector2.zero;
        }
    }

    //moves object towards vector
    void moveObject(Vector2 mv)
    {


        float step = maxSpeed * Time.deltaTime;
        //Debug.Log("accel: " + acceleration);
        //Debug.Log("forwardVelocity: " + forwardVelocity);

        //if buffaloid is moving to a point
        if (move != Vector2.zero)
        {
            //rotation handling, might make separate functions
            float currRot = transform.eulerAngles.z;
            if (currRot > 180f)
            {
                currRot -= 360.0f;
            }

            //float targetDegree = currRot - Vector2.SignedAngle(mv - (Vector2)transform.position, transform.up);
            float targetDegree = currRot - Vector2.SignedAngle(mv, transform.up);

            float rotDegree = 0;
            rotDegree = Mathf.LerpAngle(currRot, targetDegree, rotationSpeed * Time.deltaTime);
            transform.eulerAngles = new Vector3(0, 0, rotDegree);

            //move speed based on rotation
            float rotSpeedRatio = 0;
            float rotDistanceAngle = Mathf.Abs( Mathf.Abs(targetDegree) - Mathf.Abs(currRot) );
            //Debug.Log("rotDistanceAngle " + rotDistanceAngle);
            //velocity handling
            rotSpeedRatio = (0.5f - (rotDistanceAngle / 180)) * 2;
            //Debug.Log("rotspeedratio: " + rotSpeedRatio);
            if(rotSpeedRatio > 0)
            {
                Accelerate(rotSpeedRatio);
            }
            else
            {
                Decelerate(rotSpeedRatio);

            }

        }
        //decelerate
        else
        {
            Decelerate(-1);
        }
    }

    void ForceAccelerate(float rotSpeedRatio)
    {
        //rb.AddRelativeForce()
    }

    //acelerates this buffaloid game object, moving it in a forward direction
    //rotSpeedRatio : float between 0 - 1 that is multiplied with acceleration
    void Accelerate(float rotSpeedRatio) 
    {
        //Debug.Log("Accelerating...");

        forwardVelocity += rotSpeedRatio * acceleration * Time.deltaTime;
        forwardVelocity = Mathf.Min(forwardVelocity, maxSpeed);
        //Debug.Log("trans.pos:" + transform.position);
        //Debug.Log("trans.up:" + transform.up);
        transform.position = Vector2.MoveTowards(transform.position, transform.up + transform.position, forwardVelocity * Time.deltaTime);
    }

    //decelerates this buffaloid game object, moving it in a forward direction
    void Decelerate(float rotSpeedRatio)
    {
        //Debug.Log("Decelerating...");

        forwardVelocity -= (rotSpeedRatio * -1) * acceleration * Time.deltaTime;
        forwardVelocity = Mathf.Max(0, forwardVelocity);
        
        transform.position = Vector2.MoveTowards(transform.position, transform.up + transform.position, forwardVelocity * Time.deltaTime);
    }

    //get nearby buffaloids in pack
    List<GameObject> getFriends() 
    {
        Collider2D[] friendColliders = Physics2D.OverlapCircleAll(transform.position, friend_radius);
        List<GameObject> friends = new List<GameObject>();

        for (int i = 0; i < friendColliders.Length; i++)
        {
            if (friendColliders[i].gameObject.tag == "Boids" && friendColliders[i].gameObject.transform.root != transform)
            {
                friends.Add(friendColliders[i].gameObject);
            }
        }
        return friends;
    }

    private Vector2 getAlignment(List<GameObject> friends)
    {
        List<Transform> positions = new List<Transform>();
        for (int i = 0; i < friends.Count; i++)
        {
            positions.Add(friends[i].gameObject.transform);
        }

        Vector2 avg_dir = GetMeanDir(positions);
        return avg_dir;
    }

    private Vector2 GetMeanDir(List<Transform> transforms)
    {
        if (transforms.Count == 0)
            return Vector2.zero;

        Vector2 avg_vec = Vector2.zero;
        foreach (Transform pos in transforms)
        {
            avg_vec += (Vector2)pos.up;
        }
        return avg_vec.normalized;
    }

    //gets average position of a list of transforms
    private Vector2 GetMeanPos(List<Transform> transforms)
    {
        if (transforms.Count == 0)
            return Vector2.zero;

        float x = 0f;
        float y = 0f;

        foreach (Transform pos in transforms)
        {
            x += pos.position.x;
            y += pos.position.y;
 
        }
        return new Vector2(x / transforms.Count, y / transforms.Count);
    }

    //returns vector pointing towards center of pack of friends
    Vector2 getCohesion(List<GameObject> friends)
    {

        if (friends.Count > 0)
        {
            Debug.Log("in pack");
            List<Transform> positions = new List<Transform>();
            for(int i = 0; i < friends.Count; i++ )
            {
                positions.Add(friends[i].gameObject.transform);
            }

            Debug.Log("num positions: " + positions.Count);
            Debug.Log("first pos: " + positions[0]);

            Vector2 avg = GetMeanPos(positions);
            Debug.Log("calc avg: " + avg);

            Vector2 relativeAvg = avg - (Vector2)transform.position;
            //Debug.DrawRay(transform.position, avg - (Vector2)transform.position, Color.green);

            return relativeAvg.normalized;
        }
        else
        {
            return Vector2.zero;
        }
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 avoidDir = getAvoid();
        Vector2 edgeAvoidDir = getAvoidEdges();

        List<GameObject> friends = getFriends();

        //Debug.Log("avoid dir: " + avoidDir);

        Vector2 cohesionDir = getCohesion(friends);
        Vector2 alignmentDir = getAlignment(friends);

        Debug.Log("avoid dir: " + avoidDir );
        //Debug.Log("edge avoid dir: " + edgeAvoidDir);
        //Debug.Log("cohesion dir: " + cohesionDir);

        Debug.DrawRay(transform.position, alignmentDir, Color.white);
        Debug.DrawRay(transform.position, avoidDir, Color.red);
        Debug.DrawRay(transform.position, edgeAvoidDir, Color.magenta);
        Debug.DrawRay(transform.position, cohesionDir, Color.green);

        move = (avoidWeight * avoidDir) + (avoidEdgeWeight * edgeAvoidDir) 
            + (cohesionWeight * cohesionDir) + (alignWeight * alignmentDir);
        //move = avoidDir + edgeAvoidDir;

        Debug.DrawRay(transform.position, move, Color.blue);
        Debug.Log("move dir: " + move);


        moveObject(move);
    }
}
