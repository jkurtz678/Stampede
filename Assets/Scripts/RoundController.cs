using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class RoundController : MonoBehaviour
{

    public int NumLives = 3;
    public GameObject[] spawnPoints;
    public string next_level;
    public int maxWins = 3;
    public Text textPrefab;
    private GameObject renderCanvas;

    public static int player1wins = 0;
    public static int player2wins = 0;
    
    private GameObject player1;
    private GameObject player2;
    private Player_collision player1_script;
    private Player_collision player2_script;
    private Text p1LivesString;
    private Text p2LivesString;

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
        p1LivesString = GameObject.Find("Player1Lives").GetComponent<Text>();
        p2LivesString = GameObject.Find("Player2Lives").GetComponent<Text>();
        p1LivesString.text = "P1 Lives: " + (NumLives - 1);
        p2LivesString.text = "P2 Lives: " + (NumLives - 1);
        renderCanvas = GameObject.Find("Canvas");

        winsUI = GameObject.Find("Wins");


        Debug.Log("about to call wwise bank...");
        WWiseBankManager.MainMusic(gameObject);
    }

    // Update is called once per frame
    void Update()
    {

        
        if(winsUI)
        {
            winsUI.GetComponent<Text>().text = player1wins + ":" + player2wins;
        }
        if (player1_script.dead == true)
        {
            player1_lives -= 1;
            WWiseBankManager.PlayerDeath(gameObject);

            if (player1_lives <= 0)
                p1LivesString.text = "P1 Lives: 0";
            else
            {
                p1LivesString.text = "P1 Lives: " + (player1_lives - 1);
            }
            if (player1_lives <= 0)
            {
                //Player 2 wins round, move to next scene
                player2wins = Mathf.Clamp(player2wins + 1, 0, maxWins);
                winsUI.GetComponent<Text>().text = player1wins + ":" + player2wins;
                CheckWins();
                //SceneManager.LoadScene(sceneName: next_level);
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
            WWiseBankManager.PlayerDeath(gameObject);
            if (player2_lives <= 0)
            {
                p2LivesString.text = "P2 Lives: 0";
            } else
            {
                p2LivesString.text = "P2 Lives: " + (player2_lives-1);
            }
            if (player2_lives <= 0)
            {
                //Player 1 wins round, move to next scene
                player1wins = Mathf.Clamp(player1wins + 1, 0, maxWins);
                winsUI.GetComponent<Text>().text = player1wins + ":"+ player2wins;
                CheckWins();
                //SceneManager.LoadScene(sceneName: next_level);
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

    public void CheckWins()
    {
        if ((player1wins >= maxWins) || (player2wins >= maxWins))
        {
            if (player1wins > player2wins)
            {
                winsUI.GetComponent<Text>().text = "P1 Wins\n" + player1wins + ":" + player2wins;

            } else
            {
                winsUI.GetComponent<Text>().text = "P2 Wins\n" + player1wins + ":" + player2wins;
            }
            winsUI.GetComponent<Text>().fontSize = 40;
            winsUI.GetComponent<Text>().alignment = TextAnchor.LowerCenter;
            
            Text tempTextBox = Instantiate(textPrefab, Vector3.zero, transform.rotation) as Text;
            //Parent to the panel
            tempTextBox.transform.SetParent(renderCanvas.transform, false);
            //Set the text box's text element font size and style:
            tempTextBox.fontSize = 32;
            //Set the text box's text element to the current textToDisplay:
            tempTextBox.text = "Press space to restart";
            Time.timeScale = 0.001f;
            if (Input.GetKeyDown("space"))
            {
                player1wins = 0;
                player2wins = 0;
                
               
                SceneManager.LoadScene(0);
                Time.timeScale = 1.0f;
            } 
        } else
        {
            SceneManager.LoadScene(sceneName: next_level);
        }
    }
}
