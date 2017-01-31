using UnityEngine;

public class BackgroundScroller : MonoBehaviour {

    // Player
    public PlayerController player;

    // Scrolling
    float imageWidth;
    public float scrollSpeed;

    // Position
    Vector3 startPosition;
    Transform cameraTransform;

    // Use this for initialization
    void Start () {
        player = FindObjectOfType<PlayerController>();
        startPosition = transform.position;
        cameraTransform = Camera.main.transform;
        imageWidth = GetComponent<SpriteRenderer>().bounds.size.x;
    }
	
	// Update is called once per frame
	void Update () {

        // Move the image at a certain speed
        Vector3 newPosition = transform.position;
        if (player.GetComponent<Rigidbody2D>().velocity.x > 0)
        {
            newPosition.x -= Time.deltaTime * scrollSpeed * player.initialMoveSpeed;
        }
        else
        {
            newPosition.x -= Time.deltaTime * scrollSpeed * player.GetComponent<Rigidbody2D>().velocity.x;
        }
        transform.position = newPosition;

        // Check if the image is out of the camera's view
        if (transform.position.x + (imageWidth) < cameraTransform.position.x)
        {
            // Set the new position of the image
            newPosition = transform.position;
            newPosition.x += (2 * imageWidth) - 0.1f;
            transform.position = newPosition;
        }
    }
}
