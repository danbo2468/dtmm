using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour {

    public static GameManager gameManager;

	// Use this for initialization
	void Awake () {
        if(gameManager == null)
        {
            DontDestroyOnLoad(gameObject);
            gameManager = this;
        }
        else if(gameManager != this)
        {
            Destroy(gameObject);
        }
	}
}
