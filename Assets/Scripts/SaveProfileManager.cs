using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.IO;

public class SaveProfileManager : MonoBehaviour {

    public Text saveProfile1Text;
    public Text saveProfile2Text;
    public Text saveProfile3Text;
    public Text saveProfile4Text;

    // Use this for initialization
    void Start () {
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
