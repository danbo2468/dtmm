using UnityEngine;
using System.Collections;

public class WorldPlayerController : MonoBehaviour {

    private Vector3 previousLocation;
    private Animator animator;

    // Character movement
    private bool playerMoving;
    private float moveX;
    private float moveY;

    // Character gender
    public RuntimeAnimatorController boyAnimation;
    public RuntimeAnimatorController girlAnimation;

    // Use this for initialization
    void Start () {
        animator = GetComponent<Animator>();

        // Set the character animation depending on the chosen gender
        if (GameManager.gameManager.characterGender == "Female")
        {
            animator.runtimeAnimatorController = girlAnimation;
        }
        else
        {
            animator.runtimeAnimatorController = boyAnimation;
        }
    }

    void Update()
    {
        Vector3 curVel = (transform.position - previousLocation) / Time.deltaTime;
        moveX = curVel.x / 14.5f;
        moveY = curVel.y / 14.5f;
        if (transform.position == previousLocation)
        {
            playerMoving = false;
        }
        else
        {
            playerMoving = true;
        }
        animator.SetFloat("MoveY", moveY);
        animator.SetFloat("MoveX", moveX);
        animator.SetBool("PlayerMoving", playerMoving);
        previousLocation = transform.position;
    }
}
