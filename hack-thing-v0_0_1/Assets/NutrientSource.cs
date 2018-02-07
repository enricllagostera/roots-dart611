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
        float plantMod = (Garden.Instance.plantCount == 0f) ? 1f : Garden.Instance.plantCount;
        level += regenerationRate * Time.deltaTime * plantMod;
        level = Mathf.Clamp(level, 0, levelMax);
        float coeficient = level / levelMax;
        visual.color = colors.Evaluate(coeficient);
    }

    public float Drain()
    {
        level = Mathf.Clamp(level - (drainRate * Time.deltaTime), 0, levelMax);
        if (level > 0f)
        {
            return drainRate;
        }
        return 0f;
    }
}