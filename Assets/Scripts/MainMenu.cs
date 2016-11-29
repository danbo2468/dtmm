using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour {

    public void ChooseGame()
    {
        SceneManager.LoadScene("Choose Game");
    }

    public void ChooseGender()
    {
        SceneManager.LoadScene("Gender Selection");
    }

    public void ChooseName()
    {
        SceneManager.LoadScene("Name Selection");
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
