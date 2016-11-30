using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour {

    public InputField nameInputField;

    // Go to the Save Profile scene
    public void SwitchToSaveProfileScene()
    {
        SceneManager.LoadScene("Choose Save Profile");
    }

    // Go to the Save As scene
    public void SwitchToSaveAsScene()
    {
        SceneManager.LoadScene("Save As");
    }

    // Go to the Gender Selection scene
    public void SwitchToGenderSelectionScene()
    {
        SceneManager.LoadScene("Gender Selection");
    }

    // Set the chosen gender
    public void SetGender(string gender)
    {
        GameManager.gameManager.characterGender = gender;
    }

    // Go to the Name Selection scene
    public void SwitchToNameSelectionScene()
    {
        SceneManager.LoadScene("Name Selection");
    }

    // Set the chosen name
    public void SetName()
    {
        GameManager.gameManager.characterName = nameInputField.text;
    }

    // Save the player data
    public void SaveData()
    {
        GameManager.gameManager.Save();
        SceneManager.LoadScene("Tutorial");
    }

    // Set save profile number
    public void SetSaveProfile(int saveProfile)
    {
        GameManager.gameManager.saveProfileNumber = saveProfile;
    }
    
    // Load the player data
    public void LoadGame(int saveProfile)
    {
        GameManager.gameManager.LoadSaveProfile(saveProfile);
        SceneManager.LoadScene("Tutorial");
    }

    // Delete the player data
    public void DeleteSaveProfile(int saveProfile)
    {
        GameManager.gameManager.DeleteSaveProfile(saveProfile);
        int scene = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(scene, LoadSceneMode.Single);
    }

    // Go to the Settings Scene
    public void SwitchToSettingsScene()
    {
        SceneManager.LoadScene("Settings");
    }

    // Save the game settings
    public void SaveSettings()
    {
        SceneManager.LoadScene("Welcome Screen");
    }


    // Exit the game
    public void QuitGame()
    {
        Application.Quit();
    }
}
