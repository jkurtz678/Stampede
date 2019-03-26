using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Buffaloid : MonoBehaviour
{

    public float moveSpeed;
    public float rotationSpeed;
    public float separation_radius;

    private Rigidbody2D rb;
    private Vector2 move;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
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
            return Vector2.MoveTowards(transform.position, average, -1 * moveSpeed * Time.deltaTime);
        }
        else
        {
            //return transform.position;
            return Vector2.zero;
        }
    }

    //moves object towards
    void moveObject(Vector2 mv)
    {
        Debug.Log("mv vector: " + mv);
        //Vector2 direction = (Vector3)mv - transform.position;
        //Debug.Log("direction vector: "  + direction);

        //float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        //Quaternion rotation = Quaternion.AngleAxis(angle, Vector3.back);
        //transform.rotation = Quaternion.Slerp(transform.rotation, rotation, rotationSpeed * Time.deltaTime);
        //float angle = Mathf.Atan2( mv.y, mv.x) * Mathf.Rad2Deg;
        //float angle = 
        //float euler_z = angle - transform.rotation.z;
        //transform.Rotate(0, 0, euler_z);
        transform.position = mv;
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 avoidDir = getAvoid();
        Debug.Log("avoid dir: " + avoidDir);

        move = avoidDir;

        if(move != Vector2.zero)
        {
            moveObject(move);
        }
    }
}
