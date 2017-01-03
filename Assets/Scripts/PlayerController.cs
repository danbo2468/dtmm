using UnityEngine;
using System.Collections.Generic;
using System;

public class PlayerController : MonoBehaviour
{
    // Character forces
    public float initialMoveSpeed;
    private float moveSpeed;
    public float jumpForce;

    // Grounded
    private bool grounded;
    public LayerMask ground;
    public Transform groundChecker;
    public float groundCheckerRadius;
    public float currentLayer;

    // Character components
    private Rigidbody2D rigidBody;
    private Animator animator;
    private bool throwing;

    // Tutorial
    private bool tutorialMode;

    // Weapons
    public BulletController weapon;

    // Player health
    public float initialHealth;
    public float health;
    public GameObject healthCanvas;
    public List<Transform> hearts;

    // Character gender
    public RuntimeAnimatorController boyAnimation;
    public RuntimeAnimatorController girlAnimation;


    // Use this for initialization
    void Start()
    {
        Transform[] heartsTemp = healthCanvas.GetComponentsInChildren<Transform>();
        hearts = new List<Transform>();

        foreach (Transform heart in heartsTemp)
        {
            if (heart.tag != "HealthCanvas")
            {
                hearts.Add(heart);
            }
        }

        initialHealth = GameManager.gameManager.getHeartCount() * 2;
        SetupGUI();
        moveSpeed = initialMoveSpeed;
        rigidBody = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        health = initialHealth;

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

    // Update is called once per frame
    void Update()
    {

        // Check the player health
        if (health <= 0)
        {
            GameManager.gameManager.levelManager.GameOver();
        }

        //Check if the tutorialmode is on
        if (!tutorialMode && !GameManager.gameManager.levelManager.isGameOver && !GameManager.gameManager.levelManager.isFinished)
        {

            // Check if grounded
            CheckGrounded();
            if (Input.GetMouseButtonDown(0))
            {
                Vector2 mousePosition = new Vector2(Input.mousePosition.x, Screen.height - Input.mousePosition.y);
                if (mousePosition.x < Screen.width / 2)
                {
                    if (grounded)
                        Jump();
                }
                else if (mousePosition.x > Screen.width / 2)
                    Shoot();
            }
        }

        // Move forward
        rigidBody.velocity = new Vector2(moveSpeed, rigidBody.velocity.y);

        // Update the animation of the character
        UpdateAnimation();
    }

    // Check if grounded
    public bool CheckGrounded()
    {
        grounded = Physics2D.OverlapCircle(groundChecker.position, groundCheckerRadius, ground);
        Collider2D[] platforms = Physics2D.OverlapCircleAll(groundChecker.position, groundCheckerRadius, ground);
        for(int i = 0; i < platforms.Length; i++)
        {
            Platform platform = platforms[i].gameObject.GetComponent<Platform>();
            if (platform != null) {
                currentLayer = platform.layer;
            }
        }
        return grounded;
    }

    // Let the character jump
    public void Jump()
    {
        rigidBody.velocity = new Vector2(rigidBody.velocity.x, jumpForce);
    }

    // Let the character shoot
    public void Shoot()
    {
        animator.SetTrigger("Throwing");
        BulletController bullet = Instantiate(weapon);
        bullet.transform.position = new Vector2(transform.position.x + 0.8f, transform.position.y + 1.5f);
    }

    // Apply damage to the character
    void ApplyDamage(float damage)
    {
        Debug.Log("Player health before damage taken: " + health);
        health -= damage;
        Debug.Log("Player health after damage taken: " + health);
        UpdateGUI();
    }

    /// <summary>
    /// SetupGUI()
    /// Sets up the heart GUI based on the maximum heart count of the player. Every heart counts as 2 health.
    /// </summary>
    private void SetupGUI()
    {
        // Asks the gamemanager for the heartCount of the player.
        int heartCount = GameManager.gameManager.getHeartCount();
        Debug.Log(heartCount);

        if (heartCount == 0)
        {
            disableHeart(1);
            disableHeart(2);
            disableHeart(3);
            disableHeart(4);
            disableHeart(5);
        }
        else if (heartCount == 1)
        {
            disableHeart(2);
            disableHeart(3);
            disableHeart(4);
            disableHeart(5);
        }
        else if (heartCount == 2)
        {
            disableHeart(3);
            disableHeart(4);
            disableHeart(5);
        }
        else if (heartCount == 3)
        {
            disableHeart(4);
            disableHeart(5);
        }
        else if (heartCount == 4)
        {
            disableHeart(5);
        }
    }

    private void UpdateGUI()
    {
        if (health == 0)
        {
            setEmptyHeart(1);
            setEmptyHeart(2);
            setEmptyHeart(3);
            setEmptyHeart(4);
            setEmptyHeart(5);
        }

        else if (health == 1)
        {
            setHalfHeart(1);
            setEmptyHeart(2);
            setEmptyHeart(3);
            setEmptyHeart(4);
            setEmptyHeart(5);
        }
        else if (health == 2)
        {
            setFullHeart(1);
            setEmptyHeart(2);
            setEmptyHeart(3);
            setEmptyHeart(4);
            setEmptyHeart(5);
        }
        else if (health == 3)
        {
            setFullHeart(1);
            setHalfHeart(2);
            setEmptyHeart(3);
            setEmptyHeart(4);
            setEmptyHeart(5);
        }
        else if (health == 4)
        {
            setFullHeart(1);
            setFullHeart(2);
            setEmptyHeart(3);
            setEmptyHeart(4);
            setEmptyHeart(5);
        }
        else if (health == 5)
        {
            setFullHeart(1);
            setFullHeart(2);
            setHalfHeart(3);
            setEmptyHeart(4);
            setEmptyHeart(5);
        }
        else if (health == 6)
        {
            setFullHeart(1);
            setFullHeart(2);
            setFullHeart(3);
            setEmptyHeart(4);
            setEmptyHeart(5);
        }
        else if (health == 7)
        {
            setFullHeart(1);
            setFullHeart(2);
            setFullHeart(3);
            setHalfHeart(4);
            setEmptyHeart(5);
        }
        else if (health == 8)
        {
            setFullHeart(1);
            setFullHeart(2);
            setFullHeart(3);
            setFullHeart(4);
            setEmptyHeart(5);
        }
        else if (health == 9)
        {
            setFullHeart(1);
            setFullHeart(2);
            setFullHeart(3);
            setFullHeart(4);
            setHalfHeart(4);
        }
        else if (health == 10)
        {
            setFullHeart(1);
            setFullHeart(2);
            setFullHeart(3);
            setFullHeart(4);
            setFullHeart(5);
        }
    }

    // Update the animation of the character
    void UpdateAnimation()
    {
        animator.SetFloat("Speed", rigidBody.velocity.x);
        animator.SetBool("Grounded", grounded);
    }

    // Turn on tutorial mode
    public void SetTutorialMode(bool value)
    {
        tutorialMode = value;
    }

    // Set the move speed of the character
    public void SetMoveSpeed(float speed = -1)
    {
        if (speed == -1)
        {
            moveSpeed = initialMoveSpeed;
        }
        else
        {
            moveSpeed = speed;
        }


        if (speed == 0)
        {
            animator.enabled = false;
        }
        else
        {
            animator.enabled = true;
        }
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag == "Water")
        {
            GameManager.gameManager.levelManager.GameOver();
        }
    }

    void setHalfHeart(int number)
    {
        Debug.Log("setHalfHeart(" + number + ")");
        hearts[number - 1].GetComponent<SpriteRenderer>().sprite = Resources.Load("Half_heart", typeof(Sprite)) as Sprite;
    }

    void setEmptyHeart(int number)
    {
        Debug.Log("setEmptyHeart(" + number + ")");
        hearts[number - 1].GetComponent<SpriteRenderer>().sprite = Resources.Load("Empty_heart", typeof(Sprite)) as Sprite;
    }

    void setFullHeart(int number)
    {
        Debug.Log("setFullHeart(" + number + ")");
        hearts[number - 1].GetComponent<SpriteRenderer>().sprite = Resources.Load("Full_heart", typeof(Sprite)) as Sprite;
    }

    void disableHeart(int number)
    {
        Debug.Log("disableHeart(" + number + ")");
        hearts[number - 1].GetComponent<SpriteRenderer>().enabled = false;
    }
}
