using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserObstacle : MonoBehaviour
{
    //1 Declaring Sprites States
    public Sprite laserOnSprite;
    public Sprite laserOffSprite;
    //2 Declaring Intervals of Activity and Rotation Speed
    public float toggleInterval = 0.5f;
    public float rotationSpeed = 0.0f;
    //3 Toggle
    private bool isLaserOn = true;
    private float timeUntilNextToggle;
    //4 Colliders
    private Collider2D laserCollider;
    private SpriteRenderer laserRenderer;

    // Start is called before the first frame update
    void Start()
    {
        
        timeUntilNextToggle = toggleInterval;
       
        laserCollider = gameObject.GetComponent<Collider2D>();
        laserRenderer = gameObject.GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        //Counter for Toggle
        timeUntilNextToggle -= Time.deltaTime;
        //Condition to Toggle Sprite and Collider
        if (timeUntilNextToggle <= 0)
        {
        
            isLaserOn = !isLaserOn;
            laserCollider.enabled = isLaserOn;
            
            if (isLaserOn) laserRenderer.sprite = laserOnSprite;
            else laserRenderer.sprite = laserOffSprite;

            //Reset the Time Interval
            timeUntilNextToggle = toggleInterval;
        }

        //If rotation, here is enable the functionality
        transform.RotateAround(transform.position, Vector3.forward, rotationSpeed * Time.deltaTime);
    }
}
