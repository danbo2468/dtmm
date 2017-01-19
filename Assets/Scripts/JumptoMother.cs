using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumptoMother : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {

            collision.gameObject.GetComponent<PlayerController>().Jump();
            GameObject.Find("Jumper(Clone)").GetComponent<EnemyController>().SendMessage("ApplyDamage",15);
            Destroy(this.gameObject);
        }
    }
}
