using UnityEngine;
using System.Collections.Generic;

public class Plant : MonoBehaviour
{
    public int plantIndex;
    public PlantInfo info;
    // input
    public KeyCode nutrientKey;
    public KeyCode growthKey;
    public KeyCode reproduceKey;
    // gameplay
    public float growth; // 0..1
    public float health; // 0..1
    public float growthSpeed;
    public float healthSpeed;
    public float growthDecay;
    public float healthDecay;
    //FX
    private Animator nutrientFX;
    // private Animator reproduceFX;
    public SpriteRenderer visual;
    public float baseSpeed;

    public bool activeGrowth;
    public bool activeNutrient;

    public float reproductionTimer;
    public float reproductionInterval;

    public float age;
    public bool isAlive;

    public float scaleModRange;
    public Gradient tintModRange;
    private Garden _garden;

    void Start()
    {
        // wire-up
        nutrientFX = transform.Find("NutrientFX").GetComponent<Animator>();
        //reproduceFX = transform.Find("Reproduce").GetComponentInChildren<ParticleFX>();
        // gameplay init
        health = 0f;
        growth = 0f;
        baseSpeed = 0.1f;
        // plant index
        reproductionTimer = reproductionInterval;
        age = 0f;
        isAlive = true;
        visual.material.SetColor("_Color", tintModRange.Evaluate(Random.Range(0f, 1f)));
        transform.localScale *= 1f + Random.Range(-scaleModRange / 2f, scaleModRange / 2f);
        _garden = transform.parent.GetComponent<Garden>();
    }


    void Update()
    {
        /* 
        _animator.speed = 0;
        // calculate all decays
        // get all inputs
        bool nutrientsInput = Input.GetKey(nutrientKey);
        bool growthInput = Input.GetKey(this.growthKey);
        bool reproduceInput = Input.GetKey(reproduceKey);
        // nutrient action feedback
        //nutrientFX.Run(nutrientsInput, _visual.sortingOrder);
        */
        //reproduceFX.Run(reproduceInput, _visual.sortingOrder);
        activeGrowth = Input.GetKey(this.growthKey);
        activeNutrient = Input.GetKey(this.nutrientKey);
        age += Time.deltaTime * ((isAlive) ? 1f : -1f);
        nutrientFX.SetBool("Active", activeNutrient);
        visual.sortingOrder = _garden.baseSortingOrder + 5;
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


    public void Reproduce()
    {
        transform.parent.GetComponent<Garden>().CreatePlant(info);
    }
}
