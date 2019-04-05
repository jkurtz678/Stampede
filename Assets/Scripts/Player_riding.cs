using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_riding : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    //Lukas
    // Update is called once per frame
    void Update()
    {
        //Check for button press
        if (Input.GetKeyDown(KeyCode.E))
        {
            GameObject closestBoid = FindClosestEnemy();
            gameObject.transform.position = closestBoid.transform.position;
            closestBoid.GetComponent<Buffaloid>().enabled = false;
            closestBoid.transform.parent = gameObject.transform;
        }
    }

    //Lukas
    public GameObject FindClosestEnemy()
    {
        GameObject[] gos;
        gos = GameObject.FindGameObjectsWithTag("Boids");
        GameObject closest = null;
        float distance = Mathf.Infinity;
        Vector3 position = transform.position;
        foreach (GameObject go in gos)
        {
            Vector3 diff = go.transform.position - position;
            float curDistance = diff.sqrMagnitude;
            if (curDistance < distance)
            {
                closest = go;
                distance = curDistance;
            }
        }
        return closest;
    }
}
