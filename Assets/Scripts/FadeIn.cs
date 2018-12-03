using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FadeIn : MonoBehaviour
{

    public float fadeTime;
    private float fadeCounter;
    private Image blackScreen;
    public bool fadeIn;
    public bool fadeOut;
    // public bool fadeInAndOut;
    public float alphaValue;

    // Use this for initialization
    void Start()
    {
        fadeCounter = fadeTime;
        //fadeInAndOut = false;
        fadeIn = true;
        fadeOut = false;
        blackScreen = GetComponent<Image>();
    }

    // Update is called once per frame
    void Update()
    {
        alphaValue = blackScreen.color.a;
        if (fadeIn)
        {
            //blackScreen.color = new Color(0, 0, 0, 0.99f);
            blackScreen.CrossFadeAlpha(0f, fadeTime, false);

            if (fadeCounter >= 0)
            {
                fadeCounter -= Time.deltaTime;
            }
            else
            {
                //gameObject.SetActive(false);
                fadeIn = false;
                fadeCounter = fadeTime;
            }

        }
        /*else if (fadeInAndOut)
        {
            blackScreen.color = new Color(0, 0, 0, 0.01f);
            blackScreen.CrossFadeAlpha(255, fadeTime, false);
            if (2*fadeCounter >= 0)
            {
                fadeCounter -= Time.deltaTime;
            }
            else
            {
                //gameObject.SetActive(false);
                fadeInAndOut = false;
                fadeCounter = fadeTime;
                blackScreen.CrossFadeAlpha(0f, fadeTime, false);
            }
        }*/
        else if (fadeOut)
        {
            blackScreen.color = new Color(0, 0, 0, 0.01f);
            blackScreen.CrossFadeAlpha(255, fadeTime, false);
            if (fadeCounter >= 0)
            {
                fadeCounter -= Time.deltaTime;
            }
            else
            {
                //gameObject.SetActive(false);
                fadeOut = false;
                fadeCounter = fadeTime;
            }
        }
    }

    public void Terminate()
    {
        gameObject.SetActive(false);
    }
}