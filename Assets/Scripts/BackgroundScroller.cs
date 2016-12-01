using UnityEngine;
using System.Collections;

public class BackgroundScroller : MonoBehaviour {

    public float scrollSpeed;
    public bool shouldScroll;
    Vector3 startPosition;

    Transform cameraTransform;
    float spriteWidth;
    public float imageWidth;

    // Use this for initialization
    void Start () {
        startPosition = transform.position;
        cameraTransform = Camera.main.transform;
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
        spriteWidth = spriteRenderer.sprite.bounds.size.x;
    }
	
	// Update is called once per frame
	void Update () {
        if (shouldScroll)
        {
            Vector3 newPosition;
            newPosition = transform.position;
            newPosition.x -= Time.deltaTime * scrollSpeed;
            transform.position = newPosition;
        }

        if ((transform.position.x + spriteWidth + 10) < cameraTransform.position.x)
        {
            Vector3 newPos = transform.position;
            newPos.x += imageWidth;
            transform.position = newPos;
        }
    }
}
