using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rider_collision : MonoBehaviour
{
    public float bumpVelocity = 2;
    public bool bumpOff = false;
    public Vector3 dir;
    public string enemyRiderStr;
    public float killAngle = 45;

    public GameObject enemyRider;

    void OnCollisionEnter2D(Collision2D obj)
    {
        //bumpOff = false;
        if (obj.gameObject.tag == enemyRiderStr)
        {

            foreach (ContactPoint2D contact in obj.contacts)
            {
                Vector3 contactV3 = contact.point;
                dir = contactV3 - transform.position;
                Debug.DrawRay(transform.position, dir, Color.red, 3.0f);
            }
            enemyRider = GameObject.FindGameObjectWithTag(enemyRiderStr);
            //Debug.Log(Vector3.Angle(dir, transform.up));
            if (obj.relativeVelocity.magnitude > bumpVelocity && Vector3.Angle(dir, transform.up) <= 40)
            {
                
                //Set enemies bump flag to true
                enemyRider.GetComponent<Rider_collision>().bumpOff = true;

            }
        }
    }
}
