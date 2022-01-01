using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parallax : MonoBehaviour
{
    /*
    private float length, startPos;
    public float parallaxFactor;
    public GameObject cam;


    void Start()
    {
        startPos = transform.position.x;
        length = GetComponent<SpriteRenderer>().bounds.size.x;

    }

    void FixedUpdate()
    {
        //float temp = (cam.transform.position.x * (1 - parallaxFactor));
        float dist = (cam.transform.position.x * parallaxFactor);

        transform.position = new Vector3(startPos + dist, transform.position.y, transform.position.z);

        //if (temp > startPos + length) startPos += length;
        //else if (temp < startPos - length) startPos -= length;
    }
    */



    
    public Camera cam;
    public Transform subject;


    Vector2 startPosition;
    float startZ;
    float lenght;


    Vector2 travel => (Vector2) cam.transform.position - startPosition;
    float distanceFromSubject => transform.position.z - subject.position.z;
    float clippingPlane => (cam.transform.position.z + (distanceFromSubject > 0 ? cam.farClipPlane : cam.nearClipPlane));
    float parallaxFactor => Mathf.Abs(distanceFromSubject) / clippingPlane;


    Vector2 parallaxVector;


    // Start is called before the first frame update
    void Start()
    {
        startPosition = transform.position;
        startZ = transform.position.z;

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Vector2 newPos = startPosition + travel * parallaxFactor;
        transform.position = new Vector3(newPos.x, newPos.y, startZ);

    }
    
}
