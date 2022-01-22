using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class AndroidBackButton : MonoBehaviour
{

    public string model;
    public int enginesLife;
    public float velX;
    public float velY;
    public float fuel;
    public int gravity;


    void Start()
    {
            #if UNITY_ANDROID

              AndroidJavaClass unityComms = new AndroidJavaClass("edu.upc.dsa.UnityComms");
 
              model = unityComms.CallStatic<String>("getModel");
              fuel = unityComms.CallStatic<int>("getFuel");
              int minFuel = unityComms.CallStatic<int>("getMinFuel");
              enginesLife = unityComms.CallStatic<int>("getEnginesLife");
              int maxEnginesLife = unityComms.CallStatic<int>("getMaxEnginesLife");
              velX = unityComms.CallStatic<int>("getVelX");
              int maxSpeed = unityComms.CallStatic<int>("getMaxSpeed");
              velY = unityComms.CallStatic<int>("getVelY");
              int maxManoeuvrability = unityComms.CallStatic<int>("getMaxManoeuvrability");
              gravity = unityComms.CallStatic<int>("getGravity");
              int minWeight = unityComms.CallStatic<int>("getMinWeight");

              Debug.Log("===PASSED PLANE INFO===");
              Debug.Log("Model: "+model);
              Debug.Log("Fuel: "+fuel);
              Debug.Log("minFuel: "+minFuel);
              Debug.Log("enginesLife: "+enginesLife);
              Debug.Log("maxEnginesLife: "+maxEnginesLife);
              Debug.Log("velX: "+velX);
              Debug.Log("maxSpeed: "+maxSpeed);
              Debug.Log("velY: "+velY);
              Debug.Log("maxManoeuvrability: " +maxManoeuvrability);
              Debug.Log("gravity: " +gravity);
              Debug.Log("minweight: "+minWeight);
              Debug.Log("===PASSED PLANE INFO===");
  
            #endif


        /* INTERESTING

        model         -> Cessna, Airbus, Acrobatic, Fighter, Helicopter
        enginesLife   -> *lifes*      (robustness)
        velX          -> *thrust*     (speed)
        velY          -> *lift*       (manoeuvrability)
        fuel          -> *burnRate*  (fuel consumption)
        gravity       -> *weight*     

        All of them in %, we are going to apply the following formula in order to calculate the real value for Unity:

        thrust_max = 3.5f
        lift_max = 25f
        burnRate_max = 0.13f
        "weight" = x

        thrust = thrust_max * velX(%)
        lift = lift_max * velY(%)
        burnRate = burnRate_max * fuel(%)


        EXAMPLE FOR UNITY

        model = "Airbus";
        enginesLife = 4;
        velX = 40f;
        velY = 60f;
        fuel = 60f;

        */
    }

    void FixedUpdate()
    {
        if (Application.platform == RuntimePlatform.Android)
        {
            if (Input.GetKey(KeyCode.Escape))
            {
                Application.Quit();
            }
        }
    }
}
