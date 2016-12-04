using UnityEngine;
using System.Collections;

public class Collectable : MonoBehaviour {

    // Object value
    public int value;

    // Pick up the coin
    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.tag == "Player")
        {
            GameManager.gameManager.scoreManager.AddToScore(value);
            GameManager.gameManager.scoreManager.CollectedCoins(1);
            gameObject.SetActive(false);
        }
    }
}
