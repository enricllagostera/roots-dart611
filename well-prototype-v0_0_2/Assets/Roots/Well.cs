using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Well : MonoBehaviour
{
    public AnimationCurve scaleCurve;
    public float layerOffset;
    public Gradient gradient;
    public GroundLayer[] groundLayers;
    public float loopTime;
    public float timer;

    [Range(0.01f, 5f)]
    public float timeScale = 1f;


    void Start()
    {
        groundLayers = FindObjectsOfType<GroundLayer>();
        foreach (var layer in groundLayers)
        {
            layer.visual.transform.Rotate(0f, 0f, Random.Range(0f, 360f));
        }
        layerOffset = 1f / groundLayers.Length;
    }


    void Update()
    {
        Time.timeScale = timeScale;
        float progress = (Time.time % loopTime).Map(0, loopTime, 0, 1);
        for (int i = 0; i < groundLayers.Length; i++)
        {
            float layerProgress = (progress + (layerOffset * i)) % 1f;
            groundLayers[i].transform.localScale = Vector3.one * scaleCurve.Evaluate(layerProgress);
            groundLayers[i].visual.color = gradient.Evaluate(layerProgress);
            groundLayers[i].visual.sortingOrder = (int)(groundLayers[i].transform.localScale.x * 100f);
        }
    }
}
