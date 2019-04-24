﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_collision : MonoBehaviour
{

    void OnCollisionStay2D(Collision2D obj)
    {
        if (obj.gameObject.tag == "Boid")
        {
            Vector3 dir = obj.transform.position - transform.position;
            if (Vector3.Angle(dir, obj.transform.up * -1) <= 22.5)
            {
                if (obj.relativeVelocity.magnitude > 2)
                {
                    //DEATH the player died
                    gameObject.SetActive(false);
                }
            }
        }
    }
}
