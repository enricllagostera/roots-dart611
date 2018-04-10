using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Garden : MonoBehaviour
{
    public int gardenIndex;
    public float progress;
    public int plantCapacity;
    public int startSeedlingCount;
    public float preferredMinDistance;
    [SerializeField] private Plant _plantPrefab;
    [SerializeField] private float _plantLineRadius;
    [SerializeField] public int baseSortingOrder;
    private Fog _fog;
    private Bee _bee;
    private CloudsFX _cloudsFX;
    public List<Plant> plants;
    public Biome biome;
    private Ground _ground;
    private int _seedlingCount;
    public bool resetLock;

    void Start()
    {
        _fog = GetComponentInChildren<Fog>();
        _bee = GameObject.FindObjectOfType<Bee>();
        _cloudsFX = GetComponentInChildren<CloudsFX>();
        _ground = GetComponentInChildren<Ground>();
        biome.garden = this;

        plants = new List<Plant>();
        _seedlingCount = 0;

        // populates the whole of the garden
        while (plants.Count < plantCapacity)
        {
            Plant plant = null;
            Vector3 randomPosition = GeneratePlantPosition();
            var plantPick = BiomeManager.Instance.PickPlant(biome);
            if (plantPick != null)
            {
                plant = SpawnFrom(plantPick, randomPosition);
            }
        }
        resetLock = false;
        GenerateSeeds();
    }



    void LateUpdate()
    {
        _fog.UpdateColor(progress, baseSortingOrder + 20);

        _cloudsFX.sorting = baseSortingOrder + 17;
        _ground.SetSortingOrder(baseSortingOrder - 200);
        foreach (var plant in plants)
        {
            if (plant == null)
            {
                continue;
            }
            plant.GetComponentInChildren<SpriteRenderer>().sortingOrder = baseSortingOrder + plant.info.sortingOrderMod;
        }
    }

    /* 
    void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, _plantLineRadius);
    }
     */

    Plant SpawnFrom(PlantInfo plant, Vector3 position)
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
        go.MakeInert();
        return go;
    }


    Vector3 GeneratePlantPosition()
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
    public void ResetPlant()
    {
        // lock during layer reset
        if (resetLock) return;

        List<Plant> deadPlants = plants.Where(p => p.state == EPlantState.INERT).ToList();
        // is there are still available slots for new plants
        if (deadPlants.Count == 0)
        {
            return;
        }
        Plant newPlant = deadPlants.First();
        //Vector3 random = GeneratePlantPosition();
        var plantPick = BiomeManager.Instance.PickPlant(biome);
        while (plantPick == null)
        {
            plantPick = BiomeManager.Instance.PickPlant(biome);
        }
        newPlant.info = plantPick;
        newPlant.plantIndex = (int)plantPick.kind;
        newPlant.reproductionInterval = plantPick.reproductionInterval;
        // point UP to center
        newPlant.transform.localScale = Vector3.one * plantPick.baseScale;
        newPlant.GetComponentInChildren<SpriteRenderer>().sortingOrder = baseSortingOrder + plantPick.sortingOrderMod;
        newPlant.MakeSeed();
    }

    public void Reset()
    {
        plants.ForEach(p => p.MakeInert());
        GenerateSeeds();
        if (_bee.currentGardenIndex == gardenIndex)
        {
            _bee.canTeleport = true;
        }
    }

    public void GenerateSeeds()
    {
        _seedlingCount = 0;
        while (_seedlingCount < startSeedlingCount)
        {
            Plant randomPlant = plants[Random.Range(0, plants.Count)];
            randomPlant.MakeSeed();
            _seedlingCount = plants.Where(p => p.state == EPlantState.SEEDLING).Count();
        }
    }
}