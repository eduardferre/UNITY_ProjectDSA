using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageGenerator : MonoBehaviour
{
    //Stages

    public GameObject[] prefabsStages;
    //public GameObject availableStages;
    public List<GameObject> currentStages;
    private float screenWidthInPoints;

    //Obstacles and Coins
    public GameObject[] availableObjects;
    public GameObject[] availableFuels;
    public List<GameObject> objects;
    public List<GameObject> fuelItems;

    public float objMinDistance = 2.5f;
    public float objMaxDistance = 5.0f;

    public float fuelDistance = 6.0f;

    public float obstaclesMinRotation = -90.0f;
    public float obstaclesMaxRotation = 90.0f;

    // Defining the plane to change/see their variables
    GameObject planePlayer;
    PlaneController planeScript;
    Camera mainCamera;
    CameraFollow cameraScript;

    public int checkChangeStage = 0;
    public int stageNum = 0;

    public string namePlane;

    public AndroidBackButton androidInfoScript;

    private int i = 0;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(GeneratorCheck());
    }


    void AddStage(float farthestStageEndX)
    {
        //int randomStageIndex = Random.Range(0, availableStages.Length);
        //GameObject stage = (GameObject) Instantiate(availableStages[randomStageIndex]);


        GameObject stage = (GameObject) Instantiate(prefabsStages[stageNum]);

        float stageWidth = stage.transform.Find("Floor").localScale.x;
        float stageCenter = farthestStageEndX + stageWidth * 0.5f;
        stage.transform.position = new Vector3(stageCenter, 0, 0);
        currentStages.Add(stage);
    }

    void AddObject(float lastObjectX)
    {
        int randomIndex = Random.Range(0, availableObjects.Length);
        GameObject obj = (GameObject) Instantiate(availableObjects[randomIndex]);
        float objectX = lastObjectX + Random.Range(objMinDistance, objMaxDistance);
        float objectY = 0;
        float objectRot = 0;


        if (obj.CompareTag("Coins_Line")) objectY = Random.Range(-0.55f, 0.55f);
        else if (obj.CompareTag("Coins_V")) objectY = Random.Range(-0.55f, 0.3f);
        else if (obj.CompareTag("Coins_Vert")) objectY = Random.Range(-0.3f, 0.3f);
        else if (obj.CompareTag("Fuel")) objectY = Random.Range(-0.55f, 0.55f);
        //else if (obj.CompareTag("Obstacle_Horiz")) objectY = Random.Range(-0.6f, 0.6f);
        else if (obj.CompareTag("Obstacle_Vert"))
        {
            objectY = Random.Range(-0.3f, 0.3f);
            objectRot = Random.Range(obstaclesMinRotation, obstaclesMaxRotation);
        }

        obj.transform.position = new Vector3(objectX, objectY, 0);
        obj.transform.rotation = Quaternion.Euler(Vector3.forward * objectRot);
        
        objects.Add(obj);
    }

    void AddFuel(float lastFuelX)
    {
        int randomIndex = Random.Range(0, availableFuels.Length);
        GameObject obj = (GameObject) Instantiate(availableFuels[randomIndex]);
        float objectX = lastFuelX + fuelDistance;
        float objectY = Random.Range(-0.55f, 0.55f);
        

        obj.transform.position = new Vector3(objectX, objectY, 0);

        fuelItems.Add(obj);
    }


    private void GenerateStageIfRequired()
    {
        List<GameObject> stagesToRemove = new List<GameObject>();
        bool addStages = true;
        float playerX = transform.position.x;
        float removeStageX = playerX - screenWidthInPoints;
        float addStageX = playerX + screenWidthInPoints;
        float farthestStageEndX = 0;

        foreach (var stage in currentStages)
        {
            float stageWidth = stage.transform.Find("Floor").localScale.x;
            float stageStartX = stage.transform.position.x - (stageWidth * 0.5f);
            float stageEndX = stageStartX + stageWidth;

            if (stageStartX > addStageX)
            {
                addStages = false;
            }

            if (stageEndX < removeStageX)
            {
                stagesToRemove.Add(stage);
            }

            farthestStageEndX = Mathf.Max(farthestStageEndX, stageEndX);
        }

        foreach (var stage in stagesToRemove)
        {
            currentStages.Remove(stage);
            Destroy(stage);
        }

        if (addStages)
        {
            AddStage(farthestStageEndX);
        }
    }


    void GenerateObjectsIfRequired()
    {
        float playerX = transform.position.x;
        float removeObjectsX = playerX - screenWidthInPoints;
        float addObjectX = playerX + screenWidthInPoints;
        float farthestObjectX = 0;

        List<GameObject> objectsToRemove = new List<GameObject>();

        foreach (var obj in objects)
        {
            float objX = obj.transform.position.x;
            farthestObjectX = Mathf.Max(farthestObjectX, objX);

            if (objX < removeObjectsX)
            {
                objectsToRemove.Add(obj);
            }
        }

        foreach (var obj in objectsToRemove)
        {
            objects.Remove(obj);
            Destroy(obj);
        }

        if (farthestObjectX < addObjectX)
        {
            AddObject(farthestObjectX);
        }
    }

    void GenerateFuelIfRequired()
    {
        float playerX = transform.position.x;
        float removeFuelX = playerX - screenWidthInPoints;
        float addFuelX = playerX + screenWidthInPoints;
        float farthestFuelX = 0;

        List<GameObject> fuelToRemove = new List<GameObject>();


        foreach (var obj in fuelItems)
        {
            float objX = obj.transform.position.x;
            farthestFuelX = Mathf.Max(farthestFuelX, objX);

            if (objX < removeFuelX)
            {
                fuelToRemove.Add(obj);
            }
        }

        foreach (var obj in fuelToRemove)
        {
            fuelItems.Remove(obj);
            Destroy(obj);
        }

        if (farthestFuelX < addFuelX)
        {
            AddFuel(farthestFuelX);
        }
    }

    /* NO FUNCIONA
    void ChangeStageIfRequired()
    {
        if (planeScript.changeStage == true)
        {

            if (planeScript.timesStageChanged == 1 + checkChangeStage) // (1, 4, 7...)
            {
                stageNum = 0;
            }
            else if (planeScript.timesStageChanged == 2 + checkChangeStage) // (2, 5, 8...)
            {
                stageNum = 1;
            }
            else if (planeScript.timesStageChanged == 3 + checkChangeStage)// (3, 6, 9...)
            {
                stageNum = 2;

                checkChangeStage += 3;
            }

            planeScript.changeStage = false;
        }
    }
    */


    private IEnumerator GeneratorCheck()
    {
        while (true)
        {
            if (i == 0)
            {
                // AQUÍ ÉS ON ES CRIDARÀ EL NOM DE L'AVIÓ
                namePlane = "Cessna";

                float height = 2.0f * Camera.main.orthographicSize;
                screenWidthInPoints = height * Camera.main.aspect;

                mainCamera = Camera.main;
                cameraScript = mainCamera.GetComponent<CameraFollow>();

                planePlayer = GameObject.FindGameObjectWithTag(namePlane);
                planeScript = planePlayer.GetComponent<PlaneController>();

                i++;
            }

            if (cameraScript.OK)
            {
                GenerateStageIfRequired();
                GenerateFuelIfRequired();
                GenerateObjectsIfRequired();
                //ChangeStageIfRequired();
            }

            yield return new WaitForSeconds(0.25f);
        }
    }
}
