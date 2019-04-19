using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_riding : MonoBehaviour
{
    public float acceleration;
    public float steering;
    public float maxSpeed;

    public string horAxis;
    public string verAxis;

    private Rigidbody2D rb;

    void Start()
    {
        rb = gameObject.GetComponent<Rigidbody2D>();
    }

    void FixedUpdate()
    {
        float h = -Input.GetAxis(horAxis);
        float v = Input.GetAxis(verAxis);
        Debug.Log(v);
        Vector2 speed = transform.up * (v * acceleration);
        rb.AddForce(speed);

        if ( rb.velocity.magnitude > maxSpeed)
        {
            rb.velocity = rb.velocity.normalized * maxSpeed;
        }

        float direction = Vector2.Dot(rb.velocity, rb.GetRelativeVector(Vector2.up));
        if (direction >= 0.0f)
        {
            rb.rotation += h * steering * (rb.velocity.magnitude / 5.0f);
            //rb.AddTorque((h * steering) * (rb.velocity.magnitude / 10.0f));
        }
        else
        {
            rb.rotation -= h * steering * (rb.velocity.magnitude / 5.0f);
            //rb.AddTorque((-h * steering) * (rb.velocity.magnitude / 10.0f));
        }

        Vector2 forward = new Vector2(0.0f, 0.5f);
        float steeringRightAngle;
        if (rb.angularVelocity > 0)
        {
            steeringRightAngle = -90;
        }
        else
        {
            steeringRightAngle = 90;
        }

        Vector2 rightAngleFromForward = Quaternion.AngleAxis(steeringRightAngle, Vector3.forward) * forward;
        Debug.DrawLine((Vector3)rb.position, (Vector3)rb.GetRelativePoint(rightAngleFromForward), Color.green);

        float driftForce = Vector2.Dot(rb.velocity, rb.GetRelativeVector(rightAngleFromForward.normalized));

        Vector2 relativeForce = (rightAngleFromForward.normalized * -1.0f) * (driftForce * 10.0f);


        Debug.DrawLine((Vector3)rb.position, (Vector3)rb.GetRelativePoint(relativeForce), Color.red);

        rb.AddForce(rb.GetRelativeVector(relativeForce));
    }
    /*
    private Rigidbody2D rb;
    private Vector2 moveVelocity;
    private Vector3 m_EulerAngleVelocity;

    public float revSpeed;
    public float boostSpeed;
    public float slowdownSpeed;
    public float turnSpeed;

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
            rb.AddForce(transform.right * turnSpeed);
        }
        else if (Input.GetKey(KeyCode.A))
        {
            rb.MoveRotation(rb.rotation + revSpeed * Time.fixedDeltaTime);
            rb.AddForce(transform.right * -turnSpeed);
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
    */
}
