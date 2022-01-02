using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParallaxScroll : MonoBehaviour
{
    
    public Renderer background;
    public Renderer background_build;
    public Renderer middle;
    public Renderer foreground_build;
   
    public float backgroundSpeed = 0.02f;
    public float background_buildSpeed = 0.04f;
    public float middleSpeed = 0.06f;
    public float foregroundSpeed = 0.08f;


    public float offset = 0.0f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
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
