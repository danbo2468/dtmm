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
        SetLevels();
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
        else
        {
            if (IsOverworld())
            {
                Debug.Log("Getting data! player was at this node: " + script.findNodeOnPosition(GetOverworldPosition()));
                playerIsAtNode = script.findNodeOnPosition(GetOverworldPosition());
                playerIsAtNode.position = GetOverworldPosition();
                player.transform.position = playerIsAtNode.position;
            }
            else if (!IsOverworld())
            {
                if (playerIsAtNode)
                {
                    playerIsAtNode.position = GetLevelPosition();
                }
                else
                {
                    playerIsAtNode = script.GetNodeOfScene(1);
                }

                if(script.GetLevelID(playerIsAtNode) == 1)
                {
                    moveNext = true;
                }

                if (playerIsAtNode == script.GetLastNode())
                {
                    SceneManager.LoadScene("Overworld");
                }
            }
        }
        // Ask GameManager for all level completions, highscores and last player location.     
    }

    void Update()
    {
        if(playerIsAtNode)
            Debug.Log(GetOverworldPosition() + " << GM || WM >> " + playerIsAtNode.position);      
        HandleButtons();
        Movement();
    }

    public void LoadLevel()
    {
        if (IsOverworld())
        {
            Debug.Log("We have set the gamemanager's worldposition from " + GetOverworldPosition() + " to this: " + playerIsAtNode.position);
            SetOverworldPosition(playerIsAtNode.position);
            Debug.Log("This is the worldnode position now: " + GetOverworldPosition());
            Debug.Log("LOADING LEVEL::: " + playerIsAtNode.name);
            SceneManager.LoadScene(playerIsAtNode.name);
        }
        else
        {
            
        }
    }

    

    private void Movement()
    {

        if (moveNext)
        {
            HideEnterLevelCanvas();
            List<Transform> route = script.GetRoute(playerIsAtNode, true);
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
                    currentWayPoint++;
                    targetWayPoint = route[currentWayPoint];
                }

                player.transform.position = Vector2.MoveTowards(new Vector2(player.transform.position.x, player.transform.position.y), targetWayPoint.position, 14.5f * Time.deltaTime);

                if (player.transform.position == route[route.Count - 1].position)
                {
                    currentWayPoint = 0;
                    playerIsAtNode = targetWayPoint;
                    moveNext = false;
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
                        if (playerIsAtNode.tag == "AreaEnd")
                        {
                            SceneManager.LoadScene("Overworld");
                        }
                    }
                }
            }
            if (route.Count == 0)
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
                    
                }
            }
            if (route.Count == 0)
            {
                movePrevious = false;
                ShowEnterLevelCanvas(playerIsAtNode);
            }   
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

    private bool ScoreIsSet(float[] array, int index)
    {
            return array[index] > 0 ? true : false;
    }

    private bool IsOverworld()
    {
        if (SceneManager.GetActiveScene().name == "Overworld")
            return true;
        return false;
    }

    private void DisableButtons()
    {
        Button[] button = GetButtonsFromTaggedGameObject("LevelCanvas");
        foreach (Button but in button)
            but.interactable = false;
    }

    private void EnableButtons()
    {
        Button[] button = GetButtonsFromTaggedGameObject("LevelCanvas");
        foreach (Button but in button)
            but.interactable = true;
    }

    private Button[] GetButtonsFromTaggedGameObject(string tag)
    {
        return GameObject.FindGameObjectWithTag(tag).GetComponentsInChildren<Button>();
    }

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
                textFields[0].text = node.name + " - level " + script.GetLevelID(node);
                textFields[1].text = "Score: " + score;
                textFields[2].text = "Play";
            }
        Popup.SetActive(true);
        }
    }

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