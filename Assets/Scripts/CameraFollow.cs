using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public GameObject[] planes;
    public string namePlane;
    public bool OK = false;

    private GameObject targetObject;
    private float distanceToTarget;

    public AndroidBackButton androidInfoScript;

    // Start is called before the first frame update
    void Start()
    {
        // AQUÍ ÉS ON ES CRIDARÀ EL NOM DE L'AVIÓ
        namePlane = androidInfoScript.model;

        foreach (var plane in planes)
        {
            if (plane.tag != namePlane) Destroy(plane.gameObject);
        }

        targetObject = GameObject.FindGameObjectWithTag(namePlane);
        //targetObject.SetActive(true);

        OK = true;

        distanceToTarget = transform.position.x - targetObject.transform.position.x;
    }

    // Update is called once per frame
    void Update()
    {
        float targetObjectX = targetObject.transform.position.x;
        Vector3 newCamPos = transform.position;
        newCamPos.x = targetObjectX + distanceToTarget;
        transform.position = newCamPos;
    }
}
