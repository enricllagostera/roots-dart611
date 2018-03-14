using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Garden : MonoBehaviour
{
    public float progress;
    public int maxPlants;
    public float preferredMinDistance;
    [SerializeField] private Plant _plantPrefab;
    [SerializeField] private float _plantLineRadius;
    [SerializeField] private int _initialPlantCount;
    [SerializeField] public int baseSortingOrder;
    private Fog _fog;
    private Bee _bee;
    private CloudsFX _cloudsFX;
    public List<Plant> plants;
    public Biome biome;

    void Start()
    {
        _fog = GetComponentInChildren<Fog>();
        _bee = GetComponentInChildren<Bee>();
        _cloudsFX = GetComponentInChildren<CloudsFX>();
        biome.garden = this;
        plants = new List<Plant>();

        while (plants.Count < _initialPlantCount)
        {
            Vector3 random = GetPlantPosition();
            var pick = BiomeManager.Instance.PickPlant(biome);
            if (pick != null)
            {
                SpawnFrom(pick, random);
            }
        }
    }



    void LateUpdate()
    {
        _fog.UpdateColor(progress, baseSortingOrder + 20);
        _bee.sorting = baseSortingOrder + 10;
        _cloudsFX.sorting = baseSortingOrder + 17;
        transform.Find("Ground").GetComponent<SpriteRenderer>().sortingOrder = baseSortingOrder - 200;
        foreach (var plant in plants)
        {
            if (plant == null)
            {
                continue;
            }
            plant.GetComponentInChildren<SpriteRenderer>().sortingOrder = baseSortingOrder + plant.info.sortingOrderMod;
        }
    }


    void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, _plantLineRadius);
    }


    /// Internal function used for basic spawning of the layer.
    void SpawnFrom(List<PlantInfo> plantInfos, Vector3 position)
    {
        if (plantInfos.Count <= 0)
        {
            return;
        }

        int index = Random.Range(0, plantInfos.Count);
        var go = Instantiate<Plant>(_plantPrefab, transform.position, transform.rotation, transform);
        go.transform.localPosition = position;
        go.info = plantInfos[index];
        go.plantIndex = (int)plantInfos[index].kind;
        go.reproductionInterval = plantInfos[index].reproductionInterval;
        // point UP to center
        go.transform.up = transform.position - go.transform.position;
        go.transform.localScale = Vector3.one * plantInfos[index].baseScale;
        go.GetComponentInChildren<SpriteRenderer>().sortingOrder = baseSortingOrder + plantInfos[index].sortingOrderMod;
        plants.Add(go);
    }

    void SpawnFrom(PlantInfo plant, Vector3 position)
    {
        var go = Instantiate<Plant>(_plantPrefab, transform.position, transform.rotation, transform);
        go.transform.localPosition = position;
        go.info = plant;
        go.plantIndex = (int)plant.kind;
        go.reproductionInterval = plant.reproductionInterval;
        // point UP to center
        go.transform.up = transform.position - go.transform.position;
        go.transform.localScale = Vector3.one * plant.baseScale;
        go.GetComponentInChildren<SpriteRenderer>().sortingOrder = baseSortingOrder + plant.sortingOrderMod;
        plants.Add(go);
    }


    Vector3 GetPlantPosition()
    {
        int maxTries = 5;
        var plants = GetComponentsInChildren<Plant>();
        Vector3 random = Random.insideUnitCircle.normalized * _plantLineRadius;
        for (int i = 0; i < maxTries; i++)
        {
            bool found = true;
            random = Random.insideUnitCircle.normalized * _plantLineRadius;
            foreach (var plant in plants)
            {
                if (Vector3.Distance(plant.transform.localPosition, random) < preferredMinDistance)
                {
                    found = false;
                    break;
                }
            }
            if (found)
            {
                return random;
            }
        }
        return random;
    }


    /// Function for creating new plant based on complete info. Used for plant reproduction.
    public void CreateNewPlant()
    {
        if (plants.Count < maxPlants)
        {
            Vector3 random = GetPlantPosition();
            var pick = BiomeManager.Instance.PickPlant(biome);
            while (pick == null)
            {
                pick = BiomeManager.Instance.PickPlant(biome);
            }
            SpawnFrom(pick, random);
        }
    }
}
