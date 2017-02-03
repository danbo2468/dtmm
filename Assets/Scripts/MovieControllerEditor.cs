﻿using System.Collections;
using UnityEngine.UI;
using UnityEngine;

[RequireComponent (typeof(AudioSource))]

public class MovieControllerEditor : MonoBehaviour {

#if UNITY_EDITOR

    public MovieTexture movieMale;
    public MovieTexture movieFemale;
    private MovieTexture movie;
    private AudioSource audio;

    // Use this for initialization
    void Start()
    {
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
    }

    // Update is called once per frame
    void Update () {
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
    }

    // Play the file
    public void Play()
    {
        gameObject.SetActive(true);
    }

#endif
}