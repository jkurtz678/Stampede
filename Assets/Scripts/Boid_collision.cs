using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boid_collision : MonoBehaviour
{
    public float bumpVelocity = 2;
    public Vector3 dir;
    public float killAngle = 45;

    private GameObject enemyRider;
    void OnCollisionEnter2D(Collision2D obj)
    {
        if (obj.gameObject.tag == "Rider1" || obj.gameObject.tag == "Rider2")
        {

            foreach (ContactPoint2D contact in obj.contacts)
            {
                
                Vector3 contactV3 = contact.point;
                dir = contactV3 - transform.position;
                Debug.DrawRay(transform.position, dir, Color.red, 2.0f);
            }

            //Debug.Log(Vector3.Angle(dir, transform.up));
            if (Vector3.Angle(dir, transform.up) <= killAngle)
            {

                enemyRider = obj.gameObject;
                //Set enemies bump flag to true
                enemyRider.GetComponent<Rider_collision>().bumpOff = true;

            }
        }

    }
}
