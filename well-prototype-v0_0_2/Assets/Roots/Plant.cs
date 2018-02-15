using UnityEngine;
using System.Collections.Generic;

public class Plant : MonoBehaviour
{
    // input
    public KeyCode nutrientKey;
    public KeyCode growthKey;
    public KeyCode reproduceKey;
    // gameplay
    public float health; // 0..1
    public float growth; // 0..1
    public float growthSpeed;
    public float healthDecay;
    public float growthDecay;
    //FX
    private ParticleFX nutrientFX;
    private ParticleFX reproduceFX;
    private SpriteRenderer _visual;

    void Start()
    {
        // wire-up
        nutrientFX = transform.Find("Nutrient").GetComponentInChildren<ParticleFX>();
        reproduceFX = transform.Find("Reproduce").GetComponentInChildren<ParticleFX>();
        _visual = GetComponentInChildren<SpriteRenderer>();
        // gameplay init
        health = 1f;
        growth = 0f;
    }


    void Update()
    {
        // calculate all decays
        // get all inputs
        bool nutrientsInput = Input.GetKey(nutrientKey);
        bool growthInput = Input.GetKey(this.growthKey);
        bool reproduceInput = Input.GetKey(reproduceKey);
        // nutrient action feedback
        nutrientFX.Run(nutrientsInput, _visual.sortingOrder);
        reproduceFX.Run(reproduceInput, _visual.sortingOrder);
        if (Input.GetKey(this.growthKey))
        {
            growth = Mathf.Clamp01(growth + Time.deltaTime * growthSpeed);
        }
        // growth action feedback
        var scale = transform.localScale;
        scale.y = growth;
        transform.localScale = scale;
    }

    public void RandomizeInputs()
    {
        List<KeyCode> keys = new List<KeyCode>(Roots.Instance.availableInputs);
        int assigned = 0;
        while (assigned < 3)
        {
            int i = Random.Range(0, keys.Count);
            KeyCode pick = keys[i];
            if (assigned == 0)
            {
                nutrientKey = pick;
            }
            else if (assigned == 1)
            {
                growthKey = pick;
            }
            else if (assigned == 2)
            {
                reproduceKey = pick;
            }
            assigned++;
            keys.RemoveAt(i);
        }
    }

    public void ClearInputs()
    {
        nutrientKey = KeyCode.None;
        growthKey = KeyCode.None;
        reproduceKey = KeyCode.None;
    }
}