using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AndroidBackButton : MonoBehaviour
{

 void Start()
 {
#if UNITY_ANDROID
  AndroidJavaClass unityComms = new AndroidJavaClass("edu.upc.dsa.UnityComms");
 
  String model = unityComms.CallStatic<String>("getModel");
  int fuel = unityComms.CallStatic<int>("getFuel");
  int minFuel = unityComms.CallStatic<int>("getMinFuel");
  int enginesLife = unityComms.CallStatic<int>("getEnginesLife");
  int maxEnginesLife = unityComms.CallStatic<int>("getMaxEnginesLife");
  int velX = unityComms.CallStatic<int>("getVelX");
  int maxSpeed = unityComms.CallStatic<int>("getMaxSpeed");
  int velY = unityComms.CallStatic<int>("getVelY");
  int maxManoeuvrability = unityComms.CallStatic<int>("getMaxManoeuvrability");
  int gravity = unityComms.CallStatic<int>("getGravity");
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
 }
    void FixedUpdate(){
      if (Application.platform == RuntimePlatform.Android)
      {
       if (Input.GetKey(KeyCode.Escape))
       {
        Application.Quit();
       }
      }
     }
}
