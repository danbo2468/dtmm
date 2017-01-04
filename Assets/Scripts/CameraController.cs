using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour {

    // The character
    private PlayerController character;

    // Layer information
    private float previousLayer;
    private float currentLayer;
    private Vector3 bottomCameraPosition;

    // X transition
    private Vector3 lastHorizontalCharacterPosition;
    private float distanceToMoveX;

    // Y transition
    private Vector3 lastVerticalCharacterPosition;
    private float distanceToMoveY;
    private float ySteps;
    private float timesToChange;
    public float smoothness;

    // Use this for initialization
    void Start ()
    {
        character = FindObjectOfType<PlayerController>();
        lastHorizontalCharacterPosition = character.transform.position;
        lastVerticalCharacterPosition = character.transform.position;

        // Set the layer information
        previousLayer = character.currentLayer;
        currentLayer = character.currentLayer;
        bottomCameraPosition = transform.position;
    }
	
	// Update is called once per frame
	void Update ()
    {
        Vector3 newPosition;

        // Check if the character has entered another layer
        if(currentLayer != character.currentLayer)
        {
            previousLayer = currentLayer;
            currentLayer = character.currentLayer;

            // The character is running in the bottom layer
            if (currentLayer <= 1.5)
            {
                distanceToMoveY = bottomCameraPosition.y - transform.position.y;
            }

            //  The character is running in a higher layer
            if(currentLayer > 1.5)
            {
                distanceToMoveY = character.transform.position.y - lastVerticalCharacterPosition.y;
            }

            // Set variables for smooth animation
            ySteps = distanceToMoveY / smoothness;
            timesToChange = smoothness;

            // Set the new vertical character position
            lastVerticalCharacterPosition = character.transform.position;
        }

        // Smoothly change the y position of the camera
        if (timesToChange > 0)
        {
            transform.position = new Vector3(transform.position.x, transform.position.y + ySteps, transform.position.z);
            timesToChange--;
        }

        // Calculate the distance to move the camera
        distanceToMoveX = character.transform.position.x - lastHorizontalCharacterPosition.x;
        newPosition = new Vector3(transform.position.x + distanceToMoveX, transform.position.y, transform.position.z);
        transform.position = newPosition;

        // Set the new horizontal character position
        lastHorizontalCharacterPosition = character.transform.position;
	}
}
