﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LifePickup : MonoBehaviour {

    public int lifeValue;
    private LevelManager theLevelmanager;

	// Use this for initialization
	void Start ()
    {
        theLevelmanager = FindObjectOfType<LevelManager>();
	}
	
	// Update is called once per frame
	void Update ()
    {
		
	}

     void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            theLevelmanager.updateLives(lifeValue);
            gameObject.SetActive(false);
        }
    }
}
