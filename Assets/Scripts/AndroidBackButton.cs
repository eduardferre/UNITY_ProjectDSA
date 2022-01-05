using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AndroidBackButton : MonoBehaviour
{
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
