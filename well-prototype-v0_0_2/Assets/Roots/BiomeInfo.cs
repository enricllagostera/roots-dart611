using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class BiomeInfo
{
    public string name;
    public float treeProbability;
    public float grassProbability;
    public float flowerProbability;

    public AnimationCurve humidityProbabilityMod;
    public AnimationCurve fertilityProbabilityMod;
    public PoolConfig plantData;
}