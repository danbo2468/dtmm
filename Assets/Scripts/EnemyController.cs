﻿using UnityEngine;
using System.Collections;

public class EnemyController : MonoBehaviour {

    // Powers
    public float speed;
    public float health;
    public float touchDamage;

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
        health -= damage;
        if (health <= 0)
        {
            GetComponent<Animator>().SetBool("Killed", true);
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
}
