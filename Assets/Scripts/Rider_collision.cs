using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rider_collision : MonoBehaviour
{
    public float bumpVelocity = 2;
    public bool bumpOff = false;
    public Rigidbody2D attacker_rb;
    public Vector3 dir;
    public string enemyRiderStr;

    private GameObject enemyRider;

    void OnCollisionEnter2D(Collision2D obj)
    {
        //bumpOff = false;
        if (obj.gameObject.tag == enemyRiderStr)
        {
            
            foreach (ContactPoint2D contact in obj.contacts)
            {
                Debug.DrawRay(contact.point, contact.normal, Color.red, 2.0f);
                dir = contact.normal * -1;
            }

            Debug.Log(Vector3.Angle(dir, transform.up));
            if (Vector3.Angle(dir, transform.up) <= 40)
            {

                attacker_rb = obj.rigidbody;
                enemyRider = GameObject.FindGameObjectWithTag(enemyRiderStr);
                //Set enemies bump flag to true
                enemyRider.GetComponent<Rider_collision>().bumpOff = true;

            }
        }
    }

}
