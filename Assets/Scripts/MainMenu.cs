using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour {

    public InputField nameInputField;

    public void SwitchToSaveProfileScene()
    {
        SceneManager.LoadScene("Choose Save Profile");
    }

    public void SwitchToGenderSelectionScene()
    {
        SceneManager.LoadScene("Gender Selection");
    }

    public void SwitchToNameSelectionScene()
    {
        SceneManager.LoadScene("Name Selection");
    }

    public void SetGender(string gender)
    {
        GameManager.gameManager.setCharacterGender(gender);
    }

    public void SetName()
    {
        GameManager.gameManager.setCharacterName(nameInputField.text);
        SceneManager.LoadScene("Save As");
    }

    public void SaveData(int saveProfile)
    {
        GameManager.gameManager.Save(saveProfile);
        SceneManager.LoadScene("Tutorial");
    }

    public void LoadGame(int saveProfile)
    {
        GameManager.gameManager.LoadSaveProfile(saveProfile);
        SceneManager.LoadScene("Tutorial");
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
