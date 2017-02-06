using UnityEngine;
using System.Collections;

public class Collectable : MonoBehaviour {

    // Object value
    public int scoreValue;
    public int coinValue;

    // Pick up the coin
    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.tag == "Player")
        {

            GameManager.gameManager.levelManager.AddToScore(scoreValue);
            GameManager.gameManager.levelManager.CollectedCoins(coinValue);
            gameObject.SetActive(false);
        }
    }
}
