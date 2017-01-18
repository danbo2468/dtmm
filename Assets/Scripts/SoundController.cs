using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundController : MonoBehaviour {

    AudioSource soundtrack;

	// Use this for initialization
	void Start () {
        soundtrack = GetComponent<AudioSource>();
        Debug.Log(GameManager.gameManager.backgroundMusic);
	}
	
	// Update is called once per frame
	void Update () {
        if (GameManager.gameManager.backgroundMusic)
        {
            soundtrack.volume = 1;
            Debug.Log("FUCKING SONG SHOULD PLAY");
        } else {
            soundtrack.volume = 0;
        }
    }
}
