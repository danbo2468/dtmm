using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class WorldController : MonoBehaviour {
    [SerializeField]
    public GameObject player;
    [SerializeField]
    public GameObject path;

    public Transform targetPosition;
    public Transform nextTargetPosition;
    public Transform playerIsAtNode;

    public Path script;

    public string[] tagArray = { "Path", "LevelUnreachable", "AddMoreForbiddenTagsHere" };
    public bool isMoving;
    public bool isValidMove;
    public bool playerIsAtCoreNode;

    
	// Use this for initialization
	void Start () {
        // Ask GameManager for all level completions, highscores and last player location.
        script = path.GetComponent<Path>();
        HandleMovement();
        isMoving = false;    
        playerIsAtNode = script.GetFirstNodeOfScene();
        playerIsAtCoreNode = script.IsPlayerAtCoreNode(playerIsAtNode);
    }
	
	// Update is called once per frame
	void Update ()
    {
        List<Touch> touches = InputHelper.GetTouches();
        if (touches.Count > 0)
            foreach (Touch touch in touches)
                HandleInput(touch);

        if (isValidMove) // we got a valid move! Let's put that as our next target position!
            if (isMoving && playerIsAtCoreNode) // player is at a core node, so we can safely change direction if needed.
                targetPosition = nextTargetPosition; // change current target to the new target.

        if (targetPosition) // we got a target position, let's move towards it
        {
            Debug.Log('c');
            isMoving = true;
            HandleMovement();
        }

        else
            isMoving = false; // we are no longer moving, thus reached our destination.


        // if player tapped a node and the character is already moving, stop moving when next coreNode is reached.

        // if the player is still moving, don't call any node calculations
    }

    public void HandleInput(Touch touch)
    {
        //Debug.Log('b');
        isValidMove = true;
        Ray ray = Camera.main.ScreenPointToRay(touch.position);
        Vector3 worldPoint = Camera.main.ScreenToWorldPoint(touch.position);
        Vector2 touchPosition = new Vector2(worldPoint.x, worldPoint.y);
        Collider2D hit = Physics2D.OverlapPoint(touchPosition);

        if (hit)
            foreach (string tag in tagArray)
                if (!isValidMove)
                    if (hit.transform.gameObject.tag == tag)
                        isValidMove = false;

        if (isMoving)
        {
            nextTargetPosition = hit.transform.gameObject.transform;
            //Debug.Log(hit.transform.gameObject.name);
        }

        else if(!isMoving)
        {
            targetPosition = hit.transform.gameObject.transform;
        }

        //Debug.Log(nextTargetPosition);
        
    }

    public void HandleMovement()
    {
        List<Transform> route = script.CalculateTravelingPath(playerIsAtNode, targetPosition);
        Debug.Log(route.Count);
        //player.transform.position = Vector2.Lerp()

        
    }

    public void Test()
    {
        List<Transform> coreNodes = script.GetAllCoreNodes();
        List<Transform> reachableNodes = script.GetReachableCoreNodes(coreNodes);
        List<Transform> pathNodes = new List<Transform>();
        foreach (Transform node in reachableNodes)
        {
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

    public void Test2()
    {
        List<Transform> coreNodes = script.GetAllCoreNodes();
        foreach (Transform node in coreNodes)
            script.GetNextCoreNodeOfPath(node);

        foreach (Transform node in coreNodes)
            script.GetPreviousCoreNodeOfPath(node);
    }
}
