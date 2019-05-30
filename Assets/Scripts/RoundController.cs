using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class RoundController : MonoBehaviour
{

    public int NumLives = 3;
    public GameObject[] spawnPoints;
    public string next_level;

    public static int player1wins = 0;
    public static int player2wins = 0;
    
    private GameObject player1;
    private GameObject player2;
    private Player_collision player1_script;
    private Player_collision player2_script;

    private int player1_lives;
    private int player2_lives;

    private GameObject winsUI;
    
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
        GameObject.Find("Player1Lives").GetComponent<UnityEngine.UI.Text>().text = "P1 Lives: " + (NumLives-1);
        GameObject.Find("Player2Lives").GetComponent<UnityEngine.UI.Text>().text = "P2 Lives: " + (NumLives-1);

        winsUI = GameObject.Find("Wins");


        Debug.Log("about to call wwise bank...");
        WWiseBankManager.MainMusic(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        winsUI.GetComponent<UnityEngine.UI.Text>().text = player1wins + ":" + player2wins;
        if (player1_script.dead == true)
        {
            player1_lives -= 1;
            if (player1_lives <= 0)
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
                Debug.Log("Inside 0 lives");

                player2wins++;
                winsUI.GetComponent<UnityEngine.UI.Text>().text = player1wins + ":" + player2wins;
                SceneManager.LoadScene(sceneName: next_level);
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

                //WWiseBankManager.MainMusic(gameObject);
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
                player1wins++;
                winsUI.GetComponent<UnityEngine.UI.Text>().text = player1wins + ":"+ player2wins;
                SceneManager.LoadScene(sceneName: next_level);
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
                //WWiseBankManager.MainMusic(gameObject);

            }

        }
    }
}
