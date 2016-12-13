using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour {

    public InputField nameInputField;


    public void TogglePanel(GameObject panel)
    {
        panel.SetActive(!panel.activeSelf);
    }

    // Set the chosen gender
    public void SetGender(string gender)
    {
        GameManager.gameManager.characterGender = gender;
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
        SceneManager.LoadScene("Overworld");
    }

    // Delete the player data
    public void DeleteSaveProfile(int saveProfile)
    {
        GameManager.gameManager.DeleteSaveProfile(saveProfile);
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
