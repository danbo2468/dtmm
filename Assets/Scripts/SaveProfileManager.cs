using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using System.IO;

public class SaveProfileManager : MonoBehaviour {

    // Store all the buttons and make a distinction between 'Save' and 'Load'
    public bool emptyProfileClickable;
    public List<Button> saveProfileButtons = new List<Button>();
    public List<Button> saveProfileDeleteButtons = new List<Button>();

    // Use this for initialization
    void Start () {
        int i = 1;

        // Loop through all save profiles
        foreach(Button profileButton in saveProfileButtons)
        {

            // Check if there exists a profile for this button
            string saveProfileName = GameManager.gameManager.LoadSaveProfileName(i);

            // If there is a name, show that name, if there isn't a name, show 'empty'
            if (saveProfileName != null)
            {
                profileButton.GetComponentInChildren<Text>().text = saveProfileName;
            }
            else
            {
                profileButton.GetComponentInChildren<Text>().text = "Empty";

                // Make the button's non-interactable
                if (!emptyProfileClickable)
                {
                    profileButton.interactable = false;
                    saveProfileDeleteButtons[i-1].interactable = false;
                }
            }
            i++;
        }
	}
	
	public void Refresh () {
        int i = 1;

        // Loop through all save profiles
        foreach (Button profileButton in saveProfileButtons)
        {

            // Check if there exists a profile for this button
            string saveProfileName = GameManager.gameManager.LoadSaveProfileName(i);

            // If there is a name, show that name, if there isn't a name, show 'empty'
            if (saveProfileName != null)
            {
                profileButton.GetComponentInChildren<Text>().text = saveProfileName;
            }
            else
            {
                profileButton.GetComponentInChildren<Text>().text = "Empty";

                // Make the button's non-interactable
                if (!emptyProfileClickable)
                {
                    profileButton.interactable = false;
                    saveProfileDeleteButtons[i - 1].interactable = false;
                }
            }
            i++;
        }
    }
}
