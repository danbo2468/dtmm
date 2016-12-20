using UnityEngine;
using System.Collections;
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
