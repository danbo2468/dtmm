using UnityEngine;
using System.Collections;

public class WorldController : MonoBehaviour {
    [SerializeField]
    public GameObject player;
    [SerializeField]
    public GameObject path;

    public Path script;

	// Use this for initialization
	void Start () {
        // Ask GameManager for all level completions and highscores.
        script = path.GetComponent<Path>();
        HandleMovement();
    }
	
	// Update is called once per frame
	void Update () {

        
	}

    public void HandleMovement()
    {
        //player.transform.position = Vector2.Lerp()
        script.GetReachableCoreNodes(script.GetAllCoreNodes());
    }

    
   
}
