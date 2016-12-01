using UnityEngine;
using System.Collections;

public class Collectable : MonoBehaviour {

    // Object value
    public int value;

    // Score manager
    private ScoreManager scoreManager;

	// Use this for initialization
	void Start ()
    {
        scoreManager = FindObjectOfType<ScoreManager>();
    }

    // Pick up the coin
    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.tag == "Player")
        {
            ScoreManager.scoreManager.AddToScore(value);
            ScoreManager.scoreManager.CollectedCoins(1);
            gameObject.SetActive(false);
        }
    }
}
