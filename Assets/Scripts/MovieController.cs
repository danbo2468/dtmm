using System.Collections;
using UnityEngine.UI;
using UnityEngine;

public class MovieController : MonoBehaviour {

#if UNITY_ANDROID

    public int movieNumber;

    // Use this for initialization
    void Start()
    {

    }

    // Play the file
    public void Play()
    {
        gameObject.SetActive(true);
            string file;
        if (GameManager.gameManager.characterGender == "Male")
        {
            file = "Boy" + movieNumber + ".mp4";
        } else {
            file = "Girl" + movieNumber + ".mp4";
        }
        Handheld.PlayFullScreenMovie(file);
    }

#endif
}
