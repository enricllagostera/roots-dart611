using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Well : MonoBehaviour
{
    public AnimationCurve movementCurve;
    public float layerOffset;
    public Gradient gradient;
    public Garden[] gardenLayers;
    public float loopTime;
    public float timer;

    [Range(0.01f, 5f)]
    public float timeScale = 1f;

    public float activeLayerMin;
    public float activeLayerMax;
    public float wellDepth;

    void Start()
    {
        gardenLayers = FindObjectsOfType<Garden>();
        foreach (var layer in gardenLayers)
        {
            //layer.visual.transform.Rotate(0f, 0f, Random.Range(0f, 360f));
        }
        layerOffset = 1f / gardenLayers.Length;
    }


    void Update()
    {
        Time.timeScale = timeScale;
        float progress = (Time.time % loopTime).Map(0, loopTime, 0, 1);
        for (int i = 0; i < gardenLayers.Length; i++)
        {
            float layerProgress = (progress + (layerOffset * i)) % 1f;
            gardenLayers[i].progress = layerProgress;
            gardenLayers[i].transform.position = new Vector3(0, 0, movementCurve.Evaluate(layerProgress) * wellDepth);
            // gardenLayers[i].visual.color = gradient.Evaluate(layerProgress);
            gardenLayers[i].baseSortingOrder = (int)(gardenLayers[i].transform.position.z * -20f);
        }
    }
}
