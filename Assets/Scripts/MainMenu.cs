using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour {

    public InputField nameField;

    public void ChooseGame()
    {
        SceneManager.LoadScene("Choose Saveprofile");
    }

    public void ChooseGender()
    {
        SceneManager.LoadScene("Gender Selection");
    }

    public void ChooseName()
    {
        SceneManager.LoadScene("Name Selection");
    }

    public void SetGender(string gender)
    {
        GameManager.gameManager.setCharacterGender(gender);
    }

    public void LoadLevel()
    {
        GameManager.gameManager.setCharacterName(nameField.text);
        GameManager.gameManager.Save(1);
        SceneManager.LoadScene("Tutorial");
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
