using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_collision : MonoBehaviour
{
    public float deathVelocity = 0.7f;
    public bool dead;
    private GameObject shakeCamera;

    void Start()
    {
        shakeCamera = GameObject.Find("Main Camera");
    }

    void OnCollisionStay2D(Collision2D obj)
    {
        if (obj.gameObject.tag == "Boids" || obj.gameObject.tag == "Rider1" || obj.gameObject.tag == "Rider2")
        {
            Vector3 dir = obj.transform.position - transform.position;
            if (Vector3.Angle(dir, obj.transform.up * -1) <= 25)
            {
                if (obj.relativeVelocity.magnitude > deathVelocity)
                {
                    //DEATH the player died
                    gameObject.SetActive(false);
                    shakeCamera.GetComponent<ShakeBehavior>().TriggerShake();
                    dead = true;
                }
            }
        }
    }
}
