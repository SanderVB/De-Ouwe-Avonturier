﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterController : MonoBehaviour {

    private Player_controller thePlayer;

    // Use this for initialization
    void Start()
    {
        thePlayer = FindObjectOfType<Player_controller>();
    }

    // Update is called once per frame
    void Update () {
		
	}

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            thePlayer.waterContact = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            thePlayer.waterContact = false;
        }
    }

}
