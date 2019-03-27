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
            Debug.Log("objects within radius: " + hitColliders.Length);

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
        //Vector2 direction = (Vector3)mv - transform.position;
        //Debug.Log("direction vector: "  + direction);

        //float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        //Quaternion rotation = Quaternion.AngleAxis(angle, Vector3.back);
        //transform.rotation = Quaternion.Slerp(transform.rotation, rotation, rotationSpeed * Time.deltaTime);
        //float angle = Mathf.Atan2( mv.y, mv.x) * Mathf.Rad2Deg;
        //float angle = 
        //float euler_z = angle - transform.rotation.z;
        //transform.Rotate(0, 0, euler_z);
        float step = maxSpeed * Time.deltaTime;

        if (move != Vector2.zero)
        {
            forwardVelocity += acceleration * Time.deltaTime;
            forwardVelocity = Mathf.Min(forwardVelocity, maxSpeed);
            transform.position = Vector2.MoveTowards(transform.position, mv, forwardVelocity * Time.deltaTime);


            //Debug.Log("transform up: " + transform.up);
            //Debug.Log("mv: " + mv);
            //Debug.Log("relative vector: " + (mv - (Vector2)transform.position));
            //Debug.Log("vector angle: " + Vector2.Angle(mv - (Vector2)transform.position, transform.up));
            Debug.Log("trans.z: " + transform.eulerAngles.z);

            float currRot = transform.eulerAngles.z;
            if (currRot > 180f )
            {
                currRot -= 360.0f;
            }
            Debug.Log("currRot: " + currRot);

            Debug.Log("mv: " + mv);


            Debug.Log("trans.pos: " + (Vector2)transform.position);
            Debug.Log("trans.up: " + transform.up);
            Debug.Log("vec.angle: " + Vector2.SignedAngle(mv - (Vector2)transform.position, transform.up));
            float targetDegree = currRot - Vector2.SignedAngle(mv - (Vector2)transform.position, transform.up);
            Debug.Log("target degree: " + targetDegree);


            float rotDegree = 0;
            //if( targetDegree > transform.eulerAngles.z)
            rotDegree = Mathf.LerpAngle(currRot, targetDegree, 1 * Time.deltaTime);
            


            Debug.Log("rotDegree: " + rotDegree);
            transform.eulerAngles = new Vector3(0, 0, rotDegree);
        }
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
