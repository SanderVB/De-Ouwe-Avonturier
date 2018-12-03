using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class MobileUI : MonoBehaviour {
#if UNITY_STANDALONE || UNITY_WEBPLAYER
}
#else
    public Button buttonUp;
    public Button buttonLeft;
    public Button buttonRight;
    public Button buttonJump;

    //public float XMovement;
    //public float YMovement;
    public bool Jump;
    private float jumpCooldown;

    private Player_controller thePlayer;
    private PauseScreen thePauseScreen;
    private LevelManager theLevelManager;

    // Use this for initialization
    void Start ()
    {
        theLevelManager = FindObjectOfType<LevelManager>();
        thePauseScreen = FindObjectOfType<PauseScreen>();
        thePlayer = FindObjectOfType<Player_controller>();
        //XMovement = 0;
        //YMovement = 0;
        jumpCooldown = 0.2f;
	}
	
	// Update is called once per frame
	void Update ()
    {
        if (jumpCooldown > 0)
            jumpCooldown -= Time.deltaTime;
        else
        {
            Jump = false;
            jumpCooldown = 0.2f;
        }
    }

    public void LeftClick()
    {
        thePlayer.XMovement = -thePlayer.moveSpeed;
    }

    public void LeftRightDown()
    {
        thePlayer.XMovement = 0;
    }


    public void RightClick()
    {
        thePlayer.XMovement = thePlayer.moveSpeed;
    }

    public void UpClick()
    {
        thePlayer.YMovement = thePlayer.moveSpeed;
    }

    public void DownClick()
    {
        thePlayer.YMovement = -thePlayer.moveSpeed;
    }

    public void DownUpDown()
    {
        thePlayer.YMovement = 0;
    }

    public void JumpClick()
    {
        Jump = true;
        jumpCooldown = 0.1f;
    }

    public void JumpDown()
    {
        Jump = false;
    }
    public void PauseDown()
    {
        if (theLevelManager.paused)
            thePauseScreen.ResumeGame();
        else
            thePauseScreen.PauseGame();
    }
}
#endif