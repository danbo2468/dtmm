using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using System.IO;

public class SaveProfileManager : MonoBehaviour {

    public bool emptyProfileClickable;
    public List<Button> saveProfileButtons = new List<Button>();

    // Use this for initialization
    void Start () {
        int i = 1;
        foreach(Button profileButton in saveProfileButtons)
        {
            string saveProfileName = GameManager.gameManager.LoadSaveProfileName(i);

            if (saveProfileName != null)
            {
                profileButton.GetComponentInChildren<Text>().text = saveProfileName;
            }
            else
            {
                profileButton.GetComponentInChildren<Text>().text = "Empty";
                if (!emptyProfileClickable)
                {
                    profileButton.interactable = false;
                }
            }
            i++;
        }
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
