using System.Collections;
using UnityEngine.UI;
using UnityEngine;

[RequireComponent (typeof(AudioSource))]

public class MovieController : MonoBehaviour {

    #if UNITY_ANDROID
        public int movieNumber;
    #else
        public MovieTexture movieMale;
        public MovieTexture movieFemale;
        private MovieTexture movie;
        private AudioSource audio;
    #endif

    // Use this for initialization
    void Start()
    {
        #if UNITY_ANDROID
            string file;
            if (GameManager.gameManager.characterGender == "Male")
            {
                file = "Boy" + movieNumber + ".mp4";
            } else {
                file = "Girl" + movieNumber + ".mp4";
            }
            Handheld.PlayFullScreenMovie(file);
        #else
            if(GameManager.gameManager.characterGender == "Male")
            {
                movie = movieMale;
            } else {
                movie = movieFemale;
            }

            GetComponent<RawImage>().texture = movie;
            audio = GetComponent<AudioSource>();
            audio.clip = movie.audioClip;
            movie.Play();
            audio.Play();
        #endif
    }

    // Update is called once per frame
    void Update () {
        #if UNITY_ANDROID
        
        #else
            if (!movie.isPlaying)
            {
                if (GameManager.gameManager.levelManager.isFinished)
                {
                    GameManager.gameManager.levelManager.finishedMenu.SetActive(true);
                    GameManager.gameManager.levelManager.makeHeartsShrink = true;
                    gameObject.SetActive(false);
                }
                else
                {
                    gameObject.SetActive(false);
                    GameManager.gameManager.levelManager.Resume();
                }
            }
        #endif
    }

    // Play the file
    public void Play()
    {
        gameObject.SetActive(true);
    }
}
