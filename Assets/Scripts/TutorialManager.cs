using UnityEngine;
using System.Collections;

public class TutorialManager : MonoBehaviour {

    // Tutorial Points
    private GameObject jumpPoint;
    private GameObject collectPoint;
    private GameObject groundAttackPoint;
    private GameObject airAttackPoint;

    // Tutorial Points Instructions
    private GameObject jumpText;
    private GameObject collectText;
    private GameObject groundAttackText;
    private GameObject airAttackText;

    // Player
    private PlayerController player;

    // Other
    private string lastPoint;

    // Use this for initialization
    void Start () {
        jumpPoint = GameObject.Find("JumpPoint");
        collectPoint = GameObject.Find("CollectPoint");
        groundAttackPoint = GameObject.Find("GroundAttackPoint");
        airAttackPoint = GameObject.Find("AirAttackPoint");

        jumpText = GameObject.Find("JumpText");
        collectText = GameObject.Find("CollectText");
        groundAttackText = GameObject.Find("GroundAttackText");
        airAttackText = GameObject.Find("AirAttackText");

        jumpText.SetActive(false);
        collectText.SetActive(false);
        groundAttackText.SetActive(false);
        airAttackText.SetActive(false);

        player = FindObjectOfType<PlayerController>();
        player.SetTutorialMode(true);
    }
	
	// Update is called once per frame
	void Update () {

        // Check if grounded
        player.CheckGrounded();

        // After air attack point
        if (lastPoint == "AirAttackPoint")
        {
            if (Input.GetMouseButtonDown(0) && player.CheckGrounded())
            {
                player.Jump();
            }
            if (Input.GetMouseButtonDown(1))
            {
                player.Shoot();
            }
        }

        // At air attack point
        else if (player.transform.position.x > airAttackPoint.transform.position.x && lastPoint == "GroundAttackPoint")
        {
            player.SetMoveSpeed(0);
            airAttackText.SetActive(true);
            if (Input.GetMouseButtonDown(0))
            {
                player.Jump();
                ContinueFromPoint("AirAttackPoint");
            }
        }

        // Between ground attack point and air attack point
        else if (player.transform.position.x > groundAttackPoint.transform.position.x && lastPoint == "GroundAttackPoint")
        {
            if (Input.GetMouseButtonDown(0) && player.CheckGrounded())
            {
                player.Jump();
            }
            if (Input.GetMouseButtonDown(1))
            {
                player.Shoot();
            }
        }

        // At ground attack point
        else if (player.transform.position.x > groundAttackPoint.transform.position.x && lastPoint == "CollectPoint")
        {
            player.SetMoveSpeed(0);
            groundAttackText.SetActive(true);
            if (Input.GetMouseButtonDown(1))
            {
                player.Shoot();
                ContinueFromPoint("GroundAttackPoint");
            }
        }

        // Between collect point and ground attack point
        else if (player.transform.position.x > collectPoint.transform.position.x && lastPoint == "CollectPoint")
        {
            if (Input.GetMouseButtonDown(0) && player.CheckGrounded())
            {
                player.Jump();
            }
        }

        // At collect point
        else if (player.transform.position.x > collectPoint.transform.position.x && lastPoint == "JumpPoint")
        {
            player.SetMoveSpeed(0);
            collectText.SetActive(true);
            if (Input.GetMouseButtonDown(0))
            {
                ContinueFromPoint("CollectPoint");
            }
        }

        // Between jump point and collect point
        else if (player.transform.position.x > jumpPoint.transform.position.x && lastPoint == "JumpPoint")
        {
            if (Input.GetMouseButtonDown(0) && player.CheckGrounded())
            {
                player.Jump();
            }
        }

        // At jump point
        else if (player.transform.position.x > jumpPoint.transform.position.x && lastPoint == null)
        {
            player.SetMoveSpeed(0);
            jumpText.SetActive(true);
            if (Input.GetMouseButtonDown(0))
            {
                player.Jump();
                ContinueFromPoint("JumpPoint");
            }
        }
    }

    // Continue from a certain point
    void ContinueFromPoint(string continueFrom)
    {
        player.SetMoveSpeed();
        lastPoint = continueFrom;
        if(continueFrom == "JumpPoint")
        {
            jumpText.SetActive(false);
        }
        else if (continueFrom == "CollectPoint")
        {
            collectText.SetActive(false);
        }
        else if(continueFrom == "GroundAttackPoint")
        {
            groundAttackText.SetActive(false);
        }
        else if (continueFrom == "AirAttackPoint")
        {
            airAttackText.SetActive(false);
        }
    }
}
