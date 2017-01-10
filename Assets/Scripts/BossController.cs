using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BossController : MonoBehaviour {
    // Use this for initialization
    public bool settingUp;
    public bool isBusy;

    public bool attack1 = false;
    public bool attack2 = false;

    public bool block = false;

    public bool charge = false;

    public GameObject bossObject;
    public GameObject player;
    public GameObject bossIdleLocation;
    public GameObject chargeLocation;
    public GameObject spawnLocation;
    public GameObject infrontOfPlayer;
    public GameObject blockLocation;

    private List<BoxCollider2D> colliders;

    private Rigidbody2D rigidBody;
    

    private string boss;

    void Start () {
        this.enabled = false;
        rigidBody = bossObject.GetComponent<Rigidbody2D>();
        colliders = new List<BoxCollider2D>();
        foreach (BoxCollider2D x in GetComponentsInChildren<BoxCollider2D>())
        {
            colliders.Add(x);
        }
        Setup("Samurai");
	}
	
	// Update is called once per frame
	void Update ()
    {
        if (settingUp)
        {
            bossObject.GetComponent<EnemyController>().speed = player.GetComponent<PlayerController>().moveSpeed;
            rigidBody.velocity = new Vector2(bossObject.GetComponent<EnemyController>().speed, rigidBody.velocity.y);
            settingUp = false;
        }
        else 
        {
            if (attack1)
            {
                isBusy = true;
                Move(bossObject, infrontOfPlayer, 5f);
                if(bossObject.transform.position == infrontOfPlayer.transform.position)
                {
                    // todo: play animation for attack and eventually set isBusy and attack1 to false.
                    isBusy = false;
                    Reset();
                }
            }
            else if (attack2)
            {
                isBusy = true;
                Move(bossObject, infrontOfPlayer, 5f);
                if (bossObject.transform.position == infrontOfPlayer.transform.position)
                {
                    // todo: play animation for attack and eventually set isBusy and attack1 to false.
                    isBusy = false;
                    Reset();
                }
            }
            else if (block)
            {
                Move(bossObject, blockLocation, 10f);
                // Enable first collider.
                colliders[0].enabled = true;
                // start block animation
                // blocks next incoming attack
            }
            else if (charge)
            {
                Move(bossObject, chargeLocation, 25f);
            }
            if (!isBusy)
            {
                Move(bossObject, bossIdleLocation, 10f);
            }
        }
    }

    public void Move(GameObject obj, GameObject target, float speed)
    {
        obj.transform.position = Vector2.MoveTowards(new Vector2(obj.transform.position.x, obj.transform.position.y), new Vector2(target.transform.position.x, target.transform.position.y), speed * Time.deltaTime);
    }

    void Setup(string boss)
    {
        // Determine which boss we are handling.
        if (boss == "Samurai")
        {
            Debug.Log("Test");
        }

        // Now that we're done. We can introduce the boss to view of the player.
        settingUp = true;
    }

    public void HandleEvent(string tag)
    {
        if(tag == "Attack1")
        {
            attack1 = true;
        }

        if(tag == "Attack2")
        {
            attack2 = true;
        }

        if(tag == "Block")
        {
            block = true;
        }

        if(tag == "Charge")
        {
            charge = true;
        }
    }

    public void Reset()
    {
        attack1 = false;
        attack2 = false;
        block = false;
        charge = false;
    }

    /// <summary>
    /// This method will trigger the bosscontroler.
    /// </summary>
    /// <param name="collision"></param>
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // IF Colliding with player and the controller is disabled: Enable it.
        if (collision.gameObject.tag == "Player" && this.enabled == false)
        {
            this.enabled = true;
            //todo: If level 4, load samurai, if lvl 7, load... if lvl 10, load...
            Setup("Samurai");
        }
    }

    private void changeSpeed()
    {
        if (bossObject.transform.position.x > bossIdleLocation.transform.position.x)
        {
            bossObject.GetComponent<EnemyController>().speed += 0.05f;
        }
        else
        {
            bossObject.GetComponent<EnemyController>().speed -= 0.05f;
        }

    }
    /// <summary>
    /// Returns a Vector 2 location based on the % of horizontal/vertical. Reading from left/down to right/up
    /// How to use:
    /// Let's say you want a 10% margin on the right side of the screen, and a 10% margin on the top side of the screen. You'll call calculateLocatonOnScreensize(90, 90)
    /// </summary>
    private Vector2 calculateLocationOnScreensize(float percentageHorizontal, float percentageVertical)
    {
        float x = Screen.width * (percentageHorizontal / 100);
        float y = Screen.height * (percentageVertical / 100);
        return new Vector2(x, y);
    }
}
