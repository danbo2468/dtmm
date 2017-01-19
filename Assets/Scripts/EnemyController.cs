﻿using UnityEngine;
using System.Collections;

public class EnemyController : MonoBehaviour {

    // Powers
    public float speed;
    public float health;
    public float touchDamage;
    public float killingScore;

    // Dying
    public float timeAfterDeath;

    // Use this for initialization
    void Start () {
    }
	
	// Update is called once per frame
	void Update () {

    }

    // Apply damage to the enemy
    void ApplyDamage(float damage)
    {
        Debug.Log("We are hit! damage points: " + damage);
        health -= damage;
        if (health <= 0)
        {
            GameManager.gameManager.levelManager.AddToScore(killingScore);
            GetComponent<Animator>().SetBool("Killed", true);
            GetComponent<BoxCollider2D>().enabled = false;
            GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.None;
            Destroy(gameObject, timeAfterDeath);
        }
    }

    // Check if there is a collision
    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag == "Player" && health > 0)
        {
            other.gameObject.SendMessage("ApplyDamage", touchDamage);
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player" && health > 0)
        {
            if (GameObject.Find("JumpObject") != null)
            {
                Destroy(GameObject.Find("JumpObject"));
            }
            other.gameObject.SendMessage("ApplyDamage", touchDamage);
        }
    }
}
