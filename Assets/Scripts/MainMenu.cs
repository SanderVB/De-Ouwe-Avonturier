using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour {
    public int StartingLives;
    public string firstLevel;
    public string LevelSelect;
    public string[] levelNames;

	// Use this for initialization
	void Start () {

        //continue greyed out als nog geen game gestart
        /*if (PlayerPrefs.HasKey("CoinCount"))
        {
            
        }*/
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void NewGame()
    {
        SceneManager.LoadScene(firstLevel);
        for(int i=0; i<levelNames.Length; i++)
        {
            PlayerPrefs.SetInt(levelNames[i], 0);
        }
        PlayerPrefs.SetInt("CoinCount", 0);
        PlayerPrefs.SetInt("PlayerLives", StartingLives);

    }

    public void Continue()
    {
        SceneManager.LoadScene(LevelSelect);
    }

    public void QuitGame()
    {
        Application.Quit();
    }

}
