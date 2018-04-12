using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Well : Singleton<Well>
{
    public AnimationCurve movementCurve;
    public float layerOffset;
    public Gradient gradient;
    public Garden[] gardenLayers;
    public float loopTime;
    public float timer;

    [Range(0.01f, 5f)]
    public float timeScale = 1f;
    public float timeScaleMin, timeScaleMax;

    public float activeLayerMin;
    public float activeLayerMax;
    public float wellDepth;
    public float waterRate;
    public KeyCode[] possibleWaterKeys;
    public float inputStagnationTimer;
    public float inputStagnationThreshold;
    public float pacingFactor;

    void Start()
    {
        gardenLayers = FindObjectsOfType<Garden>();
        // gameplay-related setup
        foreach (var layer in gardenLayers)
        {
            layer.waterKey = possibleWaterKeys[Random.Range(0, possibleWaterKeys.Length)];
        }
        layerOffset = 1f / gardenLayers.Length;
    }


    void Update()
    {
        Time.timeScale = timeScale;
        float progress = (Time.time % loopTime).Map(0, loopTime, 0, 1f);
        for (int i = 0; i < gardenLayers.Length; i++)
        {
            // progress-based handling
            gardenLayers[i].gardenIndex = i;
            float layerProgress = (progress + (layerOffset * i)) % 1f;
            gardenLayers[i].progress = layerProgress;
            if (layerProgress <= 0.1f)
            {
                gardenLayers[i].resetLock = false;
            }
            if (layerProgress >= 0.99f && !gardenLayers[i].resetLock)
            {
                gardenLayers[i].resetLock = true;
                gardenLayers[i].Reset();
            }
            gardenLayers[i].transform.position = new Vector3(0, 0, movementCurve.Evaluate(layerProgress) * wellDepth);
            gardenLayers[i].baseSortingOrder = (int)(gardenLayers[i].transform.position.z * -20f);
            // water input handling
            if (Input.GetKey(gardenLayers[i].waterKey))
            {
                gardenLayers[i].biome.humidity = Mathf.Clamp01(gardenLayers[i].biome.humidity + waterRate * Time.deltaTime);
            }
        }
        UpdateInputMetrics();
    }



    void UpdateInputMetrics()
    {
        inputStagnationTimer += Time.deltaTime;
        float rate = inputStagnationTimer.Map(0, inputStagnationThreshold, 0f, 1f);
        float unclampedTimeScale = Mathf.Lerp(timeScale, rate.Map(0f, 1f, timeScaleMin, timeScaleMax), Time.deltaTime * pacingFactor);
        timeScale = Mathf.Clamp(unclampedTimeScale, timeScaleMin, timeScaleMax);
    }



    public void InputChangeHandler()
    {
        //Debug.Log("zero");
        inputStagnationTimer = 0;
    }
}
