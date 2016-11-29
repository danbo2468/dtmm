using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

public class GameManager : MonoBehaviour {

    int saveProfileNumber;
    string characterGender;
    string characterName;

    public static GameManager gameManager;

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

    // Set the save profile number
    public void SetSaveProfileNumber(int saveProfileNumber)
    {
        this.saveProfileNumber = saveProfileNumber;
    }

    // Set the characters gender
    public void SetCharacterGender(string characterGender)
    {
        this.characterGender = characterGender;
    }

    // Set the characters name
    public void SetCharacterName(string characterName)
    {
        this.characterName = characterName;
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

            return saveData.GetCharacterName();
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

            this.saveProfileNumber = saveData.GetSaveProfileNumber();
            this.characterGender = saveData.GetCharacterGender();
            this.characterName = saveData.GetCharacterName();
        }
    }

    // Delete a certain profile
    public void DeleteSaveProfile(int saveProfileNumber)
    {
        File.Delete(Application.persistentDataPath + "/saveProfile" + saveProfileNumber + ".dat");
    }
}


[Serializable]
class SaveData
{
    int saveProfileNumber;
    string characterGender;
    string characterName;

    float coins;
    IDictionary<int, float> levelHighscore;

    public SaveData(int saveProfileNumber, string characterGender, string characterName)
    {
        levelHighscore = new Dictionary<int, float>();
        this.saveProfileNumber = saveProfileNumber;
        this.characterGender = characterGender;
        this.characterName = characterName;
    }

    public int GetSaveProfileNumber()
    {
        return this.saveProfileNumber;
    }

    public string GetCharacterGender()
    {
        return this.characterGender;
    }

    public string GetCharacterName()
    {
        return this.characterName;
    }
}