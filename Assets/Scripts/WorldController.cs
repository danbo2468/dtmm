using UnityEngine;
using System.Collections;

public class WorldController : MonoBehaviour {
    [SerializeField]
    public GameObject player;

	// Use this for initialization
	void Start () {
	    // Ask GameManager for all level completions and highscores.
	}
	
	// Update is called once per frame
	void Update () {
        HandleMovement();
	}

    public void HandleMovement()
    {

    }
    
}
