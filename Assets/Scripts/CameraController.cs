using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour {

    // The character, his last position and the difference between those and the camera position
    private PlayerController character;
    private Vector3 lastCharacterPosition;
    private Vector4 lastVerticalCharacterPosition;
    private float distanceToMoveX;
    private float distanceToMoveY;

    // Use this for initialization
    void Start ()
    {
        character = FindObjectOfType<PlayerController>();
        lastCharacterPosition = character.transform.position;
        lastVerticalCharacterPosition = character.transform.position;
    }
	
	// Update is called once per frame
	void Update ()
    {
        if (character.CheckGrounded())
        {
            distanceToMoveY = character.transform.position.y - lastVerticalCharacterPosition.y;
            lastVerticalCharacterPosition = character.transform.position;
        }

        // Calculate the distance to move the camera
        distanceToMoveX = character.transform.position.x - lastCharacterPosition.x;

        // Move the camera
        transform.position = new Vector3(transform.position.x + distanceToMoveX, transform.position.y + distanceToMoveY, transform.position.z);

        // Set the new character position
        lastCharacterPosition = character.transform.position;
	}
}
