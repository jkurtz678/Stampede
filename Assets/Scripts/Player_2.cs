using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_2 : MonoBehaviour
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
        rb.velocity = new Vector2(Mathf.Lerp(0, Input.GetAxis("P2_Horizontal") * curSpeed, 0.8f),
                                             Mathf.Lerp(0, Input.GetAxis("P2_Vertical") * curSpeed, 0.8f));
    }
    /*
    public float speed;

    private Rigidbody2D rb;
    private Vector2 moveVelocity;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 moveInput = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        moveVelocity = moveInput.normalized * speed; //normalized to make diagonal movement same speed
    }

    private void FixedUpdate()
    {
        rb.MovePosition(rb.position + moveVelocity * Time.fixedDeltaTime);
    }*/
}
