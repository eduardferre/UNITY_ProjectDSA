using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveUpDown : MonoBehaviour
{
    GameObject planePlayer;
    PlaneController planeScript;

    public string namePlane;

    public AndroidBackButton androidInfoScript;

    int i = 0;


    // Start is called before the first frame update
    void Update()
    {
        if (i == 0)
        {
            namePlane = androidInfoScript.model;

            planePlayer = GameObject.FindGameObjectWithTag(namePlane);
            planeScript = planePlayer.GetComponent<PlaneController>();
            i++;
        }
    }

    public void UpButtonUP()
    {
        planeScript.liftActive = false;
    }

    public void UpButtonDOWN()
    {
        planeScript.liftActive = true;
    }

    public void DownButtonUP()
    {
        planeScript.descendActive = false;
    }

    public void DownButtonDOWN()
    {
        planeScript.descendActive = true;
    }

}
