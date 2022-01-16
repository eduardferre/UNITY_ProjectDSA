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

        distanceText.text = "distance: " + distanceFly.ToString() + " m";
        coinsText.text = "coins: " + coinsCollected.ToString() + " B";
        timeText.text = "time: " + timeFly.ToString() + " s";
    }

    public void RestartButton()
    {
        SceneManager.LoadScene("Insignia");
    }

    public void MainMenuButton()
    {
        
    }
}