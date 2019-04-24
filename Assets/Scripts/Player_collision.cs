using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_collision : MonoBehaviour
{

    void OnCollisionStay2D(Collision2D obj)
    {
        Vector3 dir = obj.transform.position - transform.position;
        if (Vector3.Angle(dir, obj.transform.up) <= 22.5)
        {
            Debug.Log("Hitting on the movement direction side");
        }
    }
}
