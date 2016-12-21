using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour {

    // Score
    float currentScore;
    float highScore;
    float collectedCoins;
    public Text scoreText;
    public Text highscoreText;

    // Level number
    public int level;

    // End of level
    public GameObject levelEnd;
    public bool isFinished;
    public bool isGameOver;

    // Dialogs
    public GameObject gameOverMenu;
    public GameObject finishedMenu;
    private PlayerController player;

    // Hearts
    public Transform heart1;
    public Transform heart2;
    public Transform heart3;
    public Transform heart4;
    public Transform heart5;

    // Shrinking hearts
    public bool shrinking;
    public float targetScale = 1.9f;
    public float shrinkSpeed = 2.0f;

    // UI Score calculation
    public Text oldScoreText;
    public Text newScoreText;

    void SetShrink(bool boolean)
    {
        shrinking = boolean;
    }

    // Use this for initialization
    void Start () {
        player = FindObjectOfType<PlayerController>();
        currentScore = 0;
        collectedCoins = 0;
        highScore = GameManager.gameManager.levelHighscores[level];
        GameManager.gameManager.SetLevelManager(this);
        highscoreText.text = "Highest score: " + (int)highScore;
    }

    // Call this every frame
    void Update()
    {
        // Update the score
        scoreText.text = "Score: " + (int) currentScore;

        // Check if the level is finished
        if (player.transform.position.x > levelEnd.transform.position.x && !isFinished)
        {  
            Finished();
        }

        if (isFinished)
        {
            SetShrink(false);
            List<Transform> temp = player.hearts;
            Debug.Log(temp.Count);
            Transform target = heart1;
            for (int i = 1; i <= temp.Count; i++)
            {
                if (i == 1)
                    target = heart1;

                else if (i == 2)
                    target = heart2;

                else if (i == 3)
                    target = heart3;

                else if (i == 4)
                    target = heart4;

                else if (i == 5)
                    target = heart5;
     
               temp[i - 1].transform.position = Vector2.MoveTowards(new Vector2(temp[i - 1].transform.position.x, temp[i - 1].transform.position.y), new Vector2(target.position.x, target.position.y), 5.0f * Time.deltaTime);
                if (temp[i - 1].transform.localScale.x > targetScale)
                {
                    SetShrink(true);
                    temp[i - 1].transform.localScale -= Vector3.one * Time.deltaTime * shrinkSpeed;   
                }
            }    
        }
    }

    // Add a certain amount of coins to the score
    public void AddToScore(float points)
    {
        currentScore += points;
    }

    // Add a certain amount of coins
    public void CollectedCoins(float amount)
    {
        collectedCoins += amount;
    }

    // Save the score in the GameManager
    void SaveScore()
    {
        GameManager.gameManager.SetLevelHigschore(level, calculateScore());
        GameManager.gameManager.AddCoinAmount(collectedCoins);
        GameManager.gameManager.Save();
    }

    // Show a Game Over dialog
    public void GameOver()
    {
        isGameOver = true;
        gameOverMenu.SetActive(true);
        player.SetMoveSpeed(0);
    }

    // Show a finish dialog
    public void Finished()
    {
        finishedMenu.SetActive(true);
        player.SetMoveSpeed(0);
        isFinished = true;
        oldScoreText.text = currentScore.ToString();
        if (!shrinking)
        {
            SaveScore();
            newScoreText.text = "Score: " + calculateScore();
        }
    }

    // Restart this level
    public void RestartLevel()
    {
        int scene = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(scene, LoadSceneMode.Single);
    }

    // Go back to the worldss
    public void GoToWorld()
    {
        SceneManager.LoadScene("Overworld");
    }

    // Calculates score based on current score and player health.
    // Example:
    // Score: 50, player health = 4. Return 50 * (4 / 2) = 100.
    public float calculateScore()
    {
        return currentScore * (player.health / 2);
    }
}
