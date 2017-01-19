using System.Collections;
using UnityEngine.UI;
using UnityEngine;

[RequireComponent (typeof(AudioSource))]

public class MovieController : MonoBehaviour {

    public int movieToBePlayed;
    public MovieTexture[] movies;
    private AudioSource audio;
    public string gender;

    // Use this for initialization
    void Start()
    {
        int movieToPlay;
        if(gender == "Male")
        {
            movieToPlay = (movieToBePlayed - 1) * 2;
        } else
        {
            movieToPlay = ((movieToBePlayed - 1) * 2) + 1;
        }
        MovieTexture movie = movies[movieToPlay] as MovieTexture;
        GetComponent<RawImage>().texture = movie;
        audio = GetComponent<AudioSource>();
        audio.clip = movie.audioClip;
        movie.Play();
        audio.Play();
    }

    // Update is called once per frame
    void Update () {
		
	}
}
