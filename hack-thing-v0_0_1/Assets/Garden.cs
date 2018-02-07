using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Garden : Singleton<Garden>
{
    public float timeForNextPlant;
    public float plantInterval;
    public int plantCount;
    public int plantMax;
    public Plant plantPrefab;
    public float respawnFirstPlantChance;

    public void Start()
    {
        CreatePlant(new Vector3(Random.Range(-7f, 7f), 0, 0));
        timeForNextPlant = plantInterval;
    }

    public void UpdateGarden()
    {
        // countfdown time for next plant
        timeForNextPlant -= Time.deltaTime / plantCount;
        timeForNextPlant = Mathf.Max(0f, timeForNextPlant);
        // if it's time for  new plant and ther is still space
        if (timeForNextPlant <= 0 && plantCount < plantMax)
        {
            // create a new plant in a randomized position
            CreatePlant(new Vector3(Random.Range(-7f, 7f), 0, 0));
            timeForNextPlant = plantInterval;
        }
    }


    public void CreatePlant(Vector3 position)
    {
        var plant = Instantiate<Plant>(plantPrefab, position, Quaternion.identity, transform);
        // connect plant to nutrient sources
        plant.nutrients = new List<NutrientSource>(GameObject.FindObjectsOfType<NutrientSource>());
        plantCount++;
    }


    public void Update()
    {
        // if there are no plants, 
        if (plantCount == 0)
        {
            // there's a chance it might spawn one in a random interval
            if (Random.Range(0f, 1f) < respawnFirstPlantChance)
            {
                CreatePlant(new Vector3(Random.Range(-7f, 7f), 0, 0));
                timeForNextPlant = plantInterval;
            }
        }


    }
}