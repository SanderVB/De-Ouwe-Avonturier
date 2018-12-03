using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SecretDoorScript : MonoBehaviour {
    private Player_controller thePlayer;
    public GameObject teleportTarget;
    private Vector3 teleportPosition;
    private FadeIn fader;
    private CameraController theCamera;
    private float CamSpeedStore;
    public bool CamTargetSwitcher;

    // Use this for initialization
    void Start ()
    {
        theCamera = FindObjectOfType<CameraController>();
        fader = FindObjectOfType<FadeIn>();
        thePlayer = FindObjectOfType<Player_controller>();
        teleportPosition = teleportTarget.transform.position;
	}
	
	// Update is called once per frame
	void Update ()
    {

	}

    private void OnTriggerStay2D(Collider2D collision)
    {
#if UNITY_STANDALONE || UNITY_WEBPLAYER
        if (collision.tag == "Player" && Input.GetAxisRaw("Vertical") > 0.5f && !thePlayer.teleported)
        {
            fader.fadeOut = true;
            thePlayer.canMove = false;
            thePlayer.myRigidbody.velocity = new Vector3(0, 0, 0);
            StartCoroutine("WaitForFadeTime");
        }
#else 
        if (collision.tag == "Player" &&  thePlayer.YMovement > 0 && !thePlayer.teleported)
        {
            fader.fadeOut = true;
            thePlayer.canMove = false;
            thePlayer.myRigidbody.velocity = new Vector3(0, 0, 0);
            StartCoroutine("WaitForFadeTime");
        }

#endif
    }

    IEnumerator WaitForFadeTime()
    {
        thePlayer.teleported = true;
        yield return new WaitForSecondsRealtime(fader.fadeTime);
        thePlayer.canMove = true;
        fader.fadeIn = true;
        theCamera.transform.position = new Vector3(teleportTarget.transform.position.x, theCamera.transform.position.y, theCamera.transform.position.z);
        thePlayer.transform.position = new Vector3(teleportPosition.x, teleportPosition.y - 1.5f, teleportPosition.z);
        if (CamTargetSwitcher)
        {
            theCamera.followTarget = !theCamera.followTarget;
        }
    }

}
