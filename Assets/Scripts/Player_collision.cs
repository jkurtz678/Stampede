using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_collision : MonoBehaviour
{
    public float deathVelocity = 2;

    void OnCollisionStay2D(Collision2D obj)
    {
        if (obj.gameObject.tag == "Boids")
        {
            Vector3 dir = obj.transform.position - transform.position;
            if (Vector3.Angle(dir, obj.transform.up * -1) <= 22.5)
            {
                if (obj.relativeVelocity.magnitude > deathVelocity)
                {
                    //DEATH the player died
                    gameObject.SetActive(false);
                }
            }
        }
    }
}
