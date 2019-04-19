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
    private Rigidbody2D rb;

    public float speed;
    public float agility;

    void Start()
    {
        rb = gameObject.GetComponent<Rigidbody2D>();
        walkSpeed = (float)(speed + (agility / 5));
        sprintSpeed = walkSpeed + (walkSpeed / 2);

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
