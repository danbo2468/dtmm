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

    public void SaveData()
    {
        GameManager.gameManager.setCharacterName(nameInputField.text);
        GameManager.gameManager.Save(1);
    }

    public void LoadLevel()
    {
        SceneManager.LoadScene("Tutorial");
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
