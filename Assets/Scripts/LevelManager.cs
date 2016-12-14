using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour {

    // Score
    float currentScore;
    float collectedCoins;
    public Text scoreText;

    // Level number
    public int level;

    // End of level
    public GameObject levelEnd;
    private bool finished;

    // Dialogs
    public GameObject gameOverMenu;
    public GameObject finishedMenu;
    private PlayerController player;

    // Use this for initialization
    void Start () {
        player = FindObjectOfType<PlayerController>();
        currentScore = 0;
        collectedCoins = 0;
        GameManager.gameManager.SetLevelManager(this);
        gameOverMenu.SetActive(false);
        finishedMenu.SetActive(false);
        finished = false;
	}

    // Call this every frame
    void Update()
    {
        // Update the score
        scoreText.text = "Score: " + (int) currentScore;

        // Check if the level is finished
        if(player.transform.position.x > levelEnd.transform.position.x && !finished)
        {
            SaveScore();
            Finished();
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
        GameManager.gameManager.SetLevelHigschore(level, currentScore);
        GameManager.gameManager.AddCoinAmount(collectedCoins);
        GameManager.gameManager.Save();
    }

    // Show a Game Over dialog
    public void GameOver()
    {
        gameOverMenu.SetActive(true);
        player.SetMoveSpeed(0);
    }

    // Show a finish dialog
    public void Finished()
    {
        finishedMenu.SetActive(true);
        player.SetMoveSpeed(0);
        finished = true;
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
}
