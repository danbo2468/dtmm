using UnityEngine;
using System.Collections;

public class Pickable : MonoBehaviour {

    // Object value
    public int value;

	// Use this for initialization
	void Start ()
    {
        
    }

    // Pick up the coin
    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.name == "Player")
        {
            gameObject.SetActive(false);
        }
    }
}
