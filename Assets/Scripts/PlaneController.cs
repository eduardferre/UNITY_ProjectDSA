using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlaneController : MonoBehaviour
{
    // fer-ho amb percentatges i afegir una força que sigui gravity desactivant la que hi ha al prefab, aixi poder fer lo del pes (calculs al Drive)


    // PARAMETERS

    /*
    public int lifes = 3;
    public float thrustpower = 2f; // (distance)/(time)
    public float liftpower = 10.0f;
    public float burnRate = 0.075f; // (amount)/(time)
    */

    public AndroidBackButton androidInfoScript;


    public string model;
    public int lifes;
    public float thrustpower; // (distance)/(time)
    public float liftpower;
    public float burnRate; // (amount)/(time)

    public float thrustpower_max = 3.5f;
    public float liftpower_max = 25f;
    public float burnRate_max = 0.13f;


    public float maxfuel = 100f;
    private float fuel; // (amount)
    public int refillFuel = 25;

    private Rigidbody2D planeRigidBody;

    private Vector3 startPos;

    public ParticleSystem noozel;

    public Text coinsCollectedLabel;
    public static uint coins = 0;

    public Text livesRemainingLabel;
    public HealthBar healthBar;

    public Text fuelRemainingLabel;
    public FuelBar fuelBar;

    public Text distanceDoneLabel;
    public static float distanceTravelled = 0;

    public Text timeFlyingLabel;
    public static float timeFlying = 0;

    public AudioClip coinCollectSound;
    public AudioClip fuelCollectSound;
    public AudioSource noozelAudio;

    public ParallaxScroll parallax;

    public int timesStageChanged = 1;
    public bool changeStage = false;
    private float distanceBetweenStages = 0f;

    private bool isDead = false;
    private bool gameOver = false;

    private int i = 0;

    // Start is called before the first frame update
    void Start()
    {
        distanceTravelled = 0;
        timeFlying = 0;
        coins = 0;


        fuel = maxfuel;
        fuelBar.setMaxFuel(maxfuel);
        distanceBetweenStages = Random.Range(250, 500); // Stages will change randomly between 250-500m since the previous stage appeared

        planeRigidBody = GetComponent<Rigidbody2D>();
        livesRemainingLabel.text = lifes.ToString();
        startPos = transform.position;

        /*
        model = androidInfoScript.model;
        lifes = androidInfoScript.enginesLife;
        thrustpower = thrustpower_max * (androidInfoScript.velX / 100);
        liftpower = liftpower_max * (androidInfoScript.velY / 100);
        burnRate = burnRate_max * (androidInfoScript.fuel / 100);

        planeRigidBody = GetComponent<Rigidbody2D>();
        livesRemainingLabel.text = lifes.ToString();
        startPos = transform.position;
        healthBar.setMaxHealth(lifes);
        fuelBar.setMaxFuel(maxfuel);
        */
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (i == 0)
        {
            // Defining the parameters from Android

            model = androidInfoScript.model;
            lifes = androidInfoScript.enginesLife;
            thrustpower = thrustpower_max * (androidInfoScript.velX / 100);
            liftpower = liftpower_max * (androidInfoScript.velY / 100);
            burnRate = burnRate_max * (androidInfoScript.fuel / 100);

            healthBar.setMaxHealth(lifes);

            i++;
        }



        bool liftActive = Input.GetButton("Fire1");
        bool descendActive = Input.GetButton("Fire2");

        liftActive = liftActive && !isDead;
        descendActive = descendActive && !isDead;

        if (liftActive) planeRigidBody.AddForce(new Vector2(0, liftpower));
        if (descendActive) planeRigidBody.AddForce(new Vector2(0, -liftpower));

        if (!isDead)
        {
            Vector2 newVelocity = planeRigidBody.velocity;
            newVelocity.x = thrustpower;
            planeRigidBody.velocity = newVelocity;
            noozelAudio.volume = 0.4f;
        }
        else
        {
            noozelAudio.enabled = false;
        }

        parallax.offset = transform.position.x;

        AdjustEmissions(!isDead);
    }

    void Update()
    {
        Vector3 distanceVector = transform.position - startPos;
        float distanceThisFrame = distanceVector.magnitude;
        distanceTravelled += distanceThisFrame;
        startPos = transform.position;

        float distanceShowed = Mathf.Round(distanceTravelled);
        distanceDoneLabel.text = distanceShowed.ToString();

        if (!isDead)
        {
            timeFlying += Time.deltaTime;
            float timeFly = Mathf.Round(timeFlying);
            timeFly = Mathf.Round(timeFlying * 10f) / 10f;
            timeFlyingLabel.text = "time: " + timeFly.ToString() + "0s";
        }

        if (fuel >= 0 & !isDead)
        {
            fuel -= burnRate;
            float fuelShowed = Mathf.Round(fuel);
            fuelBar.setFuel(fuel);
            fuelRemainingLabel.text = fuelShowed.ToString();
        }
        else
        {
            fuel = 0;
            isDead = true;
            gameOver = true;
        }

        if (gameOver) GameOver();

        // Change of Stage and Increase in Difficulty
        if (distanceTravelled > distanceBetweenStages * timesStageChanged & !isDead)
        {
            distanceBetweenStages = Random.Range(250, 500); // Once the stage has changed, the following stage will be generated randomly between 250-500m
            timesStageChanged++;
            changeStage = true;

            thrustpower += 0.15f * timesStageChanged;
        }
    }

    void AdjustEmissions(bool noozelActive)
    {
        var noozelEmission = noozel.emission;
        if (noozelActive)
        {
            noozelEmission.rateOverTime = 100.0f;
        }
        else
        {
            noozelEmission.rateOverTime = 0.0f;
        }
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.CompareTag("Coins")) CollectCoin(collider);
        else if (collider.gameObject.CompareTag("Fuel")) CollectFuel(collider);
        else HitByLaser(collider);
    }

    void HitByLaser(Collider2D laserCollider)
    {
        if (!isDead)
        {
            AudioSource laserZap = laserCollider.gameObject.GetComponent<AudioSource>();
            laserZap.Play();

            if (lifes > 1)
            {
                lifes -= 1;
            }
            else
            {
                lifes = 0;
                isDead = true;
            }
        }

        healthBar.setHeath(lifes);
        livesRemainingLabel.text = lifes.ToString();
    }

    void CollectCoin(Collider2D coinCollider)
    {
        coins++;
        coinsCollectedLabel.text = coins.ToString();
        Destroy(coinCollider.gameObject);
        AudioSource.PlayClipAtPoint(coinCollectSound, transform.position);
    }

    void CollectFuel(Collider2D fuelCollider)
    {
        if (!isDead)
        {
            fuel += refillFuel;

            if (fuel > maxfuel) fuel = maxfuel;

            float fuelShowed = Mathf.Round(fuel);
            fuelBar.setFuel(fuel);
            fuelRemainingLabel.text = fuelShowed.ToString();
            Destroy(fuelCollider.gameObject);
            AudioSource.PlayClipAtPoint(fuelCollectSound, transform.position);
        }
    }

    public void GameOver()
    {
        gameOver = false;

        float delayTime = 8f;

        Invoke("ChangeScene", delayTime);
    }

    public void ChangeScene()
    {
        SceneManager.LoadScene("GameOverScene");
    }
}
