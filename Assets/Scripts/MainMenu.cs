using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

using System.Collections.Generic;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour {

    // Input field for the name
    public InputField nameInputField;

    // Textfields for highscore
    public Text nr1name;
    public Text nr2name;
    public Text nr3name;
    public Text nr4name;

    public Text nr1score;
    public Text nr2score;
    public Text nr3score;
    public Text nr4score;

    // Activate or De-activate a certain panel
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
        Debug.Log("test");
        Application.Quit();
    }

    public void UpdateHighscores()
    {
        List<float> scores = GameManager.gameManager.GetHighscores();
        List<string> names = GameManager.gameManager.GetNames();
        nr1name.text = names[0];
        nr2name.text = names[1];
        nr3name.text = names[2];
        nr4name.text = names[3];

        nr1score.text = scores[0].ToString();
        nr2score.text = scores[1].ToString();
        nr3score.text = scores[2].ToString();
        nr4score.text = scores[3].ToString();
    }
}
