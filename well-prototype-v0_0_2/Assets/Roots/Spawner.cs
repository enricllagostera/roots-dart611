using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public bool randomizer = false;
    public PoolConfig pool;
    public float treeProbability;
    public float grassProbability;
    public float propProbability;

    void Start()
    {
        // point UP to center
        transform.up = Vector3.zero - transform.position;

        // totally random probabilities for testing
        if (randomizer)
        {
            treeProbability = Random.Range(0.1f, 0.9f);
            grassProbability = Random.Range(0.1f, 0.9f);
            propProbability = Random.Range(0.1f, 0.9f);
        }

        // tree spawning
        float roll = Random.value;
        if (roll <= treeProbability)
        {
            SpawnFrom(pool.trees);
            return;
        }

        // grasses spawning
        roll = Random.value;
        if (roll <= grassProbability)
        {
            SpawnFrom(pool.grass);
            return;
        }

        // prop spawning
        roll = Random.value;
        if (roll <= propProbability)
        {
            SpawnFrom(pool.props);
        }

        // Get rid of slot objects
        Destroy(gameObject);
    }

    void SpawnFrom(List<Transform> prefabs)
    {
        if (prefabs.Count <= 0)
        {
            return;
        }
        var go = Instantiate<Transform>(prefabs[Random.Range(0, prefabs.Count)], transform.position, transform.rotation, transform.parent);
    }
}
