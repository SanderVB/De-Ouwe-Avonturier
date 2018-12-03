using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_controller : MonoBehaviour
{
    public Rigidbody2D myRigidbody;

    public float moveSpeed;
    public float jumpSpeed;
    private float activeMoveSpeed;
    public bool canMove;
    public bool isJumping;

    public Transform groundCheck;
    public float groundCheck_radius;
    public LayerMask WhatIsGround;
    public bool isGrounded;
    private bool onPlatform;
    public float onPlatformSpeedmodifier;
    public bool onLadder;
    public float onLadderSpeed;
    private float onLadderVelocity;
    private float gravityStore;

    private Animator myAnim;

    public Vector3 respawnPosition;

    public LevelManager theLevelManager;

    public GameObject stompBox;

    public float knockbackForce;
    public float knockbackLength;
    private float knockbackCounter;
    public float invinciblityLength;
    private float invinciblityCounter;
    public bool isHurt;

    public AudioSource jumpSound;
    public AudioSource hurtSound;
    public AudioSource waterSplashSound;
    public float soundInterval;
    private float soundTimer;
    public bool waterContact;
    private float speedCapY;

    public float teleportCooldown;
    private float cooldownCounter;
    public bool teleported;
#if UNITY_STANDALONE
#else 
    private MobileUI mobileUI;
    public float XMovement;
    public float YMovement;

#endif

    // Use this for initialization
    void Start ()
    {
        theLevelManager = FindObjectOfType<LevelManager>();
        myRigidbody = GetComponent<Rigidbody2D>();
        myAnim = GetComponent<Animator>();
#if UNITY_STANDALONE
#else 
        mobileUI = FindObjectOfType<MobileUI>();
        XMovement = 0;
        YMovement = 0;
#endif
        cooldownCounter = teleportCooldown;
        waterContact = false;
        isHurt = false;
        canMove = true;
        isJumping = false;
        respawnPosition = transform.position;

        activeMoveSpeed = moveSpeed;
        gravityStore = myRigidbody.gravityScale;
        soundTimer = soundInterval;
        speedCapY = moveSpeed * 3;
    }
	
	// Update is called once per frame
	void Update ()
    {

        if (myRigidbody.velocity.y > speedCapY)
            myRigidbody.velocity = new Vector3(myRigidbody.velocity.x, speedCapY, 0);
        if (myRigidbody.velocity.y < -speedCapY)
            myRigidbody.velocity = new Vector3(myRigidbody.velocity.x, -speedCapY, 0);
        if (!onPlatform)
            transform.parent = null;
        //checks if player standing on something            
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheck_radius, WhatIsGround);

        //knockback on contact-checker
        if (knockbackCounter <= 0 && canMove)
        {
#if UNITY_STANDALONE || UNITY_WEBPLAYER
            //horizontal movement
            if (Input.GetAxisRaw("Horizontal") > 0f)
            {
                myRigidbody.velocity = new Vector3(activeMoveSpeed, myRigidbody.velocity.y, 0f);
                transform.localScale = new Vector3(1f, 1f, 1f);
            }

            else if (Input.GetAxisRaw("Horizontal") < 0f)
            {
                myRigidbody.velocity = new Vector3(-activeMoveSpeed, myRigidbody.velocity.y, 0f);
                transform.localScale = new Vector3(-1f, 1f, 1f);
            }

            else
            {
                myRigidbody.velocity = new Vector3(0f, myRigidbody.velocity.y, 0f);
            }

            //vertical movement
            if (Input.GetButtonDown("Jump") && isGrounded)
            {
                onPlatform = false;
                myRigidbody.velocity = new Vector3(myRigidbody.velocity.x, jumpSpeed, 0f);
                jumpSound.Play();
            }

            if (onLadder)
            {
                myRigidbody.gravityScale = 0f;
                onLadderVelocity = onLadderSpeed * Input.GetAxisRaw("Vertical");
                myRigidbody.velocity = new Vector3(myRigidbody.velocity.x, onLadderVelocity, 0f);
            }
            else
            {
                myRigidbody.gravityScale = gravityStore;
            }

            if(onPlatform)
            {
                activeMoveSpeed = moveSpeed * onPlatformSpeedmodifier;
            }
            else
            {
                activeMoveSpeed = moveSpeed;
            }
#else

            //horizontal movement
            if (XMovement > 0f)
            {
                myRigidbody.velocity = new Vector3(activeMoveSpeed, myRigidbody.velocity.y, 0f);
                transform.localScale = new Vector3(1f, 1f, 1f);
            }

            else if (XMovement < 0f)
            {
                myRigidbody.velocity = new Vector3(-activeMoveSpeed, myRigidbody.velocity.y, 0f);
                transform.localScale = new Vector3(-1f, 1f, 1f);
            }

            else
            {
                myRigidbody.velocity = new Vector3(0f, myRigidbody.velocity.y, 0f);
            }

            //vertical movement
            if (mobileUI.Jump && isGrounded)
            {
                onPlatform = false;
                myRigidbody.velocity = new Vector3(myRigidbody.velocity.x, jumpSpeed, 0f);
                jumpSound.Play();
            }
            if (onLadder)
            {
                myRigidbody.gravityScale = 0f;
                onLadderVelocity = YMovement;
                myRigidbody.velocity = new Vector3(myRigidbody.velocity.x, onLadderVelocity, 0f);
            }
            else
            {
                myRigidbody.gravityScale = gravityStore;
            }

            if(onPlatform)
            {
                activeMoveSpeed = moveSpeed * onPlatformSpeedmodifier;
            }
            else
            {
                activeMoveSpeed = moveSpeed;
            }
#endif

        }

        if (knockbackCounter > 0)
        {
            knockbackCounter -= Time.deltaTime;
            if (transform.localScale.x > 0)
            {
                myRigidbody.velocity = new Vector3(-knockbackForce, knockbackForce, 0f);
            }
            else
            {
                myRigidbody.velocity = new Vector3(knockbackForce, knockbackForce, 0f);

            }
        }

        if(invinciblityCounter > 0)
        {
            invinciblityCounter -= Time.deltaTime;
        }
        else
        {
            isHurt = false;
            theLevelManager.invincible = false;
        }

        //animation selector
        myAnim.SetFloat("Speed", Mathf.Abs(myRigidbody.velocity.x));
        if (myRigidbody.velocity.y == 0 && onLadder)
            myAnim.SetBool("climbStop", true);
        else
            myAnim.SetBool("climbStop", false);
        myAnim.SetBool("Grounded", isGrounded);
        myAnim.SetBool("isHurt", isHurt);
        if(!isGrounded)
            myAnim.SetBool("onLadder", onLadder);
        else
            myAnim.SetBool("onLadder", false);
        if (myRigidbody.velocity.y < 0)
        {
            stompBox.SetActive(true);
        }
        else
            stompBox.SetActive(false);

        if (waterContact && (myRigidbody.velocity.x != 0))
        {
            if (soundTimer > 0)
                soundTimer -= Time.deltaTime;
            else
            {
                waterSplashSound.Play();
                soundTimer = soundInterval;
            }
        }

        if (cooldownCounter > 0 && teleported)
        {
            cooldownCounter -= Time.deltaTime;
        }
        else
        {
            cooldownCounter = teleportCooldown;
            teleported = false;
        }

    }

    public void Knockback()
    {
        isHurt = true;
        knockbackCounter = knockbackLength;
        invinciblityCounter = invinciblityLength;
        theLevelManager.invincible = true;
    }

    //trigger handler
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Killzone")
        {
            //gameObject.SetActive(false);
            //transform.position = respawnPosition;
            theLevelManager.Respawn(0);
        }

        if (collision.tag == "Checkpoint")
        {
            respawnPosition = collision.transform.position;
        }
    }

    //collision handlers
    void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "MovingPlatform")
        {
            onPlatform = true;
            transform.parent = collision.transform;
        }
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "MovingPlatform" && !isGrounded)
        {
            onPlatform = false;
            transform.parent = null;
        }
    }
}
