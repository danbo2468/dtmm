using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

public class GameManager : MonoBehaviour {

    // Save profile settings
    public int saveProfileNumber;
    public string characterGender;
    public string characterName;

    // Game settings
    public bool backgroundMusic;
    public bool soundEffects;

    // Mechanics
    public static GameManager gameManager;
    public ScoreManager scoreManager;

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

    // Load the Player Prefs
    void Start()
    {
        if (PlayerPrefs.HasKey("Background Music"))
        {
            if (PlayerPrefs.GetInt("Background Music") == 1)
            {
                backgroundMusic = true;
            }
            else
            {
                backgroundMusic = false;
            }
        }
        else
        {
            backgroundMusic = true;
        }

        if (PlayerPrefs.HasKey("Sound Effects"))
        {
            if (PlayerPrefs.GetInt("Sound Effects") == 1)
            {
                soundEffects = true;
            }
            else
            {
                soundEffects = false;
            }
        }
        else
        {
            soundEffects = true;
        }
    } 

    // Save the game status
    public void Save()
    {
        BinaryFormatter formatter = new BinaryFormatter();
        FileStream file = File.Create(Application.persistentDataPath + "/saveProfile" + this.saveProfileNumber + ".dat");

        SaveData saveData = new SaveData(this.saveProfileNumber, this.characterGender, this.characterName);

        formatter.Serialize(file, saveData);
        file.Close();
    }

    // Load the character's name of a certain profile
    public string LoadSaveProfileName(int saveProfileNumber)
    {
        if (File.Exists(Application.persistentDataPath + "/saveProfile" + saveProfileNumber + ".dat"))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + "/saveProfile" + saveProfileNumber + ".dat", FileMode.Open);
            SaveData saveData = (SaveData)formatter.Deserialize(file);
            file.Close();

            return saveData.characterName;
        }
        else
        {
            return null;
        }
    }

    // Load a certain profile into the game manager
    public void LoadSaveProfile(int saveProfileNumber)
    {
        if (File.Exists(Application.persistentDataPath + "/saveProfile" + saveProfileNumber + ".dat"))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + "/saveProfile" + saveProfileNumber + ".dat", FileMode.Open);
            SaveData saveData = (SaveData)formatter.Deserialize(file);
            file.Close();

            this.saveProfileNumber = saveData.saveProfileNumber;
            this.characterGender = saveData.characterGender;
            this.characterName = saveData.characterName;
        }
    }

    // Delete a certain profile
    public void DeleteSaveProfile(int saveProfileNumber)
    {
        File.Delete(Application.persistentDataPath + "/saveProfile" + saveProfileNumber + ".dat");
    }

    // Set the score manager for this level
    public void SetScoreManager(ScoreManager scoreManager)
    {
        this.scoreManager = scoreManager;
    }

    // Relaod the level
    public void RestartLevel()
    {
        int scene = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(scene, LoadSceneMode.Single);
    }
}


[Serializable]
class SaveData
{
    public int saveProfileNumber;
    public string characterGender;
    public string characterName;

    float coins;
    IDictionary<int, float> levelHighscore;

    public SaveData(int saveProfileNumber, string characterGender, string characterName)
    {
        levelHighscore = new Dictionary<int, float>();
        this.saveProfileNumber = saveProfileNumber;
        this.characterGender = characterGender;
        this.characterName = characterName;
    }
}