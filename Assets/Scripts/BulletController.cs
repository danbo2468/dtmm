﻿using UnityEngine;
using System.Collections;

public class BulletController : MonoBehaviour {

    public float speed;
    public float damage;
    public Vector2 direction;
    private GameObject border;

    // Use this for initialization
    void Start () {
        border = GameObject.Find("RightViewBorder");
    }
	
	// Update is called once per frame
	void Update () {
        Move();
        if (transform.position.x > border.transform.position.x)
        {
            Destroy(this.gameObject);
        }
    }

    // Move the bullet in the right direction
    void Move()
    {
        Vector2 position = transform.position;
        position += direction * speed * Time.deltaTime;
        transform.position = position;
    }

    // Check if the bullet hits another object
    void OnCollisionEnter2D(Collision2D other)
    {
        if(other.gameObject.tag == "Enemy")
        {
            other.gameObject.SendMessage("ApplyDamage", damage);
            Destroy(gameObject);
        }
    }
}
