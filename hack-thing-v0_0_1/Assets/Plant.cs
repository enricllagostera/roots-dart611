using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Plant : MonoBehaviour
{
    public AnimationCurve witheringCurve;
    public float humidity;
    public float health;
    public float healthDecayRate;

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
        pos.y = Mathf.Lerp(pos.y, heightMax * health, Time.deltaTime);
        transform.position = pos;

        if (dead)
        {
            return;
        }
        else
        {
            float nutrientSum = 0f;
            for (int i = 0; i < nutrients.Count; i++)
            {
                if (Input.GetKey(nutrientsKeycodes[i]))
                {
                    nutrientSum += nutrients[i].Drain() / nutrients.Count;
                }
            }
            health += nutrientSum * Time.deltaTime;

        }

        // decrease healt based on withering over lifetime
        health -= (1f - witheringCurve.Evaluate(timer / lifetimeMax)) * healthDecayRate * Time.deltaTime;
        if (timer >= lifetimeMax)
        {
            health = 0f;
        }
        health = Mathf.Clamp(health, 0f, 1f);

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
    }

    float HealthDecay()
    {
        return health;
    }
}
