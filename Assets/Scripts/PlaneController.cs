using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlaneController : MonoBehaviour
{

    public float liftpower = 15.0f;
    public float thrustpower = 2.0f;
    private Rigidbody2D planeRigidBody;

    private Vector3 startPos;

    public float lives = 1.0f;
    private bool isDead = false;

    public ParticleSystem noozel;

    private uint coins = 0;
    private float distanceTravelled = 0;

    public Text coinsCollectedLabel;
    public Text livesRemainingLabel;
    public Text distanceDoneLabel;


    // Start is called before the first frame update
    void Start()
    {
        planeRigidBody = GetComponent<Rigidbody2D>();
        livesRemainingLabel.text = lives.ToString();
        startPos = transform.position;
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
        }

        AdjustEmissions(!isDead);

    }

    private void Update()
    {
        Vector3 distanceVector = transform.position - startPos;
        float distanceThisFrame = distanceVector.magnitude;
        distanceTravelled += distanceThisFrame;
        startPos = transform.position;

        float distanceShowed = Mathf.Round(distanceTravelled);
        distanceDoneLabel.text = distanceShowed.ToString();
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
        if (collider.gameObject.CompareTag("Coins"))
        {
            CollectCoin(collider);
        }
        else
        {
            HitByLaser(collider);
        }
    }

    void HitByLaser(Collider2D laserCollider)
    {
        if (lives == 1)
        {
            lives -= 1;
            isDead = true;
        }
        else lives -= 1;

        livesRemainingLabel.text = lives.ToString();
    }

    void CollectCoin(Collider2D coinCollider)
    {
        coins++;
        coinsCollectedLabel.text = coins.ToString();
        Destroy(coinCollider.gameObject);
    }
}
