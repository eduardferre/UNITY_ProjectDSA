using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageGenerator : MonoBehaviour
{
    //Stages
    public GameObject[] availableStages;
    public List<GameObject> currentStages;
    private float screenWidthInPoints;

    //Obstacles and Coins
    public GameObject[] availableObjects;
    public List<GameObject> objects;

    public float objMinDistance = 3.0f;
    public float objMaxDistance = 6.0f;

    public float obstaclesMinRotation = -90.0f;
    public float obstaclesMaxRotation = 90.0f;

    // Start is called before the first frame update
    void Start()
    {
        float height = 2.0f * Camera.main.orthographicSize;
        screenWidthInPoints = height * Camera.main.aspect;

        StartCoroutine(GeneratorCheck());
    }


    void AddStage(float farthestStageEndX)
    {
        
        int randomStageIndex = Random.Range(0, availableStages.Length);
        GameObject stage = (GameObject)Instantiate(availableStages[randomStageIndex]);
        float stageWidth = stage.transform.Find("Floor").localScale.x;
        float stageCenter = farthestStageEndX + stageWidth * 0.5f;
        stage.transform.position = new Vector3(stageCenter, 0, 0);
        currentStages.Add(stage);
    }

    void AddObject(float lastObjectX)
    {
        int randomIndex = Random.Range(0, availableObjects.Length);
        GameObject obj = (GameObject)Instantiate(availableObjects[randomIndex]);
        float objectX = lastObjectX + Random.Range(objMinDistance, objMaxDistance);
        float objectY = 0;
        float objectRot = 0;


        if (obj.CompareTag("Coins_Line")) objectY = Random.Range(-0.55f, 0.55f);
        else if (obj.CompareTag("Coins_V")) objectY = Random.Range(-0.55f, 0.35f);
        else if (obj.CompareTag("Coins_Vert")) objectY = Random.Range(-0.5f, 0.5f);
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


    private IEnumerator GeneratorCheck()
    {
        while (true)
        {
            GenerateStageIfRequired();
            GenerateObjectsIfRequired();
            yield return new WaitForSeconds(0.25f);
        }
    }


    // Update is called once per frame
    void Update()
    {
        
    }
}
