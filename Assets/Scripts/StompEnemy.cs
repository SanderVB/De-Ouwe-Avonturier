using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StompEnemy : MonoBehaviour {

    private Rigidbody2D playerBody;
    public GameObject deathExplosion;
    public float bounce;
    private float stompedDelay;
    private float stompedCounter;
    private bool stomped;

	// Use this for initialization
	void Start ()
    {
        stomped = false;
        stompedDelay = 0.1f;
        stompedCounter = stompedDelay;
        playerBody = GetComponentInParent<Rigidbody2D>(); 
	}
	
	// Update is called once per frame
	void Update ()
    {
        if (stomped && stompedCounter > 0)
        {
            stompedCounter -= Time.deltaTime;
        }
        else
        {
            stomped = false;
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Enemy" && !stomped)
        {
            stomped = true;
            stompedCounter = stompedDelay;
            collision.gameObject.SetActive(false);
            Instantiate(deathExplosion, collision.transform.position, collision.transform.rotation);
            playerBody.velocity = new Vector3(playerBody.velocity.x, bounce, 0f);
        }

        if(collision.tag == "Boss")
        {
            playerBody.velocity = new Vector3(playerBody.velocity.x, bounce, 0f);
            collision.transform.parent.GetComponent<BossScript>().takeDamage = true;
        }
    }
}
