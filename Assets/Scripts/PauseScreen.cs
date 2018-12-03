using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseScreen : MonoBehaviour {
    public string levelSelect;
    public string mainMenu;
    private LevelManager theLevelManager;
    private Player_controller thePlayer;
    public GameObject thePauseScreen;

	// Use this for initialization
	void Start ()
    {
        theLevelManager = FindObjectOfType<LevelManager>();
        thePlayer = FindObjectOfType<Player_controller>();
	}
	
	// Update is called once per frame
	void Update ()
    {
        if (Input.GetButtonDown("Pause"))
        {
            if (Time.timeScale == 0)
                ResumeGame();
            else
                PauseGame();
        }
	}

    public void PauseGame()
    {
        theLevelManager.paused = true;
        thePauseScreen.SetActive(true);
        Time.timeScale = 0;
        thePlayer.canMove = false;
        theLevelManager.LevelMusic.Pause();
        theLevelManager.pauseMusic.Play();
    }

    public void ResumeGame()
    {
        theLevelManager.paused = false;
        thePauseScreen.SetActive(false);
        Time.timeScale = 1f;
        thePlayer.canMove = true;
        theLevelManager.pauseMusic.Stop();
        theLevelManager.LevelMusic.Play();
    }

    public void LevelSelect()
    {
        PlayerPrefs.SetInt("CoinCount", theLevelManager.CoinCount);
        PlayerPrefs.SetInt("PlayerLives", theLevelManager.currentLives);
        Time.timeScale = 1f;
        SceneManager.LoadScene(levelSelect);
    }

    public void QuitToMainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(mainMenu);
    }
}
