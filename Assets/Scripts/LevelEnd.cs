using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelEnd : MonoBehaviour {

    public string levelToLoad;
    private Player_controller thePlayer;
    private CameraController theCamera;
    private LevelManager theLevelmanager;
    public string levelToUnlock;

    public float waitToMove;
    public float waitToLoad;
    private bool movePlayer;

    //public Sprite flagOpen;
    //private SpriteRenderer thespriteRenderer;
    private FadeIn fader;

    // Use this for initialization
    void Start ()
    {
        thePlayer = FindObjectOfType<Player_controller>();
        theCamera = FindObjectOfType<CameraController>();
        fader = FindObjectOfType<FadeIn>();
        theLevelmanager = FindObjectOfType<LevelManager>();
        //thespriteRenderer = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update ()
    {
        if(movePlayer)
        {
            thePlayer.myRigidbody.velocity = new Vector3(thePlayer.moveSpeed, thePlayer.myRigidbody.velocity.y, 0f);
        }

    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            theLevelmanager.LevelMusic.Stop();
            theLevelmanager.LevelEnd.Play();
            //thespriteRenderer.sprite = flagOpen;
            StartCoroutine("LevelEndCo");
        }
    }

    public IEnumerator LevelEndCo()
    {
        thePlayer.canMove = false;
        theCamera.followTarget = false;
        theLevelmanager.invincible = true;
        thePlayer.myRigidbody.velocity = Vector3.zero;
        PlayerPrefs.SetInt("CoinCount", theLevelmanager.CoinCount);
        PlayerPrefs.SetInt("PlayerLives", theLevelmanager.currentLives);
        PlayerPrefs.SetInt(levelToUnlock, 1);
        yield return new WaitForSeconds(waitToMove);
        movePlayer = true;
        yield return new WaitForSeconds(waitToMove*2f);
        fader.fadeOut = true;
        yield return new WaitForSeconds(waitToLoad);
        SceneManager.LoadScene(levelToLoad);
    }
}
