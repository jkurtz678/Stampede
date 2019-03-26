﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Buffaloid : MonoBehaviour
{

    public float speed;
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
            transform.right = hitColliders[0].gameObject.transform.position - transform.position;
            return Vector2.MoveTowards(transform.position, hitColliders[0].gameObject.transform.position, -1 * speed * Time.deltaTime);
        }
        else
        {
            return transform.position;
        }
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 avoidDir = getAvoid();
        move = avoidDir;
        transform.position = move;
    }
}
