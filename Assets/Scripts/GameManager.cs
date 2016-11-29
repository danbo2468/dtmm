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

    public void setCharacterName(string characterName)
    {
        this.characterName = characterName;
    }

    public void setCharacterGender(string characterGender)
    {
        this.characterGender = characterGender;
    }

    public void Save(int saveProfileNumber)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        FileStream file = File.Create(Application.persistentDataPath + "/saveProfile" + saveProfileNumber + ".dat");

        SaveData saveData = new SaveData(this.saveProfileNumber, this.characterGender, this.characterName);

        formatter.Serialize(file, saveData);
        file.Close();
    }

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