using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour {

    float currentScore;
    float collectedCoins;
    Text scoreText;
    public static ScoreManager scoreManager;

    // Use this for initialization
    void Awake () {
        if (scoreManager == null)
        {
            DontDestroyOnLoad(gameObject);
            scoreManager = this;
        }
        else if (scoreManager != this)
        {
            Destroy(gameObject);
        }
        scoreText = GetComponent<Text>();
        currentScore = 0;
        collectedCoins = 0;
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
