using UnityEngine;
using System.Collections;

public class BulletController : MonoBehaviour {

    public float speed;
    public float damage;
    public Vector2 direction;
    private GameObject border;

    // Use this for initialization
    void Start () {
        border = GameObject.Find("Right Camera Border");
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

    public void Deflect()
    {
        direction.x = -1;
        direction.y = (float) Random.Range(-1f, 1f);
    }

    public void ResetDirections()
    {
        direction.x = 1;
        direction.y = 0;
    }

    // Check if the bullet hits another object
    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.tag == "Enemy")
        {
            other.gameObject.SendMessage("ApplyDamage", damage);
            Destroy(gameObject);
        }
        if(other.gameObject.tag == "Samurai")
        {
            GameObject.FindGameObjectWithTag("Trigger").SendMessage("IsHit", this.gameObject);

        }
    }
}
