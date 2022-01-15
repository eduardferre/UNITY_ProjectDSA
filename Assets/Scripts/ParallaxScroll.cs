using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParallaxScroll : MonoBehaviour
{
    public Renderer[] backgrounds;
    public Renderer[] background_builds;
    public Renderer[] middles;
    public Renderer[] foreground_builds;

    public Renderer background;
    public Renderer background_build;
    public Renderer middle;
    public Renderer foreground_build;
   
    public float backgroundSpeed = 0.02f;
    public float background_buildSpeed = 0.04f;
    public float middleSpeed = 0.06f;
    public float foregroundSpeed = 0.08f;

    public float offset = 0.0f;

    GameObject planePlayer;
    PlaneController planeScript;

    public int checkChangeStage = 0;

    public string namePlane;


    // Start is called before the first frame update
    void Start()
    {
        // AQUÍ ÉS ON ES CRIDARÀ EL NOM DE L'AVIÓ
        namePlane = "Airbus";

        backgrounds[1].enabled = false;
        background_builds[1].enabled = false;
        middles[1].enabled = false;
        foreground_builds[1].enabled = false;
        backgrounds[2].enabled = false;
        background_builds[2].enabled = false;
        middles[2].enabled = false;
        foreground_builds[2].enabled = false;


        planePlayer = GameObject.FindGameObjectWithTag(namePlane);
        planeScript = planePlayer.GetComponent<PlaneController>();
    }

    // Update is called once per frame
    void Update()
    {
        if (planeScript.changeStage)
        {
            background.enabled = false;
            background_build.enabled = false;
            middle.enabled = false;
            foreground_build.enabled = false;

            if (planeScript.timesStageChanged == 1 + checkChangeStage) // (1, 4, 7...)
            {
                background = backgrounds[0];
                background_build = background_builds[0];
                middle = middles[0];
                foreground_build = foreground_builds[0];
            }
            else if (planeScript.timesStageChanged == 2 + checkChangeStage) // (2, 5, 8...)
            {
                background = backgrounds[1];
                background_build = background_builds[1];
                middle = middles[1];
                foreground_build = foreground_builds[1];
            }
            else if (planeScript.timesStageChanged == 3 + checkChangeStage)// (3, 6, 9...)
            {
                background = backgrounds[2];
                background_build = background_builds[2];
                middle = middles[2];
                foreground_build = foreground_builds[2];

                checkChangeStage += 3;
            }

            background.enabled = true;
            background_build.enabled = true;
            middle.enabled = true;
            foreground_build.enabled = true;


            planeScript.changeStage = false;
        }


        float backgroundOffset = offset * backgroundSpeed;
        float background_buildOffset = offset * background_buildSpeed;
        float middleOffset = offset * middleSpeed;
        float foreground_buildOffset = offset * foregroundSpeed;

        background.material.mainTextureOffset = new Vector2(backgroundOffset, 0);
        background_build.material.mainTextureOffset = new Vector2(background_buildOffset, 0);
        middle.material.mainTextureOffset = new Vector2(middleOffset, 0);
        foreground_build.material.mainTextureOffset = new Vector2(foreground_buildOffset, 0);
    }
}
