using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_riding : MonoBehaviour
{


    private Rigidbody2D rb;
    private Vector2 moveVelocity;
    private Vector3 m_EulerAngleVelocity;

    public float revSpeed;
    public float boostSpeed;
    public float slowdownSpeed;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        m_EulerAngleVelocity = new Vector3(0, 100, 0);
    }

    //Lukas
    // Update is called once per frame
    void Update()
    {
        //Vector2 moveInput = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        //moveVelocity = moveInput.normalized * boostSpeed;
        //Check for button press
        
    }

    void FixedUpdate()
    {
        Quaternion deltaRotation = Quaternion.Euler(m_EulerAngleVelocity * Time.deltaTime);
        //rb.AddForce(moveVelocity * boostSpeed);
        if (Input.GetKeyDown(KeyCode.W))
        {
            rb.AddForce(transform.up * boostSpeed);
        }
        else if (Input.GetKey(KeyCode.S))
        {
            rb.AddForce(transform.up * -slowdownSpeed);
        }
        if (Input.GetKey(KeyCode.D))
        {
            rb.MoveRotation(rb.rotation - revSpeed * Time.fixedDeltaTime);
        }
        else if (Input.GetKey(KeyCode.A))
        {
            rb.MoveRotation(rb.rotation + revSpeed * Time.fixedDeltaTime);
        }
    }

    //Lukas
    public void DetachFromParent()
    {
        // Detaches the transform from its parent.
        transform.parent = null;
    }


    //Lukas
    public GameObject FindClosestBoid()
    {
        GameObject[] gos;
        gos = GameObject.FindGameObjectsWithTag("Boids");
        GameObject closest = null;
        float distance = Mathf.Infinity;
        Vector3 position = transform.position;
        foreach (GameObject go in gos)
        {
            Vector3 diff = go.transform.position - position;
            float curDistance = diff.sqrMagnitude;
            if (curDistance < distance)
            {
                closest = go;
                distance = curDistance;
            }
        }
        return closest;
    }
}
