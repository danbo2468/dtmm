using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class WorldController : MonoBehaviour
{
    [SerializeField]
    public GameObject player;
    [SerializeField]
    public GameObject path;

    public Transform targetPosition;
    public Transform nextTargetPosition;
    public Transform playerIsAtNode;
    public Transform targetWayPoint;

    public GameObject Popup;


    public Path script;

    public string[] tagArray = { "Path", "LevelUnreachable", "AddMoreForbiddenTagsHere" };
    public bool isMoving;
    public bool isValidMove;

    public int currentWayPoint = 0;

    public bool moveNext;
    public bool movePrevious;

    public bool MoveNext
    {
        get
        {
            return moveNext;
        }

        set
        {
            moveNext = value;
        }
    }

    public bool MovePrevious
    {
        get
        {
            return movePrevious;
        }

        set
        {
            movePrevious = value;
        }
    }

    void Start()
    {
        
        script = path.GetComponent<Path>();
        //playerIsAtNode = script.GetNodeOfScene(1);
        // if the nodes are 0,0,0 it means they aren't set and we should set a default one, preferably the first node of the scene.
        if (GameManager.gameManager.worldNode.x == 0 && GameManager.gameManager.worldNode.y == 0 && GameManager.gameManager.worldNode.z == 0)
        {
            Debug.Log("First time playing this game!");
            playerIsAtNode = script.GetNodeOfScene(1);
            Debug.Log(playerIsAtNode.position);
            SetOverworldPosition(playerIsAtNode.position);
            Debug.Log(GetOverworldPosition());
        }
        // The overworld node is not 0,0,0 meaning there should be a node saved in the player profile.
        else
        {
            // If we are in the overworld -> ask the script for the node that is saved in the player profile.
            // After the node has been found, set the position of the player to that node and 'teleport' the player to it.
            if (IsOverworld())
            {
                Debug.Log("Getting data! player was at this node: " + script.findNodeOnPosition(GetOverworldPosition()));
                playerIsAtNode = script.findNodeOnPosition(GetOverworldPosition());
                playerIsAtNode.position = GetOverworldPosition();
                player.transform.position = playerIsAtNode.position;
            }
            // If we are not in the over world -> we must be in one of the area's.
            else if (!IsOverworld())
            {
                // i don't know what to write here.
                if (playerIsAtNode)
                {
                    playerIsAtNode.position = GetLevelPosition();
                }
                else
                {
                    playerIsAtNode = script.GetNodeOfScene(1);
                }

                // If player is at the first node -> Move to next node;
                if(script.GetLevelID(playerIsAtNode) == 1)
                {
                    moveNext = true;
                }

                // If player is at the last node -> Load overworld;
                if (playerIsAtNode == script.GetLastNode())
                {
                    SceneManager.LoadScene("Overworld");
                }
            }
        }
        // after all evaluation is done, set the nodes to the correct values.
        SetLevels();    
    }

    void Update()
    {  
        HandleButtons();
        Movement();
    }

    /// <summary>
    /// LoadLevel() A function to load the overworld or a level. This may have to be integrated in the level manager.
    /// Right now it is important that the scene name equals the node name.
    /// TODO: Danbo look please.
    /// </summary>
    public void LoadLevel()
    {
        if (IsOverworld())
        {
            SetOverworldPosition(playerIsAtNode.position);
            SceneManager.LoadScene(playerIsAtNode.name);
        }
        else
        {
            SetLevelPosition(playerIsAtNode.position);
            SceneManager.LoadScene(playerIsAtNode.name);
        }
    }

    /// <summary>
    /// Movement() A function to move the player to the desired location. It's a big method that requires refactoring.
    /// Basically it works like this:
    /// Step 1: Check if player wants to move forward or backwards
    /// Step 2: Ask script to calculate the path/route it needs to take.
    /// Step 3: If the current way point does not match the last way point in the route, it means were not done yet and it continues
    /// Step 4: Move the player towards the waypoint
    /// Step 5: At somepoint we've reached our destination and it will reset some values
    /// Step 6: Check if the player is at the overworld, if yes save the location, if not save the level location. 
    /// Step 7: Upon reaching the destination, the popup with levelname + score will show again. Also the buttons will be enabled again.
    /// TODO: Refactor
    /// </summary>
    private void Movement()
    {
        if (moveNext) // step 1
        {
            HideEnterLevelCanvas();
            List<Transform> route = script.GetRoute(playerIsAtNode, true); // step 2
            if (route.Count > 0)
            {
                isMoving = true;
                if (currentWayPoint < route.Count) // step 3
                {
                    if (targetWayPoint == null)
                    {
                        targetWayPoint = route[currentWayPoint];
                    }
                }

                if (player.transform.position == targetWayPoint.position && currentWayPoint != route.Count) // step 4
                {
                    currentWayPoint++;
                    targetWayPoint = route[currentWayPoint];
                }

                player.transform.position = Vector2.MoveTowards(new Vector2(player.transform.position.x, player.transform.position.y), targetWayPoint.position, 14.5f * Time.deltaTime);

                if (player.transform.position == route[route.Count - 1].position) // step 5
                {
                    currentWayPoint = 0;
                    playerIsAtNode = targetWayPoint;
                    moveNext = false;
                    targetWayPoint = null;
                    isMoving = false;
                    ShowEnterLevelCanvas(playerIsAtNode);

                    if (IsOverworld()) // step 6
                    {
                        SetOverworldPosition(playerIsAtNode.position);
                    }
                    if (!IsOverworld())
                    {
                        if (playerIsAtNode.tag == "AreaEnd")
                        {
                            SceneManager.LoadScene("Overworld");
                        }
                        SetLevelPosition(playerIsAtNode.position);                      
                    }
                    GameManager.gameManager.Save();
                }
            }
            if (route.Count == 0) // step 7
            {
                moveNext = false;
                ShowEnterLevelCanvas(playerIsAtNode);
            }
        }

        if (movePrevious)
        {
            HideEnterLevelCanvas();
            List<Transform> route = script.GetRoute(playerIsAtNode, false);
            if (route.Count > 0)
            {
                isMoving = true;
                if (currentWayPoint < route.Count)
                {
                    if (targetWayPoint == null)
                    {
                        targetWayPoint = route[currentWayPoint];
                    }
                }

                if (player.transform.position == targetWayPoint.position && currentWayPoint != route.Count)
                {
                    targetWayPoint = route[currentWayPoint];
                    currentWayPoint++;
                }

                player.transform.position = Vector2.MoveTowards(new Vector2(player.transform.position.x, player.transform.position.y), targetWayPoint.position, 14.5f * Time.deltaTime);

                if (player.transform.position == route[route.Count - 1].position)
                {
                    currentWayPoint = 0;
                    playerIsAtNode = targetWayPoint;
                    movePrevious = false;
                    targetWayPoint = null;
                    isMoving = false;
                    ShowEnterLevelCanvas(playerIsAtNode);

                    if (IsOverworld())
                    {
                        SetOverworldPosition(playerIsAtNode.position);
                    }
                    if (!IsOverworld())
                    {
                        SetLevelPosition(playerIsAtNode.position);
                        if (playerIsAtNode.tag == "AreaBegin")
                        {
                            Debug.Log("LOADING OVERWORLD");
                            SceneManager.LoadScene("Overworld");
                        }
                    }
                    GameManager.gameManager.Save();
                }
            }
            if (route.Count == 0)
            {
                movePrevious = false;
                ShowEnterLevelCanvas(playerIsAtNode);
            }   
        }
    }

    /// <summary>
    /// Button handler, disables/enables buttons based on current state.
    /// </summary>
    private void HandleButtons()
    {
        if (isMoving)
        {
            DisableButtons();
        }

        else if (!isMoving)
        {
            EnableButtons();
        }
    }

    private void SetOverworldPosition(Vector3 position)
    {
        GameManager.gameManager.SetWorldPosition(position);
    }

    private void SetLevelPosition(Vector3 position)
    {
        GameManager.gameManager.SetLevelPosition(position);
    }

    private Vector3 GetOverworldPosition()
    {
        return GameManager.gameManager.worldNode;
    }

    private Vector3 GetLevelPosition()
    {
        return GameManager.gameManager.levelNode;
    }

    private float[] GetHighscores()
    {
        return GameManager.gameManager.levelHighscores;
    }

    /// <summary>
    /// For now I'm hardcoding the levels, I might try and develop some formula for it later.
    /// </summary>
    private void SetLevels()
    {
        float[] levelHighscores = GetHighscores();
        
        if (!IsOverworld())
        {
            if (SceneManager.GetActiveScene().name == "House")
            {
                if (ScoreIsSet(levelHighscores, 1))
                {
                    script.setLevelCompleted(script.GetNodeOfScene(2));
                }
            }

            if (SceneManager.GetActiveScene().name == "Street")
            {
                if (ScoreIsSet(levelHighscores, 2))
                {
                    script.setLevelCompleted(script.GetNodeOfScene(2));
                    script.setLevelUncompleted(script.GetNodeOfScene(3));
                }

                if (ScoreIsSet(levelHighscores, 3))
                {
                    script.setLevelCompleted(script.GetNodeOfScene(3));
                    script.setLevelUncompleted(script.GetNodeOfScene(4));
                }

                if (ScoreIsSet(levelHighscores, 4))
                {
                    script.setLevelCompleted(script.GetNodeOfScene(4));
                }
            }

            if (SceneManager.GetActiveScene().name == "Market")
            {
                if (ScoreIsSet(levelHighscores, 5))
                {
                    script.setLevelCompleted(script.GetNodeOfScene(2));
                    script.setLevelUncompleted(script.GetNodeOfScene(3));
                }

                if (ScoreIsSet(levelHighscores, 6))
                {
                    script.setLevelCompleted(script.GetNodeOfScene(3));
                    script.setLevelUncompleted(script.GetNodeOfScene(4));
                }

                if (ScoreIsSet(levelHighscores, 7))
                {
                    script.setLevelCompleted(script.GetNodeOfScene(4));
                }
            }

            if (SceneManager.GetActiveScene().name == "School")
            {
                if (ScoreIsSet(levelHighscores, 8))
                {
                    script.setLevelCompleted(script.GetNodeOfScene(2));
                    script.setLevelUncompleted(script.GetNodeOfScene(3));
                }

                if (ScoreIsSet(levelHighscores, 9))
                {
                    script.setLevelCompleted(script.GetNodeOfScene(3));
                    script.setLevelUncompleted(script.GetNodeOfScene(4));
                }

                if (ScoreIsSet(levelHighscores, 10))
                {
                    script.setLevelCompleted(script.GetNodeOfScene(4));
                }
            }

            if (SceneManager.GetActiveScene().name == "Jungle")
            {
                if (ScoreIsSet(levelHighscores, 11))
                {
                    script.setLevelCompleted(script.GetNodeOfScene(2));
                    script.setLevelUncompleted(script.GetNodeOfScene(3));
                }

                if (ScoreIsSet(levelHighscores, 12))
                {
                    script.setLevelCompleted(script.GetNodeOfScene(3));
                    script.setLevelUncompleted(script.GetNodeOfScene(4));
                }

                if (ScoreIsSet(levelHighscores, 13))
                {
                    script.setLevelCompleted(script.GetNodeOfScene(4));
                }
            }

        }
        else
        {
            if (ScoreIsSet(levelHighscores, 0))
            {
                script.setLevelCompleted(script.GetNodeOfScene(1));
            }

            if (ScoreIsSet(levelHighscores, 1))
            {
                script.setLevelCompleted(script.GetNodeOfScene(2));
                script.setLevelUncompleted(script.GetNodeOfScene(3));
            }

            if (ScoreIsSet(levelHighscores, new int[] { 2,3,4 }))
            {
                script.setLevelCompleted(script.GetNodeOfScene(3));
                script.setLevelUncompleted(script.GetNodeOfScene(4));
            }

            if (ScoreIsSet(levelHighscores, new int[] { 5,6,7 }))
            {
                script.setLevelCompleted(script.GetNodeOfScene(4));
                script.setLevelUncompleted(script.GetNodeOfScene(5));
            }

            if (ScoreIsSet(levelHighscores, new int[] { 8,9,10 }))
            {
                script.setLevelCompleted(script.GetNodeOfScene(5));
                script.setLevelUncompleted(script.GetNodeOfScene(6));
            }

            if (ScoreIsSet(levelHighscores, new int[] { 11,12,13 }))
            {
                script.setLevelCompleted(script.GetNodeOfScene(6));
                script.setLevelUncompleted(script.GetNodeOfScene(7));
            }

        }
    }
    
    /// <summary>
    /// Checks the array for scores based on the indexes supplied, returns true if ALL of the indexes are set.
    /// </summary>
    /// <param name="array"></param>
    /// <param name="indexes"></param>
    /// <returns>True/False</returns>
    private bool ScoreIsSet(float[] array, int[] indexes)
    {
        bool returnBool = true;
        foreach (int index in indexes)
        {
            returnBool = array[index] > 0 ? true : false;
            if (!returnBool)
                return false;
        }

        return returnBool;
    }

    /// <summary>
    /// Checks the array for scores based on one index supplied, returns true if it's set. 
    /// </summary>
    /// <param name="array"></param>
    /// <param name="index"></param>
    /// <returns>True/False</returns>
    private bool ScoreIsSet(float[] array, int index)
    {
            return array[index] > 0 ? true : false;
    }

    /// <summary>
    /// Checks whether the current scene is the overworld.
    /// </summary>
    /// <returns></returns>
    private bool IsOverworld()
    {
        if (SceneManager.GetActiveScene().name == "Overworld")
            return true;
        return false;
    }

    /// <summary>
    /// Disables the tagged buttons in the LevelCanvas;
    /// </summary>
    private void DisableButtons()
    {
        Button[] button = GetButtonsFromTaggedGameObject("LevelCanvas");
        foreach (Button but in button)
            but.interactable = false;
    }

    /// <summary>
    /// Enables the tagged buttons in the LevelCanvas
    /// </summary>
    private void EnableButtons()
    {
        Button[] button = GetButtonsFromTaggedGameObject("LevelCanvas");
        foreach (Button but in button)
            but.interactable = true;
    }

    /// <summary>
    /// Returns all the buttons in an array based on a given tag.
    /// </summary>
    /// <param name="tag"></param>
    /// <returns></returns>
    private Button[] GetButtonsFromTaggedGameObject(string tag)
    {
        return GameObject.FindGameObjectWithTag(tag).GetComponentsInChildren<Button>();
    }

    /// <summary>
    /// Displays the popup above the player when idling at a level/area node.
    /// </summary>
    /// <param name="node"></param>
    private void ShowEnterLevelCanvas(Transform node)
    {
        int compare = 1;
        if (IsOverworld())
            compare = 0;

        Text[] textFields = Popup.GetComponentsInChildren<Text>();
        float score = Mathf.Round(UnityEngine.Random.Range(100f, 1000f)); // Load score from file or load manager. 

        if (script.GetLevelID(node) != compare && script.GetLevelID(node) != script.GetAllCoreNodes().Count)
        {
            if (IsOverworld())
            {
                textFields[0].text = "World - " + node.name;
                textFields[1].text = "Total Score: " + score;
                textFields[2].text = "Enter";
            }
            else
            {
                textFields[0].text = SceneManager.GetActiveScene().name + " - " + node.name;
                textFields[1].text = "Score: " + score;
                textFields[2].text = "Play";
            }
        Popup.SetActive(true);
        }
    }

    /// <summary>
    /// Hides the popup above the player when moving in the area/overworld;
    /// </summary>
    private void HideEnterLevelCanvas()
    {
        Popup.SetActive(false);
    }
}
    


































/* OLD WORLD CONTROLLER

	// Use this for initialization
	void Start () {
        // Ask GameManager for all level completions, highscores and last player location.
        script = path.GetComponent<Path>();
        isMoving = false;    
        playerIsAtNode = script.GetFirstNodeOfScene();
        playerIsAtCoreNode = script.IsPlayerAtCoreNode(playerIsAtNode);
    }
	
	// Update is called once per frame
	void Update ()
    {
        if (script.IsPlayerAtCoreNode(player.transform))
            playerIsAtNode = player.transform.gameObject.transform;

            //script.IsPlayerAtCoreNode(player.transform)

        if (!isMoving)
        {
            List<Touch> touches = InputHelper.GetTouches();
            if (touches.Count > 0)
                foreach (Touch touch in touches)
                    HandleInput(touch);

            if (isValidMove) // we got a valid move! Let's put that as our next target position!
                if (isMoving && playerIsAtCoreNode) // player is at a core node, so we can safely change direction if needed.
                    targetPosition = nextTargetPosition; // change current target to the new target.
        }
            if (targetPosition && !isMoving) // we got a target position, let's move towards it
            {
                List<Transform> route = script.CalculateTravelingPath(playerIsAtNode, targetPosition);

           // foreach (Transform path in route)
                //Debug.Log(path.name);

                if (currentWayPoint < route.Count)
                {
                    if(targetWayPoint == null)
                        targetWayPoint = route[currentWayPoint];
                    HandleMovement(route);
                } 
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

        if (isMoving && isValidMove)
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

    public void HandleMovement(List<Transform> route)
    {

        player.transform.position = Vector2.MoveTowards(new Vector2(player.transform.position.x, player.transform.position.y), targetWayPoint.position, 14.5f * Time.deltaTime);

        if (player.transform.position == targetWayPoint.position)
        {
            currentWayPoint++;
            targetWayPoint = route[currentWayPoint];
        }

        if (player.transform.position == targetPosition.position)
            isMoving = false;
        //for (int i = 1; i < route.Count; i++)
        // Debug.Log(route[i].name);
        /*
        for (int i = 0; i < route.Count; i++)
        {
           //Debug.Log(route[i].name);
           // Debug.Log("I moved from: " + player.transform.position);
                player.transform.position = Vector2.MoveTowards(new Vector2(player.transform.position.x, player.transform.position.y), route[i].position, 0.5f * Time.deltaTime);
            //Debug.Log("To here: " + player.transform.position);
            Debug.Log("Boolean check!" + isMoving + isValidMove + playerIsAtCoreNode);
           // Vector2.Lerp(player.transform.position, route[i].position, 4f * Time.deltaTime);
        }

        isMoving = false;
        //transform.position = Vector3.MoveTowards(transform.position, targetWayPoint.position, speed * Time.deltaTime);
        //player.transform.position = Vector2.Lerp()

    */
/*
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
*/