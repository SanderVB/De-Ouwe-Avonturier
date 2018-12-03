using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BossScript : MonoBehaviour {

    public bool bossActive;
    public float timeBetweenDrops;
    private float dropCount;
    private float timeBetweenDropsStore;
    public float platformWaitTime;
    private float platformCount;
    public Transform leftPoint;
    public Transform rightPoint;
    public Transform dropSawSpawnPoint;
    public GameObject dropSaw;
    public GameObject theBoss;
    public bool bossIsRight;
    public GameObject leftPlatforms;
    public GameObject rightPlatforms;
    public int bossHealth;
    private int bossCurHealth;
    public GameObject levelExit;
    private CameraController bossCam;
    private LevelManager theLevelManager;
    public bool waitingForRespawn;
    public AudioSource dropSound;

    public Image heart1;
    public Image heart2;
    public Image heart3;
    public Sprite heartFull;
    public Sprite heartHalf;
    public Sprite heartEmpty;

    public float hitLength;
    //private float hitCounter;
    public bool takeDamage;
    private bool vulnerable;

    public float landTime;
    private bool hasLanded;
    public float hasLandedCounter;

    private Animator myAnim;
    public GameObject BossHealthMeter;

    public AudioSource hurtSound;
    public GameObject deathExplosion;
    public Transform explodeLocation;

    private bool switchMusic;
    public AudioSource bossMusic;
    private float ogVolume;

    // Use this for initialization
    void Start ()
    {
        theLevelManager = FindObjectOfType<LevelManager>();
        myAnim = GetComponent<Animator>();
        bossCam = FindObjectOfType<CameraController>();
        hasLandedCounter = landTime;
        //hitCounter = hitLength;
        vulnerable = true;
        BossHealthMeter.SetActive(false);
        timeBetweenDropsStore = timeBetweenDrops;
        dropCount = timeBetweenDrops;
        platformCount = platformWaitTime;
        theBoss.transform.position = rightPoint.position;
        bossIsRight = true;
        bossCurHealth = bossHealth;
        switchMusic = true;
        ogVolume = theLevelManager.LevelMusic.volume;
	}
	
	// Update is called once per frame
	void Update ()
    {
        if (theLevelManager.paused)
            bossMusic.Pause();
        else
            bossMusic.UnPause();
        myAnim.SetBool("hasLanded", hasLanded);
        myAnim.SetBool("isHurt", takeDamage);

        if (bossActive && switchMusic)
        {
            theLevelManager.LevelMusic.Stop();
            theLevelManager.LevelMusic.volume = 0;
            bossMusic.Play();
            switchMusic = false;
        }
        else if (!bossActive && !switchMusic)
        {
            theLevelManager.LevelMusic.Play();
            bossMusic.Stop();
            theLevelManager.LevelMusic.volume = ogVolume;
            switchMusic = true;
        }
        if (theLevelManager.respawnCoActive)
        {
            waitingForRespawn = true;
        }
        if(waitingForRespawn && !theLevelManager.respawnCoActive)
        {
            bossCam.followTarget = true;
            BossHealthMeter.SetActive(false);
            theBoss.SetActive(false);
            leftPlatforms.SetActive(false);
            rightPlatforms.SetActive(false);
            dropCount = timeBetweenDrops;
            timeBetweenDrops = timeBetweenDropsStore;
            platformCount = platformWaitTime; 
            theBoss.transform.position = rightPoint.position;
            bossIsRight = true;
            bossCurHealth = bossHealth;
            waitingForRespawn = false;
            updateBossHeartMeter();
            bossActive = false;
        }
        if (bossActive)
        {
            myAnim.SetBool("isActive", true);
            //myAnim.SetBool("hasLanded", isGrounded);
            //myAnim.SetBool("isHurt", isHurt);
            if (hasLandedCounter > 0 && !hasLanded)
            {
                hasLandedCounter -= Time.deltaTime;
            }
            else
            {
                hasLanded = true;
                hasLandedCounter = landTime;
            }

            if (bossCurHealth > 2)
                myAnim.SetBool("LowHealth", false);
            else
                myAnim.SetBool("LowHealth", true);


            BossHealthMeter.SetActive(true);
            bossCam.followTarget = false;
            bossCam.transform.position = Vector3.Lerp(bossCam.transform.position, new Vector3(transform.position.x, bossCam.transform.position.y, bossCam.transform.position.z), bossCam.smoothing * Time.deltaTime);

            theBoss.SetActive(true);
            if (dropCount > 0)
            {
                dropCount -= Time.deltaTime;
            }
            else
            {
                dropSawSpawnPoint.position = new Vector3(Random.Range(leftPoint.position.x + 1, rightPoint.position.x - 1), dropSawSpawnPoint.position.y, dropSawSpawnPoint.position.z);
                Instantiate(dropSaw, dropSawSpawnPoint.position, dropSawSpawnPoint.rotation);
                dropSound.Play();
                dropCount = timeBetweenDrops;
            }
            if (bossIsRight)
            {
                theBoss.transform.localRotation = Quaternion.Euler(0, 0, 0);
                if (platformCount > 0)
                {
                    platformCount -= Time.deltaTime;
                }
                else
                {
                    rightPlatforms.SetActive(true);
                }
            }
            else
            {
                theBoss.transform.localRotation = Quaternion.Euler(0, 180, 0);

                if (platformCount > 0)
                {
                    platformCount -= Time.deltaTime;
                }
                else
                {
                    leftPlatforms.SetActive(true);
                }

            }

            if (takeDamage && vulnerable)
            {
                vulnerable = false;
                hurtSound.Play();
                bossCurHealth -= 1;
                updateBossHeartMeter();
                StartCoroutine("BossRespawn");
            }
        }
	}
    public IEnumerator BossRespawn()
    {
        leftPlatforms.SetActive(false);
        rightPlatforms.SetActive(false);
        platformCount = platformWaitTime;
        if (bossCurHealth <= 0)
        {
            myAnim.SetBool("isDead", true);
            InvokeRepeating("deathExplode", 0, .5f);

            yield return new WaitForSeconds(hitLength*2);
            CancelInvoke();
            levelExit.SetActive(true);
            bossCam.followTarget = true;
            gameObject.SetActive(false);
            theLevelManager.LevelMusic.volume = ogVolume;
            theLevelManager.LevelMusic.Play();
        }


        /*yield return new WaitForSeconds(hitLength);
        if (bossCurHealth <= 0)
        {
            levelExit.SetActive(true);
            bossCam.followTarget = true;
            gameObject.SetActive(false);
        }*/

        else
        {
            yield return new WaitForSeconds(hitLength);
            hasLanded = false;

            if (bossIsRight)
            {
                theBoss.transform.position = leftPoint.position;
            }
            else
            {
                theBoss.transform.position = rightPoint.position;
            }

            bossIsRight = !bossIsRight;

            takeDamage = false;

            timeBetweenDrops = timeBetweenDrops / 1.5f;
        }

        vulnerable = true;
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            bossActive = true;
        }

       // if(collision.tag == "BossGround")
       // {
            //theBoss.SetActive(false);

      //  }
    }

    public void updateBossHeartMeter()
    {
        //UI hearthmeter selector
        switch (bossCurHealth)
        {
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

    public void deathExplode()
    {
        //explosionSpawnPoint.position = new Vector3(Random.Range(theBoss.transform.position.x -1, theBoss.transform.position.x + 1), Random.Range(theBoss.transform.position.y - 1, theBoss.transform.position.y + 1), dropSawSpawnPoint.position.z);

        //Instantiate(deathExplosion, explosionSpawnPoint.position, explosionSpawnPoint.rotation);

        explodeLocation.position = new Vector3(Random.Range(theBoss.transform.position.x-1, theBoss.transform.position.x+1), Random.Range(theBoss.transform.position.y -1, theBoss.transform.position.y +1), theBoss.transform.position.z);
        Instantiate(deathExplosion, explodeLocation.position, explodeLocation.rotation);

    }
}

//backup update method
/*
 *     void Update()
    {
        if (theLevelManager.respawnCoActive)
        {
            bossActive = false;
            waitingForRespawn = true;
        }
        if (waitingForRespawn && !theLevelManager.respawnCoActive)
        {
            bossCam.followTarget = true;
            BossHealthMeter.SetActive(false);
            theBoss.SetActive(false);
            leftPlatforms.SetActive(false);
            rightPlatforms.SetActive(false);
            dropCount = timeBetweenDrops;
            timeBetweenDrops = timeBetweenDropsStore;
            platformCount = platformWaitTime;
            theBoss.transform.position = rightPoint.position;
            bossIsRight = true;
            bossCurHealth = bossHealth;
            waitingForRespawn = false;
            updateBossHeartMeter();

        }
        if (bossActive)
        {
            BossHealthMeter.SetActive(true);
            bossCam.followTarget = false;
            bossCam.transform.position = Vector3.Lerp(bossCam.transform.position, new Vector3(transform.position.x, bossCam.transform.position.y, bossCam.transform.position.z), bossCam.smoothing * Time.deltaTime);

            theBoss.SetActive(true);
            if (dropCount > 0)
            {
                dropCount -= Time.deltaTime;
            }
            else
            {
                dropSawSpawnPoint.position = new Vector3(Random.Range(leftPoint.position.x, rightPoint.position.x), dropSawSpawnPoint.position.y, dropSawSpawnPoint.position.z);
                Instantiate(dropSaw, dropSawSpawnPoint.position, dropSawSpawnPoint.rotation);
                dropSound.Play();
                dropCount = timeBetweenDrops;
            }
            if (bossIsRight)
            {
                if (platformCount > 0)
                {
                    platformCount -= Time.deltaTime;
                }
                else
                {
                    rightPlatforms.SetActive(true);
                }
            }
            else
            {
                if (platformCount > 0)
                {
                    platformCount -= Time.deltaTime;
                }
                else
                {
                    leftPlatforms.SetActive(true);
                }

            }

            if (takeDamage)
            {
                bossCurHealth -= 1;
                updateBossHeartMeter();
                if (bossCurHealth <= 0)
                {
                    levelExit.SetActive(true);
                    bossCam.followTarget = true;
                    gameObject.SetActive(false);
                }

                else
                {
                    if (bossIsRight)
                    {
                        theBoss.transform.position = leftPoint.position;
                    }
                    else
                    {
                        theBoss.transform.position = rightPoint.position;
                    }

                    bossIsRight = !bossIsRight;

                    takeDamage = false;

                    leftPlatforms.SetActive(false);
                    rightPlatforms.SetActive(false);
                    timeBetweenDrops = timeBetweenDrops / 2f;
                    platformCount = platformWaitTime;
                }
            }
        }
    }
    */

