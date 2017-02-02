using System.Collections;
using UnityEngine.UI;
using UnityEngine;

public class MovieController : MonoBehaviour {

    public int movieNumber;

    // Use this for initialization
    void Start()
    {
        string file;
        if (GameManager.gameManager.characterGender == "Male")
        {
            file = "Boy" + movieNumber + ".mp4";
        } else {
            file = "Girl" + movieNumber + ".mp4";
        }
        Handheld.PlayFullScreenMovie(file);
    }

    // Play the file
    public void Play()
    {
        gameObject.SetActive(true);
    }
}
