using UnityEngine;
using System.Collections;

public class BackgroundScroller : MonoBehaviour {

    public float scrollSpeed;
    Vector3 startPosition;
    Transform cameraTransform;
    public PlayerController player;
    float tileWidth;

    // Use this for initialization
    void Start () {
        player = FindObjectOfType<PlayerController>();
        startPosition = transform.position;
        cameraTransform = Camera.main.transform;
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
        tileWidth = GetComponent<SpriteRenderer>().bounds.size.x;
    }
	
	// Update is called once per frame
	void Update () {
        Vector3 newPosition = transform.position;
        newPosition.x -= Time.deltaTime * scrollSpeed * player.GetComponent<Rigidbody2D>().velocity.x;
        transform.position = newPosition;

        if ((transform.position.x + 35.5) < cameraTransform.position.x)
        {
            newPosition = transform.position;
            newPosition.x += (2 * tileWidth) - 0.1f;
            transform.position = newPosition;
        }
    }
}
