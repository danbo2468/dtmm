using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour {

    float currentScore;
    float collectedCoins;
    public Text scoreText;
    public int level;
    public GameObject levelEnd;
    public GameObject gameOverMenu;
    public GameObject finishedMenu;
    PlayerController player;
    private bool finished;

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

    void Update()
    {
        scoreText.text = "Score: " + (int) currentScore;
        if(player.transform.position.x > levelEnd.transform.position.x && !finished)
        {
            SaveScore();
            Finished();
        }
    }

    public void AddToScore(float points)
    {
        currentScore += points;
    }

    public void CollectedCoins(float amount)
    {
        collectedCoins += amount;
    }

    void SaveScore()
    {
        GameManager.gameManager.SetLevelHigschore(level, currentScore);
        GameManager.gameManager.AddCoinAmount(collectedCoins);
        GameManager.gameManager.Save();
    }

    void ResetScore()
    {
        currentScore = 0;
    }

    public void GameOver()
    {
        gameOverMenu.SetActive(true);
        player.SetMoveSpeed(0);
    }

    public void Finished()
    {
        finishedMenu.SetActive(true);
        player.SetMoveSpeed(0);
        finished = true;
    }

    public void RestartLevel()
    {
        int scene = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(scene, LoadSceneMode.Single);
    }

    public void GoToWorld()
    {
        SceneManager.LoadScene("Overworld");
    }
}
