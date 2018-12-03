using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelManager : MonoBehaviour {

    public float waitToRespawn;
    public Player_controller thePlayer;
    public GameObject DeathExplosion;
    public int CoinCount;
    private int CoinBonusCount;
    public int CoinsNeeded;
    public Text coinText;
    public AudioSource coinSound;
    public Image heart1;
    public Image heart2;
    public Image heart3;
    public Sprite heartFull;
    public Sprite heartHalf;
    public Sprite heartEmpty;
    public int maxHealth;
    public int currentHealth;
    public AudioSource healthSound;
    private bool isRespawning;
    public bool respawnCoActive;
    public bool invincible;
    public int startingLives;
    public int currentLives;
    public Text livesText;
    public AudioSource lifeSound;
    public AudioSource pauseMusic;
    public bool paused;
    public GameObject GameOverScreen;
    public AudioSource GameOverMusic;
    public AudioSource LevelMusic;
    public AudioSource LevelEnd;
    private CameraController theCamera;
    private float cameraSpeedStore;
    public FadeIn fader;
    public ResetOnRespawn[] objectsToReset;

    // Use this for initialization
    void Start ()
    {
        //fader = FindObjectOfType<FadeIn>();
        thePlayer = FindObjectOfType<Player_controller>();
        theCamera = FindObjectOfType<CameraController>();
        currentHealth = maxHealth;
        objectsToReset = FindObjectsOfType<ResetOnRespawn>();

        if (PlayerPrefs.HasKey("CoinCount"))
        {
            CoinCount = PlayerPrefs.GetInt("CoinCount");
        }

        coinText.text = "Coins: " + CoinCount;

        if (PlayerPrefs.HasKey("PlayerLives"))
        {
            currentLives = PlayerPrefs.GetInt("PlayerLives");
        }
        else
        {
            currentLives = startingLives;
        }
        livesText.text = "Lives x " + currentLives;
        paused = false;
	}
	
	// Update is called once per frame
	void Update ()
    {
        if(currentHealth<= 0)
        {
            Respawn();
        }

        //coins for extra life
        if(CoinBonusCount>= CoinsNeeded)
        {
            currentLives += 1;
            lifeSound.Play();
            livesText.text = "Lives x " + currentLives;
            CoinBonusCount -= CoinsNeeded;
        }
    }

    public void Respawn()
    {
        if(!isRespawning)
        {
            currentLives--;
            livesText.text = "Lives x " + currentLives;

            if (currentLives > 0)
            {
                isRespawning = true;
                StartCoroutine("RespawnCo");
            }
            else
            {
                isRespawning = true;
                thePlayer.gameObject.SetActive(false);
                LevelMusic.Stop();
                fader.Terminate();
                GameOverMusic.Play();
                GameOverScreen.SetActive(true);
            }
        }
    }

    public void Respawn(int health)
    {
        currentHealth = health;
        updateHeartMeter();
    }

    public IEnumerator RespawnCo()
    {
        respawnCoActive = true;
        thePlayer.gameObject.SetActive(false);
        Instantiate(DeathExplosion, thePlayer.transform.position, thePlayer.transform.rotation);
        yield return new WaitForSeconds(waitToRespawn);
        fader.fadeOut = true;
        yield return new WaitForSecondsRealtime(fader.fadeTime);
        StartCoroutine("WaitForFadeTime");
        respawnCoActive = false;
        currentHealth = maxHealth;
        updateHeartMeter();
        thePlayer.transform.position = thePlayer.respawnPosition;
        theCamera.transform.position = new Vector3(thePlayer.respawnPosition.x, theCamera.transform.position.y, theCamera.transform.position.z);
        thePlayer.gameObject.SetActive(true);

        CoinCount = 0;
        CoinBonusCount = 0;
        coinText.text = "Coins: " + CoinCount;

        //enemies & coins verschijnen na death
        for (int i = 0; i < objectsToReset.Length; i++)
        {
            objectsToReset[i].gameObject.SetActive(true);
            objectsToReset[i].ResetObject();
        }
        isRespawning = false;
    }
    public IEnumerator WaitForFadeTime()
    {
        yield return new WaitForSecondsRealtime(0);
        fader.fadeIn = true;
    }

        public void AddCoins(int coinsToAdd)
    {
        CoinCount += coinsToAdd;
        CoinBonusCount += coinsToAdd;
        coinText.text = "Coins: " + CoinCount;
        coinSound.Play();
    }

    public void HurtPlayer(int damage)
    {
        if (!invincible)
        {
            currentHealth -= damage;
            updateHeartMeter();
            thePlayer.Knockback();
            thePlayer.hurtSound.Play();
        }
    }

    public void HealPlayer(int heal)
    {
        currentHealth += heal;
        if (currentHealth > maxHealth)
            currentHealth = maxHealth;
        updateHeartMeter();
        healthSound.Play();
    }

    public void updateHeartMeter()
    {
        //UI hearthmeter selector
        switch(currentHealth)
        {
            case 6:
                heart1.sprite = heartFull;
                heart2.sprite = heartFull;
                heart3.sprite = heartFull;
                return;

            case 5:
                heart1.sprite = heartHalf;
                heart2.sprite = heartFull;
                heart3.sprite = heartFull;
                return;

            case 4:
                heart1.sprite = heartEmpty;
                heart2.sprite = heartFull;
                heart3.sprite = heartFull;
                return;

            case 3:
                heart1.sprite = heartEmpty;
                heart2.sprite = heartHalf;
                heart3.sprite = heartFull;
                return;

            case 2:
                heart1.sprite = heartEmpty;
                heart2.sprite = heartEmpty;
                heart3.sprite = heartFull;
                return;

            case 1:
                heart1.sprite = heartEmpty;
                heart2.sprite = heartEmpty;
                heart3.sprite = heartHalf;
                return;

            default:
                heart1.sprite = heartEmpty;
                heart2.sprite = heartEmpty;
                heart3.sprite = heartEmpty;
                return;
        }
    }

    public void updateLives(int livesToAdd)
    {
        currentLives += livesToAdd;
        livesText.text = "Lives x " + currentLives;
        lifeSound.Play();
    }
}
