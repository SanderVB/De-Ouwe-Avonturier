using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckpointController : MonoBehaviour{

    public Sprite flagClosed;
    public Sprite flagOpen;
    public AudioSource flagSound;

    private SpriteRenderer thespriteRenderer;
    public bool checkpointActive;

	// Use this for initialization
	void Start ()
    {
        thespriteRenderer = GetComponent<SpriteRenderer>();
	}
	
	// Update is called once per frame
	void Update ()
    {
		
	}

    void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            if (!checkpointActive)
            {
                thespriteRenderer.sprite = flagOpen;
                checkpointActive = true;
                flagSound.Play();
            }
        }
    }
}
