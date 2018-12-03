using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour {

    private LevelManager theLevelmanager;
    public int CoinValue;
    private bool isCollected;

	// Use this for initialization
	void Start ()
    {
        theLevelmanager = FindObjectOfType<LevelManager>();
        isCollected = false;
	}
	
	// Update is called once per frame
	void Update ()
    {
		
	}

    //collision handler
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            if (!isCollected)
            {
                theLevelmanager.AddCoins(CoinValue);
                //Destroy(gameObject);
                gameObject.SetActive(false);
                isCollected = true;
            }
        }
    }
}
