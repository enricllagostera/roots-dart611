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
    public EPlantState state;
    //FX
    public Animator animator;
    private Animator nutrientFX;
    // private Animator reproduceFX;
    public SpriteRenderer visual;
    public float baseSpeed;

    public bool activeGrowth;
    public bool activeNutrient;

    public float reproductionTimer;
    public float reproductionInterval;

    public float age;


    public float scaleModRange;
    public Gradient tintModRange;
    private Garden _garden;

    public float growthAnimationMod;
    public float growthAnimationSpeed;
    private float _scaleMod;

    public InputHandler NotifyInputChange;
    public NewPlantFX newPlantFX;
    public NewPlantFX deadPlantFX;
    public Vector2 activeRange;

    void Awake()
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
        visual.material.SetColor("_Color", tintModRange.Evaluate(Random.Range(0f, 1f)));
        _scaleMod = 1f + Random.Range(-scaleModRange / 2f, scaleModRange / 2f);
        visual.transform.localScale *= _scaleMod;
        _garden = transform.parent.GetComponent<Garden>();
        animator = transform.Find("Visual").GetComponent<Animator>();
    }


    void Update()
    {
        if (state == EPlantState.INERT)
        {
            nutrientFX.SetBool("Active", false);
            return;
        }

        visual.sortingOrder = _garden.baseSortingOrder + 5;

        if (_garden.progress < activeRange.x || _garden.progress > activeRange.y)
        {
            activeGrowth = activeNutrient = false;
            nutrientFX.SetBool("Active", false);
            return;
        }

        bool oldInput = activeGrowth;
        activeGrowth = Input.GetKey(this.growthKey);
        activeNutrient = activeGrowth;

        bool alive = (state == EPlantState.SEEDLING || state == EPlantState.NORMAL);
        age += Time.deltaTime * (alive ? 1f : -1f);

        if (oldInput != activeGrowth && age > 0.2f)
        {
            if (NotifyInputChange != null)
            {
                NotifyInputChange();
            }
        }

        if (alive)
        {
            nutrientFX.SetBool("Active", activeNutrient);
            if (activeGrowth)
            {

                visual.transform.localScale = Vector3.one * (1f + (growthAnimationMod *
                    Mathf.Sin((Time.realtimeSinceStartup + transform.position.z) * growthAnimationSpeed)));
                visual.transform.localScale *= _scaleMod;
            }
        }
        else
        {
            nutrientFX.SetBool("Active", false);
        }
    }

    public void RandomizeInputs(List<KeyCode> availableInputs)
    {
        List<KeyCode> keys = new List<KeyCode>(availableInputs);
        int i = Random.Range(0, keys.Count);
        growthKey = nutrientKey = keys[i];
    }

    public void ClearInputs()
    {
        nutrientKey = KeyCode.None;
        growthKey = KeyCode.None;
        reproduceKey = KeyCode.None;
    }


    public void Reproduce()
    {
        transform.parent.GetComponent<Garden>().ResetPlant();
    }

    public void MakeInert()
    {
        // #todo change animation
        age = 0;
        health = 0;
        growth = 0;
        state = EPlantState.INERT;
        animator.Play("inertBT", 0, 1);
    }

    public void MakeDead()
    {
        if (state != EPlantState.DEAD)
        {
            // #todo change animation
            var fx = GameObject.Instantiate<NewPlantFX>(deadPlantFX, transform.position, Quaternion.identity);
            fx.sprite.sortingOrder = _garden.baseSortingOrder + 11;
            Well.Instance.PlantDeadFX(_garden.progress);
            state = EPlantState.DEAD;
            age *= -1f;
        }
    }

    public void MakeSeed()
    {
        if (state != EPlantState.SEEDLING)
        {
            age = 0;
            health = 0;
            growth = 0;
            state = EPlantState.SEEDLING;
            var fx = GameObject.Instantiate<NewPlantFX>(newPlantFX, transform.position, Quaternion.identity);
            fx.sprite.sortingOrder = _garden.baseSortingOrder + 11;
            RandomizeInputs(Well.Instance.inputPool);
        }
    }

}
