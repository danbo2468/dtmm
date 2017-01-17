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
    public bool isBlocking = false;
    public bool charge = false;

    public GameObject bossObject;
    public GameObject player;
    public GameObject bossIdleLocation;
    public GameObject chargeLocation;
    public GameObject spawnLocation;
    public GameObject infrontOfPlayer;
    public GameObject blockLocation;

    public BoxCollider2D weaponCollider;

    private List<BoxCollider2D> colliders;

    private Rigidbody2D rigidBody;
    private Animator animator;
    

    private string boss;

    void Start () {
        this.enabled = false;
        weaponCollider.enabled = false;
        rigidBody = bossObject.GetComponent<Rigidbody2D>();
        animator = bossObject.GetComponent<Animator>();
	}
	
	// Update is called once per frame
	void Update ()
    {    
        if (attack1)
        {
            isBusy = true;
            Move(bossObject, infrontOfPlayer, 5f);
            weaponCollider.enabled = true;
            if(bossObject.transform.position == infrontOfPlayer.transform.position)
            {
                animator.SetBool("isAttacking1", true);
                Wait(1);
                Reset();
            }
        }
        else if (attack2)
        {
            isBusy = true;
            Move(bossObject, infrontOfPlayer, 5f);
            weaponCollider.enabled = true;
            if (bossObject.transform.position == infrontOfPlayer.transform.position)
            {
                animator.SetBool("isAttacking2", true);
                Wait(1);
                Reset();
            }
        }
        else if (block)
        {
            isBusy = true;
            Move(bossObject, blockLocation, 10f);
            if (bossObject.transform.position == blockLocation.transform.position)
            {
                isBlocking = true;
                animator.SetBool("isBlocking", true);
                Wait(3); // Reset after 3 seconds.
                Reset();
            }
        }
        else if (charge)
        {
            isBusy = true;
            if (!animator.GetBool("isAttacking2"))
            {
                Move(bossObject, infrontOfPlayer, 20f);
                if (bossObject.transform.position == infrontOfPlayer.transform.position)
                {
                    
                    animator.SetBool("isAttacking2", true);
                    Debug.Log("Setting the bool to true");
                    Debug.Log(animator.GetBool("isAttacking2");
                }
            }
            Debug.Log(animator.GetBool("isAttacking2"));
            if (animator.GetBool("IsAttacking2"))
            {
                Move(bossObject, chargeLocation, 20f);
                if (bossObject.transform.position == chargeLocation.transform.position)
                {
                    bossObject.transform.position = spawnLocation.transform.position;
                    Reset();
                }
            }
        }

        if (!isBusy)
        {
            animator.SetBool("isFlying", true);
            Move(bossObject, bossIdleLocation, 5f);
        }
        
    }

    public void Move(GameObject obj, GameObject target, float speed)
    {
        obj.transform.position = Vector2.MoveTowards(new Vector2(obj.transform.position.x, obj.transform.position.y), new Vector2(target.transform.position.x, target.transform.position.y), speed * Time.deltaTime);
    }

    void Setup()
    {
        bossObject.GetComponent<EnemyController>().speed = player.GetComponent<PlayerController>().moveSpeed;
        rigidBody.velocity = new Vector2(bossObject.GetComponent<EnemyController>().speed, rigidBody.velocity.y);
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

        if(tag == "WeaponCollider")
        {
            // We've hit the player, apply damage. Ask daniel how the best way to approach.
        }
    }
    /// <summary>
    /// Resets all neccesary booleans and animator booleans.
    /// Todo: Detect what boss is loaded and what animations are part of it?
    /// </summary>
    public void Reset()
    {
        attack1 = false;
        attack2 = false;
        block = false;
        charge = false;
        isBusy = false;
        animator.SetBool("isFlying", false);
        animator.SetBool("isBlocking", false);
        animator.SetBool("isAttacking1", false);
        animator.SetBool("isAttacking2", false);
        weaponCollider.enabled = false;
    }

    
    /// <summary>
    /// Probably not used anymore.
    /// </summary>
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

    public IEnumerator Wait(float time)
    {
        yield return new WaitForSeconds(time);
    }

    /// <summary>
    /// When the boss is hit by a weapon, this method is called from the weapon entity.
    /// </summary>
    /// <param name="weapon"></param>
    /// <returns></returns>
    public void IsHit(GameObject weapon)
    {
        Debug.Log("We are hit, sending message to cenemy controller hopefully");
        if (isBlocking)
        {
            weapon.SendMessage("Deflect");
            Wait(0.1f);
            isBlocking = false;
            Reset();
            Wait(0.1f);
            Destroy(weapon);
            animator.SetBool("isBlocking", false);
        }else
        {
            Debug.Log("We are hit, sending message to cenemy controller hopefully");
            bossObject.GetComponent<EnemyController>().SendMessage("ApplyDamage", 1);
        }
    }

    /// <summary>
    /// This method will handle all triggers
    /// </summary>
    /// <param name="collision"></param>
    private void OnTriggerEnter2D(Collider2D collision)
    {

        // IF Colliding with player and the controller is disabled: Enable it.
        if (collision.gameObject.tag == "Player" && this.enabled == false)
        {
            this.enabled = true;
            Setup();
        }
    }
}
