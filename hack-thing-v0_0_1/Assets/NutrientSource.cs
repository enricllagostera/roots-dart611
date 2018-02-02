using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NutrientSource : MonoBehaviour
{
    public float drainRate;
    public float level;
    public float levelMax;
    public float regenerationRate;

    public Gradient colors;
    public SpriteRenderer visual;

    void Start()
    {
        level = levelMax;
    }

    void Update()
    {
        level += regenerationRate * Time.deltaTime;
        float coeficient = level / levelMax;
        visual.color = colors.Evaluate(coeficient);
    }

    public float Drain()
    {
        level = Mathf.Clamp(level - drainRate * Time.deltaTime, 0, levelMax);
        return level;
    }
}