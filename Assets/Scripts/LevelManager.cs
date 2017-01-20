using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour {

    // Score
    float currentScore;
    float highScore;
    float collectedCoins;
    bool scoreSaved;
    public Text scoreText;
    public Text highscoreText;

    // Level number
    public int level;

    // End of level
    public GameObject levelEnd;
    public bool isFinished = false;
    public bool isGameOver = false;

    // Dialogs
    public GameObject gameOverMenu;
    public GameObject finishedMenu;
    public GameObject mainMenu;
    private PlayerController player;

    // Still Water Mosquitoes
    public StillWaterMosquito[] stillWaterMosquitoes;

    // Hearts
    public Transform heart1;
    public Transform heart2;
    public Transform heart3;
    public Transform heart4;
    public Transform heart5;

    // Shrinking hearts
    public bool makeHeartsShrink;
    public bool shrinking;
    public float targetScale = 1.9f;
    public float shrinkSpeed = 2.0f;

    // UI Score calculation
    public Text oldScoreText;
    public Text newScoreText;
    public List<Transform> heartCountList;

    // Cutscenes
    public MovieController cutsceneBefore;
    public MovieController cutsceneAfter;

    // Use this for initialization
    void Start () {
        stillWaterMosquitoes = FindObjectsOfType<StillWaterMosquito>();
        player = FindObjectOfType<PlayerController>();
        currentScore = 0;
        collectedCoins = 0;
        highScore = GameManager.gameManager.levelHighscores[level];
        GameManager.gameManager.SetLevelManager(this);
        highscoreText.text = "Highest score: " + (int)highScore;

        if (cutsceneBefore != null)
        {
            Pause();
            cutsceneBefore.Play();
        }
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

        if (isFinished && makeHeartsShrink)
        {
            heartCountList = player.hearts;
            Transform target = heart1;
            for (int i = 1; i <= heartCountList.Count; i++)
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

                heartCountList[i - 1].transform.position = Vector2.MoveTowards(new Vector2(heartCountList[i - 1].transform.position.x, heartCountList[i - 1].transform.position.y), new Vector2(target.position.x, target.position.y), 5.0f * Time.deltaTime);
                if (heartCountList[i - 1].transform.localScale.x > targetScale)
                { 
                    heartCountList[i - 1].transform.localScale -= Vector3.one * Time.deltaTime * shrinkSpeed;
                }
            }
            if (!scoreSaved)
            {
                SaveScore();
                scoreSaved = true;
            } 
            newScoreText.text = "Score: " + (int) calculateScore();
        }
    }

    // Play an animation for mosquitoes attacking the character after falling into still water
    public void AttackPlayer()
    {
        for (int i = 0; i < stillWaterMosquitoes.Length; i++)
        {
            stillWaterMosquitoes[i].GameOver();
        }
    }

    public void StillWaterGameOver()
    {
        StartCoroutine(ShowStillWaterGameOver());
    }

    // Show gameover menu after falling into still water
    System.Collections.IEnumerator ShowStillWaterGameOver()
    {
        AttackPlayer();
        isGameOver = true;
        player.SetMoveSpeed(0);
        yield return new WaitForSeconds(2);
        gameOverMenu.SetActive(true);
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

    // Pause the game
    public void Pause()
    {
        player.SetMoveSpeed(0);
        player.canShoot = false;
        player.jumpForce = 0;
    }

    // Resume the game
    public void Resume()
    {
        player.SetMoveSpeed(player.initialMoveSpeed);
        player.canShoot = true;
        player.jumpForce = 20;
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
        player.SetMoveSpeed(0);
        isFinished = true;
        if (cutsceneAfter != null)
        {
            cutsceneAfter.Play();
        }
        else
        {
            finishedMenu.SetActive(true);
        }
        oldScoreText.text = currentScore.ToString();
        
    }

    public void Continue()
    {
        mainMenu.SetActive(false);
        Resume();
    }

    public void Menu()
    {
        Pause();
        mainMenu.SetActive(true);
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
