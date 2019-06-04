﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_switch : MonoBehaviour
{
    public float ridingBoundary;
    public GameObject rider;
    public float bumpForce1;
    public float mountVel = 1.5f;
    public Sprite[] Sprites;
    public Sprite[] buffSprites;

    private GameObject playerRider;
    private GameObject player_game_obj;
    private GameObject closestBoid;
    //False = walking, True = riding
    private bool controlType = false;
    public bool stunned;

    // Start is called before the first frame update
    void Start()
    {
        stunned = false;
        player_game_obj = GameObject.Find("Player1");
        Sprites = Resources.LoadAll<Sprite>("rider_spritemap");
        
    }

    // Update is called once per frame
    void Update()
    {
        if (player_game_obj.activeInHierarchy == true && Input.GetKeyDown(KeyCode.E) && controlType == false && stunned == false)
        {
            controlType = true;
            closestBoid = FindClosestBoid(player_game_obj);
            if (RidingBoundary(closestBoid, player_game_obj) == true && closestBoid.GetComponent<Rigidbody2D>().velocity.magnitude < mountVel)
            {
                playerRider = Instantiate(rider, closestBoid.transform.position, closestBoid.transform.rotation);
                SpriteRenderer srBuff = closestBoid.GetComponent<SpriteRenderer>();
                SpriteRenderer srRider = playerRider.GetComponent<SpriteRenderer>();
                if (srBuff.sprite == buffSprites[0])
                {
                    srRider.sprite = Sprites[1];
                }
                else if (srBuff.sprite == buffSprites[1])
                {
                    srRider.sprite = Sprites[3];
                }
                else if (srBuff.sprite == buffSprites[2])
                {
                    srRider.sprite = Sprites[5];
                }
                playerRider.GetComponent<Player_riding>().horAxis = "P1_Horizontal";
                playerRider.GetComponent<Player_riding>().verAxis = "P1_Vertical";

                //Set the opponent in script
                playerRider.GetComponent<Rider_collision>().enemyRiderStr = "Rider2";
                playerRider.tag = "Rider1";

                playerRider.GetComponent<Rigidbody2D>().velocity = closestBoid.GetComponent<Rigidbody2D>().velocity;
                closestBoid.SetActive(false);
                player_game_obj.SetActive(false);
                //WWiseBankManager.RidingMusic(gameObject);

            }
            else
            {
                controlType = false;
            }
        }
        else if(Input.GetKeyDown(KeyCode.E) && controlType == true)
        {
            Rigidbody2D rider_rb = playerRider.GetComponent<Rigidbody2D>();
            Vector2 inheritVel = rider_rb.velocity;

            controlType = false;
            closestBoid.SetActive(true);
            player_game_obj.SetActive(true);
            Vector3 dir = new Vector3(Input.GetAxis("P1_Horizontal"), Input.GetAxis("P1_Vertical"));

            closestBoid.transform.position = rider_rb.position;
            closestBoid.transform.rotation = playerRider.transform.rotation;
            closestBoid.GetComponent<Rigidbody2D>().velocity = inheritVel;

            player_game_obj.transform.position = playerRider.transform.position + dir/1.5f;

            player_game_obj.transform.rotation = Quaternion.Euler(dir);
            Destroy(playerRider);
            
        }
        else if (playerRider != null && playerRider.GetComponent<Rider_collision>().bumpOff == true && controlType == true)
        {
            
            Rigidbody2D rider_rb = playerRider.GetComponent<Rigidbody2D>();
            Rider_collision collisionScript = playerRider.GetComponent<Rider_collision>();
            Vector3 inheritVel = rider_rb.velocity;


            controlType = false;
            playerRider.SetActive(false);
            closestBoid.SetActive(true);
            player_game_obj.SetActive(true);

            this.stunned = true;
            player_game_obj.GetComponent<Player>().stunned = true;

            closestBoid.transform.position =  rider_rb.position;
            closestBoid.transform.rotation = playerRider.transform.rotation;
            closestBoid.GetComponent<Rigidbody2D>().velocity = inheritVel;

            player_game_obj.transform.position = playerRider.transform.position + (collisionScript.dir.normalized * -1.5f);
            player_game_obj.transform.rotation = playerRider.transform.rotation;
            player_game_obj.GetComponent<Rigidbody2D>().AddForce(collisionScript.dir.normalized * -bumpForce1);
            
            Destroy(playerRider);
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
