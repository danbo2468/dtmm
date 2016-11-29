using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.IO;

public class SaveProfileManager : MonoBehaviour {

    public Text saveProfile1Text;
    public Text saveProfile2Text;
    public Text saveProfile3Text;
    public Text saveProfile4Text;
    private Text[4] saveProfilesText;

    // Use this for initialization
    void Start () {
        saveProfilesText[1] = saveProfile1Text;
        saveProfilesText[2] = saveProfile1Text;
        saveProfilesText[3] = saveProfile1Text;
        saveProfilesText[4] = saveProfile1Text;

        string saveProfileName = GameManager.gameManager.LoadSaveProfileName(1);

        if (saveProfileName != null)
        {
            saveProfile1Text.text = saveProfileName;
        } else
        {
            saveProfile1Text.text = "Empty";
        }
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
