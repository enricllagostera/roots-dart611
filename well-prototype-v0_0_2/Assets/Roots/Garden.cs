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
    [SerializeField] private PoolConfig _pool;
    [SerializeField] private bool _randomizer;
    [SerializeField] private float _treeProbability;
    [SerializeField] private float _flowerProbability;
    [SerializeField] private float _grassProbability;
    [SerializeField] private float _propProbability;
    [SerializeField] private int _initialPlantCount;
    [SerializeField] public int baseSortingOrder;
    private Fog _fog;
    private Bee _bee;
    private CloudsFX _cloudsFX;
    public List<Plant> plants;

    void Start()
    {
        _fog = GetComponentInChildren<Fog>();
        _bee = GetComponentInChildren<Bee>();
        _cloudsFX = GetComponentInChildren<CloudsFX>();
        plants = new List<Plant>();
        // totally random probabilities for testing
        if (_randomizer)
        {
            _treeProbability = Random.Range(0.1f, 0.9f);
            _flowerProbability = Random.Range(0.1f, 0.9f);
            _grassProbability = Random.Range(0.1f, 0.9f);
            _propProbability = Random.Range(0.1f, 0.9f);
        }

        for (int i = 0; i < _initialPlantCount; i++)
        {
            Vector3 random = GetPlantPosition();

            // tree spawning
            float roll = Random.value;
            if (roll <= _treeProbability)
            {
                SpawnFrom(_pool.trees, random);
                continue;
            }

            // grasses spawning
            roll = Random.value;
            if (roll <= _grassProbability)
            {
                SpawnFrom(_pool.grass, random);
                continue;
            }

            // prop spawning
            roll = Random.value;
            if (roll <= _propProbability)
            {
                SpawnFrom(_pool.props, random);
                continue;
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
    public void CreatePlant(PlantInfo info)
    {
        if (plants.Count >= maxPlants)
        {
            return;
        }
        Vector3 pos = GetPlantPosition();
        var go = Instantiate<Plant>(_plantPrefab, transform.position, transform.rotation, transform);
        go.transform.localPosition = pos;
        go.info = info;
        go.plantIndex = (int)info.kind;
        go.reproductionInterval = info.reproductionInterval;
        // point UP to center
        go.transform.up = transform.position - go.transform.position;
        go.transform.localScale = Vector3.one * info.baseScale;
        go.GetComponentInChildren<SpriteRenderer>().sortingOrder = baseSortingOrder + info.sortingOrderMod;
        plants.Add(go);
    }
}
