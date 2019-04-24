using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{

    // Normal Movements Variables
    private float walkSpeed;
    private float curSpeed;
    private float maxSpeed;
    private float sprintSpeed;
    private float myAngle;
    private Rigidbody2D rb;

    public float speed;
    public float agility;

    void Start()
    {
        myAngle = 0;
        rb = gameObject.GetComponent<Rigidbody2D>();
        walkSpeed = (float)(speed + (agility / 5));
        sprintSpeed = walkSpeed + (walkSpeed / 2);

    }
    private void Update()
    {
        //Rotate towards movement
        float horAxis = -Input.GetAxis("P1_Horizontal");
        float verAxis = Input.GetAxis("P1_Vertical");
        if (horAxis != 0 | verAxis != 0)
        {
            myAngle = Mathf.Atan2(horAxis, verAxis) * Mathf.Rad2Deg;
        }
        gameObject.transform.rotation = Quaternion.AngleAxis(myAngle, Vector3.forward);
    }

    void FixedUpdate()
    {
        curSpeed = walkSpeed;
        maxSpeed = curSpeed;

        // Move senteces
        rb.velocity = new Vector2(Mathf.Lerp(0, Input.GetAxis("P1_Horizontal") * curSpeed, 0.8f),
                                             Mathf.Lerp(0, Input.GetAxis("P1_Vertical") * curSpeed, 0.8f));
    }
}
