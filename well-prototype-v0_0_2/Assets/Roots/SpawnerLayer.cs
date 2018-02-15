using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnerLayer : MonoBehaviour
{
    public int plantCount;
    public bool randomizer = false;
    public PoolConfig pool;
    public float treeProbability;
    public float grassProbability;
    public float propProbability;
    public float spawnRadius;

    void Start()
    {
        for (int i = 0; i < plantCount; i++)
        {
            var circlePos = Random.insideUnitCircle.normalized * spawnRadius;
            var up = Vector2.zero - circlePos;

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
                SpawnFrom(pool.trees, circlePos, up);
                continue;
            }

            // grasses spawning
            roll = Random.value;
            if (roll <= grassProbability)
            {
                SpawnFrom(pool.grass, circlePos, up);
                continue;
            }

            // prop spawning
            roll = Random.value;
            if (roll <= propProbability)
            {
                SpawnFrom(pool.props, circlePos, up);
            }
        }
    }

    void SpawnFrom(List<Transform> prefabs, Vector3 pos, Vector2 up)
    {
        if (prefabs.Count <= 0)
        {
            return;
        }
        var go = Instantiate<Transform>(prefabs[Random.Range(0, prefabs.Count)], pos, transform.rotation, transform);
        go.transform.up = up;
    }
}
