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

    // Go to the Gender Selection scene
    public void SwitchToGenderSelectionScene()
    {
        SceneManager.LoadScene("Gender Selection");
    }

    // Set the chosen gender
    public void SetGender(string gender)
    {
        GameManager.gameManager.SetCharacterGender(gender);
    }

    // Go to the Name Selection scene
    public void SwitchToNameSelectionScene()
    {
        SceneManager.LoadScene("Name Selection");
    }

    // Set the chosen name
    public void SetName()
    {
        GameManager.gameManager.SetCharacterName(nameInputField.text);
        SceneManager.LoadScene("Save As");
    }

    // Save the player data
    public void SaveData(int saveProfile)
    {
        GameManager.gameManager.SetSaveProfileNumber(saveProfile);
        GameManager.gameManager.Save();
        SceneManager.LoadScene("Tutorial");
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


    // Exit the game
    public void QuitGame()
    {
        Application.Quit();
    }
}
