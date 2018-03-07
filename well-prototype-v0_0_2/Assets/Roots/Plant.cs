using UnityEngine;
using System.Collections.Generic;

public class Plant : MonoBehaviour
{
    public int plantIndex;
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
    public float wiltSpeed;
    //FX
    private ParticleFX nutrientFX;
    private ParticleFX reproduceFX;
    private SpriteRenderer _visual;
    private Animator _animator;
    public float baseSpeed;

    public bool activeGrowth;
    public bool activeNutrient;

    void Start()
    {
        // wire-up
        //nutrientFX = transform.Find("Nutrient").GetComponentInChildren<ParticleFX>();
        //reproduceFX = transform.Find("Reproduce").GetComponentInChildren<ParticleFX>();
        _visual = GetComponentInChildren<SpriteRenderer>();
        _animator = GetComponentInChildren<Animator>();
        // gameplay init
        health = 0f;
        growth = 0f;
        baseSpeed = 0.1f;
        // plant index
        _animator.SetFloat("PlantSwitch", plantIndex / 12f);
        _animator.SetFloat("AnimSpeed", 0f);
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

        // final number cleanup
        _animator.SetFloat("Growth", growth);
        _animator.SetFloat("Health", health);
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
