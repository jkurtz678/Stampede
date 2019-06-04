using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_2 : MonoBehaviour
{
    // Normal Movements Variables
    private float curSpeed;
    private float myAngle;
    private Rigidbody2D rb;
    public float stunTime = 7;
    private float stunCheck;
    private float flashTime;
    private SpriteRenderer sr;

    public float speed = 1.5f;
    public float stunScalar = 2.5f;
    public bool stunned;

    void Start()
    {
        stunned = false;
        flashTime = 0;
        stunCheck = stunTime;
        myAngle = 0;
        sr = gameObject.GetComponent<SpriteRenderer>();
        rb = gameObject.GetComponent<Rigidbody2D>();

    }
    private void Update()
    {
        //Rotate towards movement
        float horAxis = -Input.GetAxis("P2_Horizontal");
        float verAxis = Input.GetAxis("P2_Vertical");
        if (horAxis != 0 | verAxis != 0)
        {
            myAngle = Mathf.Atan2(horAxis, verAxis) * Mathf.Rad2Deg;
        }
        gameObject.transform.rotation = Quaternion.AngleAxis(myAngle, Vector3.forward);
    }

    void FixedUpdate()
    {

        if (stunned)
        {
            if (stunTime == stunCheck)
            {
                curSpeed = speed / stunScalar;
            }
            flashTime += Time.deltaTime;
            stunTime -= Time.deltaTime;
            if(flashTime > 0.20f)
            {
                if(sr.color == Color.red)
                {
                    sr.color = Color.white;
                } else
                {
                    sr.color = Color.red;
                }
                flashTime = 0;
            } 
            if (stunTime < 0)
            {
                sr.color = Color.white;
                GameObject.Find("GameManager").GetComponent<Player_2_switch>().stunned = false;
                stunned = false;
                stunTime = stunCheck;
            }
        }
        else
        {
            curSpeed = speed;
        }
        // Move senteces
        rb.velocity = new Vector2(Mathf.Lerp(0, Input.GetAxis("P2_Horizontal") * curSpeed, 0.8f),
                                             Mathf.Lerp(0, Input.GetAxis("P2_Vertical") * curSpeed, 0.8f));
    }
}
