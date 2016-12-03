using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class WorldController : MonoBehaviour {
    [SerializeField]
    public GameObject player;
    [SerializeField]
    public GameObject path;

    public Path script;
    public bool isMoving;
    public Transform targetPosition;
    public Transform playerIsAtNode;

	// Use this for initialization
	void Start () {
        // Ask GameManager for all level completions, highscores and last player location.
        script = path.GetComponent<Path>();
        HandleMovement();
        isMoving = false;
        playerIsAtNode = script.GetFirstNodeOfScene();
    }
	
	// Update is called once per frame
	void Update ()
    {
        List<Touch> touches = InputHelper.GetTouches();
        if (touches.Count > 0)
            foreach (Touch touch in touches)
            {
                HandleInput(touch);
            }
        //HandleTouchEvents(Input.GetTouch(0));
        // Listen for input

        // if player tapped a node and the character is already moving, stop moving when next node is reached.

        // if the player is still moving, don't call any node calculations
    }

    public void HandleInput(Touch touch)
    {
        Debug.Log(touch.position);
        Ray ray = Camera.main.ScreenPointToRay(touch.position);


        Vector3 worldPoint = Camera.main.ScreenToWorldPoint(touch.position);
        Vector2 touchPosition = new Vector2(worldPoint.x, worldPoint.y);
        Collider2D hit = Physics2D.OverlapPoint(touchPosition);
        Debug.Log(touch.position);
        if (hit)
            Debug.Log(hit.transform.gameObject.name);
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
            ;
            foreach (Transform node1 in script.GetPathNodesFromCoreNode(node))
            {
                script.GetParentNode(node1);
            }
            Debug.Log(script.GetPathNodesFromCoreNode(node).Count);
        }

        Transform lastUnreachableNode = script.GetNextUnreachableNode();
        Debug.Log(lastUnreachableNode);

        Debug.Log(script.GetFirstNodeOfScene());
    }

    
   
}
