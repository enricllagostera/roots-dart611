using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Plant : MonoBehaviour
{
    public AnimationCurve witheringCurve;
    public float humidity;
    public float health;
    public float healthDecayRate;

    public float leafBlossomLevel;
    public float leafWitherLevel;
    public float lastLeafTime;
    public float leafInterval;
    public Leaf[] leaves;
    public Leaf leafPrefab;

    public float heightMax;
    public float lifetimeMax;

    public float timer;
    public bool dead;

    public List<NutrientSource> nutrients;
    public List<KeyCode> nutrientsKeycodes;


    void Start()
    {
        Reset();
    }


    void Update()
    {
        timer += Time.deltaTime;
        // update plant position
        var pos = transform.position;
        float healthHeight = Mathf.Clamp01(health * 2f);
        pos.y = Mathf.Lerp(pos.y, heightMax * healthHeight, Time.deltaTime);
        transform.position = pos;

        if (dead)
        {
            return;
        }

        float nutrientSum = 0f;
        for (int i = 0; i < nutrients.Count; i++)
        {
            if (Input.GetKey(nutrientsKeycodes[i]))
            {
                nutrientSum += nutrients[i].Drain() / nutrients.Count;
            }
        }
        health += nutrientSum * Time.deltaTime;

        // decrease health based on withering curve over lifetime
        health -= (1f - witheringCurve.Evaluate(timer / lifetimeMax)) * healthDecayRate * Time.deltaTime;
        if (timer >= lifetimeMax)
        {
            health = 0f;
        }
        health = Mathf.Clamp(health, 0f, 1f);

        if (health >= leafBlossomLevel)
        {
            if ((Time.realtimeSinceStartup - lastLeafTime) >= leafInterval)
            {
                BlossomLeaf();
            }
        }
        else if (health <= leafWitherLevel)
        {
            if ((Time.realtimeSinceStartup - lastLeafTime) >= leafInterval)
            {
                WitherLeaf();
            }
        }

        if (health <= 0 && !dead)
        {
            dead = true;
            StartCoroutine(DelayReset());
        }
    }


    IEnumerator DelayReset()
    {
        yield return new WaitForSeconds(Random.Range(4f, 10f));
        Reset();
    }


    void Reset()
    {
        dead = false;
        health = 0.1f;
        humidity = 1f;
        timer = 0f;
        lastLeafTime = Time.realtimeSinceStartup;
        foreach (var slot in leaves)
        {
            slot.KillLeaf();
        }
    }


    void BlossomLeaf()
    {
        lastLeafTime = Time.realtimeSinceStartup;
        bool addedLeaf = false;
        foreach (var slot in leaves)
        {
            var canBlossom = (slot.status != LeafStatus.MATURE);
            if (canBlossom && !addedLeaf)
            {
                addedLeaf = true;
                if (slot.status == LeafStatus.DORMANT)
                {
                    slot.CreateLeaf();
                }
                else
                {
                    slot.MatureLeaf();
                }
            }
        }
    }

    void WitherLeaf()
    {
        lastLeafTime = Time.realtimeSinceStartup;
        bool changed = false;
        foreach (var slot in leaves)
        {
            if (slot.status != LeafStatus.DORMANT && !changed)
            {
                changed = true;
                if (slot.status == LeafStatus.WITHERING)
                {
                    slot.KillLeaf();
                }
                else
                {
                    slot.WitherLeaf();
                }
            }
        }
    }
}
