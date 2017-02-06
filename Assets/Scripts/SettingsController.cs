using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class SettingsController : MonoBehaviour {

    public Toggle backgroundMusicToggle;
    public Toggle soundEffectsToggle;

	// Use this for initialization
	void Start () {
	    if(PlayerPrefs.HasKey("Background Music"))
        {
            if(PlayerPrefs.GetInt("Background Music") == 1)
            {
                backgroundMusicToggle.isOn = true;
            }
            else
            {
                backgroundMusicToggle.isOn = false;
            }
        }
        else
        {
            backgroundMusicToggle.isOn = true;
        }

        if (PlayerPrefs.HasKey("Sound Effects"))
        {
            if (PlayerPrefs.GetInt("Sound Effects") == 1)
            {
                soundEffectsToggle.isOn = true;
            }
            else
            {
                soundEffectsToggle.isOn = false;
            }
        }
        else
        {
            soundEffectsToggle.isOn = true;
        }
    }

    public void SaveSettings()
    {
        int backgroundMusic;
        if (backgroundMusicToggle.isOn)
        {
            backgroundMusic = 1;
        } else
        {
            backgroundMusic = 0;
        }
        PlayerPrefs.SetInt("Background Music", backgroundMusic);

        int soundEffects;
        if (soundEffectsToggle.isOn)
        {
            soundEffects = 1;
        }
        else
        {
            soundEffects = 0;
        }
        PlayerPrefs.SetInt("Sound Effects", soundEffects);

        GameManager.gameManager.backgroundMusic = backgroundMusicToggle.isOn;
        GameManager.gameManager.soundEffects = soundEffectsToggle.isOn;
    }
}
