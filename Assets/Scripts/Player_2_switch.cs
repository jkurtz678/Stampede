using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_2_switch : MonoBehaviour
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
        player_game_obj = GameObject.Find("Player2");
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && controlType == false)
        {
            controlType = true;
            closestBoid = FindClosestBoid(player_game_obj);
            if (RidingBoundary(closestBoid, player_game_obj) == true)
            {
                playerRider = Instantiate(rider, closestBoid.transform.position, closestBoid.transform.rotation);
                playerRider.GetComponent<Player_riding>().horAxis = "P2_Horizontal";
                playerRider.GetComponent<Player_riding>().verAxis = "P2_Vertical";
                playerRider.GetComponent<Rigidbody2D>().velocity = closestBoid.GetComponent<Rigidbody2D>().velocity;
                closestBoid.SetActive(false);
                player_game_obj.SetActive(false);

            }
            else
            {
                controlType = false;
            }
        }
        else if (Input.GetKeyDown(KeyCode.Space) && controlType == true)
        {
            Rigidbody2D rider_rb = playerRider.GetComponent<Rigidbody2D>();
            Vector2 inheritVel = rider_rb.velocity;

            controlType = false;
            playerRider.SetActive(false);
            closestBoid.SetActive(true);
            player_game_obj.SetActive(true);

            closestBoid.transform.position = rider_rb.position;
            closestBoid.transform.rotation = playerRider.transform.rotation;
            closestBoid.GetComponent<Rigidbody2D>().velocity = inheritVel;
            player_game_obj.transform.position = rider_rb.position + new Vector2(0, 1);
            player_game_obj.transform.rotation = playerRider.transform.rotation;
        }
    }

    public bool RidingBoundary(GameObject closest, GameObject player_game_obj)
    {
        Vector3 diff = closest.transform.position - player_game_obj.transform.position;
        if (diff.sqrMagnitude < ridingBoundary)
        {
            return true;
        }
        else
        {
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
