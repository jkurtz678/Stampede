using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoundController : MonoBehaviour
{
    public int NumLives = 3;
    public GameObject[] spawnPoints;
    
    public GameObject player1;
    private GameObject player2;
    private Player_collision player1_script;
    private Player_collision player2_script;
    public int player1_lives;
    public int player2_lives;
    
    // Start is called before the first frame update
    void Start()
    {
        player1 = GameObject.Find("Player1");
        player2 = GameObject.Find("Player2");

        player1_script = player1.GetComponent<Player_collision>();
        player2_script = player2.GetComponent<Player_collision>();

        spawnPoints = GameObject.FindGameObjectsWithTag("Respawn");

        player1_lives = NumLives;
        player2_lives = NumLives;

        WWiseBankManager.MainMusic(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        if(player1_script.dead == true)
        {
            player1_lives -= 1;
            if (player2_lives <= 0)
            {
                GameObject.Find("Player1Lives").GetComponent<UnityEngine.UI.Text>().text = "P1 Lives: 0";
            }
            else
            {
                GameObject.Find("Player1Lives").GetComponent<UnityEngine.UI.Text>().text = "P1 Lives: " + (player1_lives - 1);
            }
            if (player1_lives <= 0)
            {
                //Player 2 wins round, move to next scene
                
            } else
            {
                //Respawn player
                int respawnSelect = Random.Range(1, spawnPoints.Length);
                GameObject rider2 = GameObject.FindGameObjectWithTag("Rider2");
                if (rider2 != null && Vector3.Distance(spawnPoints[respawnSelect].transform.position, GameObject.FindGameObjectWithTag("Rider2").transform.position) < 5)
                {
                    respawnSelect = Random.Range(1, spawnPoints.Length);
                }
                player1_script.dead = false;
                player1.SetActive(true);
                player1.transform.position = spawnPoints[respawnSelect].transform.position;
            }
        }

        if(player2_script.dead == true)
        {
            player2_lives -= 1;
            if(player2_lives <= 0)
            {
                GameObject.Find("Player2Lives").GetComponent<UnityEngine.UI.Text>().text = "P2 Lives: 0";
            } else
            {
                GameObject.Find("Player2Lives").GetComponent<UnityEngine.UI.Text>().text = "P2 Lives: " + (player2_lives-1);
            }
            if (player2_lives <= 0)
            {
                //Player 1 wins round, move to next scene

            } else
            {
                //Respawn player
                int respawnSelect = Random.Range(1, spawnPoints.Length);
                GameObject rider1 = GameObject.FindGameObjectWithTag("Rider1");
                if (rider1 != null && Vector3.Distance(spawnPoints[respawnSelect].transform.position, rider1.transform.position) < 5)
                {
                    respawnSelect = Random.Range(1, spawnPoints.Length);
                }
                player2_script.dead = false;
                player2.SetActive(true);
                
                player2.transform.position = spawnPoints[respawnSelect].transform.position;
            }
            
        }
    }
}
