using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_switch : MonoBehaviour
{
    public float ridingBoundary;
    public GameObject rider;

    private GameObject playerRider;
    private GameObject player_game_obj;
    private GameObject closestBoid;
    //False = standard, True = riding
    private bool controlType = false;

    // Start is called before the first frame update
    void Start()
    {
        player_game_obj = GameObject.Find("Player1");
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && controlType == false)
        {
            controlType = true;
            closestBoid = FindClosestBoid(player_game_obj);
            if (RidingBoundary(closestBoid, player_game_obj) == true){
                playerRider = Instantiate(rider, closestBoid.transform.position, closestBoid.transform.rotation);
                playerRider.GetComponent<Player_riding>().horAxis = "P1_Horizontal";
                playerRider.GetComponent<Player_riding>().verAxis = "P1_Vertical";
                closestBoid.SetActive(false);
                player_game_obj.SetActive(false);
                /*
                player_game_obj.transform.position = closestBoid.transform.position;
                player_game_obj.transform.rotation = closestBoid.transform.rotation;
                
                //Alter components of player and boid for force movement.
                closestBoid.GetComponent<Buffaloid>().enabled = false;
                closestBoid.GetComponent<Rigidbody2D>().isKinematic = true;
                closestBoid.GetComponent<Collider2D>().enabled = false;
                
                //player_game_obj.GetComponent<Collider2D>().enabled = false;
                player_game_obj.GetComponent<Player>().enabled = false;
                player_game_obj.GetComponent<Player_riding>().enabled = true;
                //Reset player velocity
                player_game_obj.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
                player_game_obj.GetComponent<Rigidbody2D>().angularVelocity = 0;
                closestBoid.transform.parent = player_game_obj.transform;
                */
            } else
            {
                controlType = false;
            }
        }
        else if(Input.GetKeyDown(KeyCode.E) && controlType == true)
        {
            controlType = false;
            playerRider.SetActive(false);
            closestBoid.SetActive(true);
            player_game_obj.SetActive(true);
            /*
            player_game_obj.GetComponent<Player_riding>().enabled = false;
            //player_game_obj.GetComponent<Collider2D>().enabled = true;
            player_game_obj.GetComponent<Player>().enabled = true;
            closestBoid.GetComponent<Buffaloid>().enabled = true;
            closestBoid.GetComponent<Rigidbody2D>().isKinematic = false;
            closestBoid.GetComponent<Collider2D>().enabled = true;
            closestBoid.transform.parent = null;*/
        }
    }

    public bool RidingBoundary(GameObject closest, GameObject player_game_obj)
    {
       
        Vector3 diff = closest.transform.position - player_game_obj.transform.position;
        if (diff.sqrMagnitude < ridingBoundary)
        {
            return true;
        } else {
            return false;
        }
    }

    //Lukas
    public GameObject FindClosestBoid(GameObject player_game_obj)
    {
        GameObject[] gos;
        gos = GameObject.FindGameObjectsWithTag("Boids");
        GameObject closest = null;
        float distance = Mathf.Infinity;
        Vector3 position = player_game_obj.transform.position;
        foreach (GameObject go in gos)
        {
            if (go.GetComponent<Rigidbody2D>().isKinematic == false)
            {
                Vector3 diff = go.transform.position - position;
                float curDistance = diff.sqrMagnitude;
                if (curDistance < distance)
                {
                    closest = go;
                    distance = curDistance;
                }
            }
        }
        return closest;
    }
}
