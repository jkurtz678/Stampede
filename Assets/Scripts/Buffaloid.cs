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

    Vector2 getAvoid()
    {

        Collider2D[] hitColliders = Physics2D.OverlapCircleAll(transform.position, separation_radius);
        Debug.Log(hitColliders.Length);

        if (hitColliders.Length > 0)
        {
            Debug.Log("object within radius");
            //transform.up = hitColliders[0].gameObject.transform.position - transform.position;
            return Vector2.MoveTowards(transform.position, hitColliders[0].gameObject.transform.position, -1 * moveSpeed * Time.deltaTime);
        }
        else
        {
            return transform.position;
        }
    }


    void moveObject(Vector2 mv)
    {
        Vector2 direction = (Vector3)mv - transform.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        Quaternion rotation = Quaternion.AngleAxis(angle, Vector3.down);
        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, rotationSpeed * Time.deltaTime);
        transform.position = mv;
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 avoidDir = getAvoid();
        move = avoidDir;
        moveObject(move);
    }
}
