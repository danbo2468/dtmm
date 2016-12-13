using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour {

    public InputField nameInputField;

    void Awake()
    {
        GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>().orthographicSize = Screen.height / 2;
        for (int i=0;i < transform.childCount;i++)
        {
            Transform childTransform = transform.GetChild(i);
            for (int k = 0; k < childTransform.childCount; k++)
            {
                Transform childChildTransform = childTransform.GetChild(k);
                childChildTransform.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
                if (childChildTransform.GetComponent<RectTransform>().anchorMax == new Vector2(0.5f,1)
                    && childChildTransform.GetComponent<RectTransform>().anchorMin == new Vector2(0.5f, 1))
                {
                    childChildTransform.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, -Screen.height/5);
                }
                if (childChildTransform.GetComponent<RectTransform>().anchorMax == new Vector2(1, 0)
                    && childChildTransform.GetComponent<RectTransform>().anchorMin == new Vector2(1, 0))
                {
                    childChildTransform.GetComponent<RectTransform>().anchoredPosition = new Vector2(-Screen.width/10, Screen.height / 10);
                }
                if (childChildTransform.GetComponent<RectTransform>().anchorMax == new Vector2(0.5f, 0)
                    && childChildTransform.GetComponent<RectTransform>().anchorMin == new Vector2(0.5f, 0))
                {
                    childChildTransform.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, Screen.height / 10);
                }

            }
        }
    }

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
