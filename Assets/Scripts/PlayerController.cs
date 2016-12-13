using UnityEngine;

public class PlayerController : MonoBehaviour
{

    // Player health
    public float initialHealth;
    private float health;

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

    // Use this for initialization
    void Start()
    {
        // Get the components
        moveSpeed = initialMoveSpeed;
        rigidBody = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        health = initialHealth;
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
        if (!tutorialMode) {

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
            /*
            // Check if spacebar or left mouse button is pressed
            if (Input.GetMouseButtonDown(0) && grounded)
            {
                Jump();
            }
            
            // Check if the right mous button is pressed
            if (Input.GetMouseButtonDown(0))
            {
                Shoot();
            }
            */

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
        health -= damage;
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
