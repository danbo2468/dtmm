using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovieController : MonoBehaviour {

    

    // Use this for initialization
    void Start()
    {
        Handheld.PlayFullScreenMovie("Dummy 1.mp4", Color.black, FullScreenMovieControlMode.CancelOnInput);
    }

    // Update is called once per frame
    void Update () {
		
	}
}
