using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelDoorScript : MonoBehaviour {

    public string levelToLoad;
    public bool unlocked;

    public SpriteRenderer doorTop;
    public SpriteRenderer doorBottom;

    public Sprite bottomOpen;
    public Sprite bottomClosed;
    public Sprite topOpen;
    public Sprite topClosed;
    private LevelManager theLevelManager;
    private Player_controller thePlayer;

    // Use this for initialization
    void Start ()
    {
        theLevelManager = FindObjectOfType<LevelManager>();
        thePlayer = FindObjectOfType<Player_controller>();
        PlayerPrefs.SetInt("Level1", 1);
        if(PlayerPrefs.GetInt(levelToLoad) == 1)
        {
            unlocked = true;
        }
        else
        {
            unlocked = false;
        }
        if(unlocked)
        {
            doorTop.sprite = topOpen;
            doorBottom.sprite = bottomOpen;
        }
        else
        {
            doorTop.sprite = topClosed;
            doorBottom.sprite = bottomClosed;
        }
        		
	}
	
	// Update is called once per frame
	void Update ()
    {
		
	}

    private void OnTriggerStay2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
#if UNITY_STANDALONE || UNITY_WEBPLAYER
            if (Input.GetAxisRaw("Vertical") > 0.5f && unlocked)
            {
                StartCoroutine("fade");
            }
#else
            if (collision.tag == "Player" && thePlayer.YMovement > 0 && unlocked)
            {
                StartCoroutine("fade");
            }

#endif
        }
    }

    public IEnumerator fade()
    {
        theLevelManager.fader.fadeOut = true;
        yield return new WaitForSeconds(theLevelManager.fader.fadeTime);
        SceneManager.LoadScene(levelToLoad);
    }

}
