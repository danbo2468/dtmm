using UnityEngine;
using System.Collections;


public class ColliderMessage : MonoBehaviour {

    public BossController boss;

    void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Player")
            boss.GetComponent<BossController>().SendMessage("HandleEvent", gameObject.tag);
    }
}
