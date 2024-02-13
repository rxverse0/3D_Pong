using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Ball : MonoBehaviour
{
    private Vector3 direction;
    public float speed;

    [SerializeField]
    private int playerOneScore;
    [SerializeField]
    private int playerTwoScore;
    public Vector3 spawnPoint;
    public GameObject goalEffect;

    public TextMeshProUGUI playerOneText;
    public TextMeshProUGUI playerTwoText;
    public AudioSource[] audioPlayers; 
    public TextMeshProUGUI winText;
    public Button restartButton;

    // Start is called before the first frame update
    void Start()
    {
        playerOneScore = 0;
        playerTwoScore = 0;
        this.direction = new Vector3 (1f, 1f, 1f);
        winText.gameObject.SetActive(false);
        restartButton.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        this.transform.position += direction * speed;
        playerOneText.text = playerOneScore.ToString();
        playerTwoText.text = playerTwoScore.ToString();
        if (playerOneScore >= 5 || playerTwoScore >= 5)
        {
            HandleWin();
        }
    }

    void OnCollisionEnter (Collision col)
    {
        Vector3 normal = col.contacts[0].normal;
        direction = Vector3.Reflect(direction, normal);
        if(col.gameObject.name == "Player1" || col.gameObject.name == "Player2")
        {
            audioPlayers[0].Play();
        }
        if(col.gameObject.name == "WestWall")
        {
            playerTwoScore++;
            Instantiate(goalEffect, this.transform.position, Quaternion.identity);
            ResetBall();
        }
        if(col.gameObject.name == "EastWall")
        {
            playerOneScore++;
            Instantiate(goalEffect, this.transform.position, Quaternion.identity);
            ResetBall();
        }
    }

    void ResetBall()
    {
        audioPlayers[1].Play();           
        transform.position = spawnPoint;
        transform.position = new Vector3(transform.position.x, 0.75f, transform.position.z);
    }

    void HandleWin()
    {
        // Display victory message
        if (playerOneScore >= 5)
        {
            winText.text = "Player 1 Wins! Try again?";
        }
        else
        {
            winText.text = "Player 2 Wins! Try again?";
        }

        // Enable UI elements
        winText.gameObject.SetActive(true);
        restartButton.gameObject.SetActive(true);

        // Disable the script to stop further updates
        enabled = false;
    }

    // Function to be called when the restart button is clicked
    public void RestartGame()
    {
        // Reset scores
        playerOneScore = 0;
        playerTwoScore = 0;

        // Hide UI elements
        winText.gameObject.SetActive(false);
        restartButton.gameObject.SetActive(false);

        // Enable the script to resume updates
        enabled = true;
    }
}
