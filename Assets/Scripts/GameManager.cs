using UnityEngine;
using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

public class GameManager : MonoBehaviour {

    // Save profile settings
    public int saveProfileNumber;
    public string characterGender;
    public string characterName;
    public float[] levelHighscores;
    public float coins;
	public bool[] boughtItems;

    // Game settings
    public bool backgroundMusic;
    public bool soundEffects;

    // Mechanics
    public static GameManager gameManager;
    public LevelManager levelManager;

    // WorldController
    public Vector3 worldNode; // player is at this worldNode;
    public Vector3 levelNode; // player is at this levelNode;

    // heartCount
    public int heartCount;

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

    void Start()
    {
        heartCount = 2;
		levelHighscores = new float[14];
		boughtItems = new bool[2];
        //worldNode = new Vector3(0, 0, 0);
        //levelNode = new Vector3(0, 0, 0);

        // ssLoad the Player Prefs
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

    public int getHeartCount()
    {
        return heartCount;
    }

    public void addHeartCount()
    {
        heartCount++;
    }

	public void BoughtItem(int productID)
	{
		boughtItems[productID] = true;
	}

    // Save the game status
    public void Save()
    {

        // Create a new save file
        BinaryFormatter formatter = new BinaryFormatter();
        FileStream file = File.Create(Application.persistentDataPath + "/saveProfile" + this.saveProfileNumber + ".dat");

        // Save the data to the file
		SaveData saveData = new SaveData(this.saveProfileNumber, this.characterGender, this.characterName, this.levelHighscores, this.coins, this.boughtItems, Vector3ToArray(this.worldNode), Vector3ToArray(this.levelNode), heartCount);

        // Close the file
        formatter.Serialize(file, saveData);
        file.Close();
    }

    // Load the character's name of a certain profile
    public string LoadSaveProfileName(int saveProfileNumber)
    {
        // Check if the save file exists
        if (File.Exists(Application.persistentDataPath + "/saveProfile" + saveProfileNumber + ".dat"))
        {

            // Open the file and load the name
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + "/saveProfile" + saveProfileNumber + ".dat", FileMode.Open);
            SaveData saveData = (SaveData)formatter.Deserialize(file);
            file.Close();

            return saveData.characterName;
        }

        else
        {   
            // Return null
            return null;
        }
    }

    // Load a certain profile into the game manager
    public void LoadSaveProfile(int saveProfileNumber)
    {

        // Check if the save file exists
        if (File.Exists(Application.persistentDataPath + "/saveProfile" + saveProfileNumber + ".dat"))
        {
            // Load the information from the file
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + "/saveProfile" + saveProfileNumber + ".dat", FileMode.Open);
            SaveData saveData = (SaveData)formatter.Deserialize(file);
            file.Close();

            // Load the save file information into the game manager
            this.saveProfileNumber = saveData.saveProfileNumber;
            this.characterGender = saveData.characterGender;
            this.characterName = saveData.characterName;
            this.levelHighscores = saveData.levelHighscores;
            this.coins = saveData.coins;
            this.worldNode = SetArrayToTransform(saveData.worldNode);
            this.levelNode = SetArrayToTransform(saveData.levelNode);
            this.heartCount = saveData.heartCount;

            Debug.Log("We've loaded a file! The settings are: ");
            Debug.Log("WorldNode: " + worldNode + " and levelNode: " + levelNode);
        }
    }

    // Delete a certain profile
    public void DeleteSaveProfile(int saveProfileNumber)
    {
        File.Delete(Application.persistentDataPath + "/saveProfile" + saveProfileNumber + ".dat");
    }

    // Set the level manager for this level
    public void SetLevelManager(LevelManager levelManager)
    {
        this.levelManager = levelManager;
    }

    // Set the highscore for a certain level
    public void SetLevelHigschore(int level, float highscore)
    {
        if (highscore > levelHighscores[level])
        {
            levelHighscores[level] = highscore;
        }
    }

    // Add coins to the current amount
    public void AddCoinAmount(float collectedCoins)
    {
        coins += collectedCoins;
    }

    // Convert a Vector3 to an Array
    public float[] Vector3ToArray(Vector3 vector)
    {
        return new float[3] { vector.x, vector.y, vector.z };      
    }

    // Convert an Array to a Vector3
    public Vector3 SetArrayToTransform(float[] array)
    {
        return new Vector3(array[0], array[1], array[2]);
    }

    // Set the position in the world
    public void SetWorldPosition(Vector3 position)
    {
        this.worldNode = position;
    }

    // Set the position in a section
    public void SetLevelPosition(Vector3 position)
    {
        this.levelNode = position;
    }
}

// The format of save files
[Serializable]
class SaveData
{
    public int saveProfileNumber;
    public string characterGender;
    public string characterName;
    public float[] levelHighscores;
    public float coins;
	public bool[] boughtItems;
    public float[] worldNode;
    public float[] levelNode;
    public int heartCount;

	public SaveData(int saveProfileNumber, string characterGender, string characterName, float[] levelHighscores, float coins, bool[] boughtItems, float[] worldNode, float[] levelNode, int heartCount)
    {
        this.saveProfileNumber = saveProfileNumber;
        this.characterGender = characterGender;
        this.characterName = characterName;
        this.levelHighscores = levelHighscores;
        this.coins = coins;
		this.boughtItems = boughtItems;
        this.worldNode = worldNode;
        this.levelNode = levelNode;
        this.heartCount = heartCount;
    }
}