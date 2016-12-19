using UnityEngine;

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

    // Character components
    private Rigidbody2D rigidBody;
    private Animator animator;

    // Tutorial
    private bool tutorialMode;

    // Weapons
    public BulletController weapon;

    // Healthbar
    public Texture2D bgImage;
    public Texture2D fgImage;
    public float healthBarLength;

    // Player health
    public float initialHealth;
    private float health;

    // Character gender
    public RuntimeAnimatorController boyAnimation;
    public RuntimeAnimatorController girlAnimation;

    // Use this for initialization
    void Start()
    {
        // Get the components
        bgImage = new Texture2D(1, 1);
        healthBarLength = Screen.width / 3;
        moveSpeed = initialMoveSpeed;
        rigidBody = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        health = initialHealth;

        if(GameManager.gameManager.characterGender == "Female")
        {
            animator.runtimeAnimatorController = girlAnimation;
        } else
        {
            animator.runtimeAnimatorController = boyAnimation;
        }
    }

    // Update is called once per frame
    void Update()
    {   
        // Check the player health
        if(health <= 0)
        {
            GameManager.gameManager.levelManager.GameOver();
        }

        //Check if the tutorialmode is on
        if (!tutorialMode && !GameManager.gameManager.levelManager.isGameOver && !GameManager.gameManager.levelManager.isFinished) {

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

    void OnGUI()
    {
        /*
        // Create one Group to contain both images
        // Adjust the first 2 coordinates to place it somewhere else on-screen
        GUI.BeginGroup(new Rect(Screen.width/3, 5, healthBarLength, 32));

        // Draw the background image
        GUI.Box(new Rect(0, 0, healthBarLength, 32), bgImage);
        //GUI.DrawTexture(new Rect(0, 0, healthBarLength, 32), bgImage, ScaleMode.ScaleToFit, true, 10.0f);
        // Create a second Group which will be clipped
        // We want to clip the image and not scale it, which is why we need the second Group
        GUI.BeginGroup(new Rect(0, 0, health / initialHealth * healthBarLength, 32));

        // Draw the foreground image
        GUI.Box(new Rect(0, 0, healthBarLength, 32), fgImage);
        //GUI.DrawTexture(new Rect(0, 0, healthBarLength, 32), fgImage, ScaleMode.ScaleToFit, true, 10.0f);
        // End both Groups
        GUI.EndGroup();

        GUI.EndGroup();
        */
    }

    // Check if grounded
    public bool CheckGrounded()
    {
        grounded = Physics2D.OverlapCircle(groundChecker.position, groundCheckerRadius, ground);
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
        BulletController bullet = Instantiate(weapon);
        bullet.transform.position = new Vector2(transform.position.x + 0.3f, transform.position.y + 1.5f);
    }

    // Apply damage to the character
    void ApplyDamage(float damage)
    {
        Debug.Log("Player health before damage taken: " + health);
        health -= damage;
        Debug.Log("Player health after damage taken: " + health);
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
        } else
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
}
