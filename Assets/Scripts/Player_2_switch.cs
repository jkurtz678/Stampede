using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_2_switch : MonoBehaviour
{
    private GameObject player_game_obj;
    private GameObject closestBoid;
    //False = standard, True = riding
    private bool controlType = false;

    // Start is called before the first frame update
    void Start()
    {
        player_game_obj = GameObject.Find("Player2");
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && controlType == false)
        {
            controlType = true;
            closestBoid = FindClosestBoid();
            
            player_game_obj.transform.position = closestBoid.transform.position;
            player_game_obj.transform.rotation = closestBoid.transform.rotation;
            //Alter components of player and boid for force movement.
            closestBoid.GetComponent<Buffaloid>().enabled = false;
            closestBoid.GetComponent<Rigidbody2D>().isKinematic = true;
            player_game_obj.GetComponent<Player_2>().enabled = false;
            player_game_obj.GetComponent<Collider2D>().enabled = false;
            player_game_obj.GetComponent<Player_riding>().enabled = true;
            closestBoid.transform.parent = player_game_obj.transform;
        }
        else if(Input.GetKeyDown(KeyCode.Space) && controlType == true)
        {
            controlType = false;
            player_game_obj.GetComponent<Player_riding>().enabled = false;
            player_game_obj.GetComponent<Collider2D>().enabled = true;
            player_game_obj.GetComponent<Player_2>().enabled = true;
            closestBoid.GetComponent<Buffaloid>().enabled = true;
            closestBoid.GetComponent<Rigidbody2D>().isKinematic = false;
        }
    }


    //Lukas
    public GameObject FindClosestBoid()
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
