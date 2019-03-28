using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Buffaloid : MonoBehaviour
{

    public float maxSpeed;
    public float timeZeroToMax;
    public float rotationSpeed;
    public float separation_radius;

    private Rigidbody2D rb;
    private Vector2 move;
    private float forwardVelocity;
    private float acceleration;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        acceleration = maxSpeed / timeZeroToMax;
        forwardVelocity = 0f;
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
            Vector2 averageInverted = ((Vector2)transform.position - average) + (Vector2)transform.position;
            return averageInverted;
        }
        else
        {
            //return transform.position;
            return Vector2.zero;
        }
    }

    //moves object towards vector
    void moveObject(Vector2 mv)
    {
        float step = maxSpeed * Time.deltaTime;
        Debug.Log("accel: " + acceleration);
        Debug.Log("forwardVelocity: " + forwardVelocity);

        //if buffaloid is moving to a point
        if (move != Vector2.zero)
        {
            //rotation handling, might make separate functions
            float currRot = transform.eulerAngles.z;
            if (currRot > 180f)
            {
                currRot -= 360.0f;
            }

            float targetDegree = currRot - Vector2.SignedAngle(mv - (Vector2)transform.position, transform.up);

            float rotDegree = 0;
            rotDegree = Mathf.LerpAngle(currRot, targetDegree, rotationSpeed * Time.deltaTime);
            transform.eulerAngles = new Vector3(0, 0, rotDegree);

            //move speed based on rotation
            float rotSpeedRatio = 0;
            float rotDistanceAngle = Mathf.Abs( Mathf.Abs(targetDegree) - Mathf.Abs(currRot) );
            Debug.Log("rotDistanceAngle " + rotDistanceAngle);
            //velocity handling
            if (rotDistanceAngle < 90){
                rotSpeedRatio = 1 - (rotDistanceAngle / 90);
                Debug.Log("rotspeedratio: " + rotSpeedRatio);
                Accelerate(rotSpeedRatio);
            }
            else
            {
                Decelerate();
            }
        }
        //decelerate
        else
        {
            Decelerate();
        }
    }

    //acelerates this buffaloid game object, moving it in a forward direction
    //rotSpeedRatio : float between 0 - 1 that is multiplied with acceleration
    void Accelerate(float rotSpeedRatio) 
    {
        forwardVelocity += rotSpeedRatio * acceleration * Time.deltaTime;
        forwardVelocity = Mathf.Min(forwardVelocity, maxSpeed);
        Debug.Log("trans.pos:" + transform.position);
        Debug.Log("trans.up:" + transform.up);
        transform.position = Vector2.MoveTowards(transform.position, transform.up + transform.position, forwardVelocity * Time.deltaTime);
    }

    //decelerates this buffaloid game object, moving it in a forward direction
    void Decelerate()
    {
        Debug.Log("Decelerating...");

        forwardVelocity -= acceleration * Time.deltaTime;
        forwardVelocity = Mathf.Max(0, forwardVelocity);
        transform.position = Vector2.MoveTowards(transform.position, transform.up + transform.position, forwardVelocity * Time.deltaTime);
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 avoidDir = getAvoid();
        //Debug.Log("avoid dir: " + avoidDir);

        move = avoidDir;

        moveObject(move);
    }
}
