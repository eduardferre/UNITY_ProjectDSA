using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameOverScreen : MonoBehaviour
{
    public Text distanceText;
    public Text timeText;
    public Text coinsText;

    public float distanceFly;
    public float timeFly;
    public uint coinsCollected;

    void Start()
    {
        distanceFly = PlaneController.distanceTravelled;
        timeFly = PlaneController.timeFlying;
        coinsCollected = PlaneController.coins;
    }

    void Update()
    {

        distanceText.text = "You have flown " + distanceFly.ToString() + " meters";
        timeText.text = "Your game lasted " + timeFly.ToString() + " seconds";
        coinsText.text = "You have collected " + coinsCollected.ToString() + " Bitcoins";
    }

    public void RestartButton()
    {
        SceneManager.LoadScene("Insignia");
    }

    public void MainMenuButton()
    {
        
    }



}