using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour {

    // The character, his last position and the difference between those and the camera position
    private PlayerController character;
    private Vector3 lastVerticalCharacterPosition;
    private Vector3 lastHorizontalCharacterPosition;
    private float distanceToMoveX;

    // Y change
    private float distanceToMoveY;
    private float ySteps;
    private float timesToChange;

    // Use this for initialization
    void Start ()
    {
        timesToChange = 0;
        character = FindObjectOfType<PlayerController>();
        lastHorizontalCharacterPosition = character.transform.position;
        lastVerticalCharacterPosition = character.transform.position;
    }
	
	// Update is called once per frame
	void Update ()
    {
        // Check if user enters another layer
        if (character.CheckGrounded() && timesToChange == 0)
        {
            distanceToMoveY = (character.transform.position.y - lastVerticalCharacterPosition.y)/3;
            ySteps = distanceToMoveY / 30;
            timesToChange = 30;
            lastVerticalCharacterPosition = character.transform.position;
        }

        // Change Y
        if(timesToChange > 0)
        {
            transform.position = new Vector3(transform.position.x, transform.position.y + ySteps, transform.position.z);
            timesToChange--;
        }

        // Calculate the distance to move the camera
        distanceToMoveX = character.transform.position.x - lastHorizontalCharacterPosition.x;

        // Move the camera
        Vector3 newPosition = new Vector3(transform.position.x + distanceToMoveX, transform.position.y, transform.position.z);
        transform.position = newPosition;

        // Set the new character position
        lastHorizontalCharacterPosition = character.transform.position;
	}
}
