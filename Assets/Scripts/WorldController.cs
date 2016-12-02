using UnityEngine;
using System.Collections;
using System.Collections.Generic;

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
        
        List<Transform> coreNodes = script.GetAllCoreNodes();
        List<Transform> reachableNodes = script.GetReachableCoreNodes(coreNodes);
        List<Transform> pathNodes = new List<Transform>();
        Debug.Log("Expected numbers are: 0 | 0 | 8 ||| 7 8 10 ");
        foreach (Transform node in reachableNodes)
        {
            script.GetPathNodesFromCoreNode(node);
            Debug.Log(script.GetPathNodesFromCoreNode(node).Count);
        }
    }

    
   
}
