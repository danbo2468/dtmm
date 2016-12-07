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
    PlayerController player;

    // Use this for initialization
    void Start () {
        player = FindObjectOfType<PlayerController>();
        currentScore = 0;
        collectedCoins = 0;
        GameManager.gameManager.SetLevelManager(this);
	}

    void Update()
    {
        scoreText.text = "Score: " + (int) currentScore;
        if(player.transform.position.x > levelEnd.transform.position.x)
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
        int scene = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(scene, LoadSceneMode.Single);
    }

    public void Finished()
    {
        int scene = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(scene, LoadSceneMode.Single);
    }
}
