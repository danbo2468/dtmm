using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour {

    float currentScore;
    float collectedCoins;
    public Text scoreText;
    public static ScoreManager scoreManager;

    // Use this for initialization
    void Start () {
        currentScore = 0;
        collectedCoins = 0;
        GameManager.gameManager.SetScoreManager(this);
	}

    void Update()
    {
        scoreText.text = "Score: " + (int) currentScore;
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

    }

    void ResetScore()
    {
        currentScore = 0;
    }
}
