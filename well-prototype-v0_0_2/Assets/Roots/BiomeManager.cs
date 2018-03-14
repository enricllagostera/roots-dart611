using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class BiomeManager : Singleton<BiomeManager>
{
    public List<BiomeInfo> biomesDB;
    public List<Biome> biomes;
    public List<Garden> gardens;

    void Start()
    {
        // get a reference to all biomes in the scene
        foreach (var garden in gardens)
        {
            biomes.Add(garden.biome);
        }
    }

    public PlantInfo PickPlant(Biome biome)
    {
        PlantInfo res = null;

        // var fertilityFit = biomesDB.OrderByDescending(b => b.fertilityProbabilityMod.Evaluate(biome.fertility)).First();
        var fit = biomesDB.OrderByDescending(b => b.humidityProbabilityMod.Evaluate(biome.humidity)).First();

        // tree spawning
        float roll = Random.value;
        if (roll <= fit.treeProbability)
        {
            res = fit.plantData.trees[Random.Range(0, fit.plantData.trees.Count)];
            return res;
        }

        // grasses spawning
        roll = Random.value;
        if (roll <= fit.grassProbability)
        {
            res = fit.plantData.grass[Random.Range(0, fit.plantData.grass.Count)];
            return res;
        }

        // flower spawning
        roll = Random.value;
        if (roll <= fit.flowerProbability)
        {
            res = fit.plantData.flowers[Random.Range(0, fit.plantData.flowers.Count)];
            return res;
        }

        // res = fit.plantData.props[Random.Range(0, fit.plantData.props.Count)];
        return res;
    }
}
