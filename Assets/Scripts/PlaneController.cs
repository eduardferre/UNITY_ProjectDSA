using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlaneController : MonoBehaviour
{

    public float liftpower = 15.0f;
    public float thrustpower = 2.0f; // (distance)/(time)
    private Rigidbody2D planeRigidBody;

    private Vector3 startPos;

    public int lives = 3;
    private bool isDead = false;

    public float maxfuel = 100f;
    private float fuel; // (amount)
    public float burnRate = 0.1f; // (amount)/(time)
    public int refillFuel = 20;

    public ParticleSystem noozel;

    public Text coinsCollectedLabel;
    private uint coins = 0;

    public Text livesRemainingLabel;
    public HealthBar healthBar;

    public Text fuelRemainingLabel;
    public FuelBar fuelBar;

    public Text distanceDoneLabel;
    private float distanceTravelled = 0;

    public AudioClip coinCollectSound;
    public AudioClip fuelCollectSound;
    public AudioSource noozelAudio;

    public ParallaxScroll parallax;



    // Start is called before the first frame update
    void Start()
    {
        fuel = maxfuel;

        planeRigidBody = GetComponent<Rigidbody2D>();
        livesRemainingLabel.text = lives.ToString();
        startPos = transform.position;
        healthBar.setMaxHealth(lives);
        fuelBar.setMaxFuel(maxfuel);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
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
        }

        if (lives == 1)
        {
            lives -= 1;
            isDead = true;
        }
        else lives -= 1;

        healthBar.setHeath(lives);
        livesRemainingLabel.text = lives.ToString();
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
        fuel += refillFuel;

        if (fuel > maxfuel) fuel = maxfuel;

        float fuelShowed = Mathf.Round(fuel);
        fuelBar.setFuel(fuel);
        fuelRemainingLabel.text = fuelShowed.ToString();
        Destroy(fuelCollider.gameObject);
        AudioSource.PlayClipAtPoint(fuelCollectSound, transform.position);
    }
}
