using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloudsFX : MonoBehaviour
{
    public float spawnRadiusMin;
    public float spawnRadiusMax;
    public float speedMin;
    public float speedMax;
    public float spawnFrequencyMin;
    public float spawnFrequencyMax;
    public float durationMin;
    public float durationMax;
    public float probability;
    public float fadeInDuration;
    public float fadeOutDuration;
    public Cloud cloudPrefab;
    public float opacityMax;
    public int sorting;
    public float humidity;

    void Start()
    {
        StartCoroutine(GenerateClouds());
    }

    IEnumerator GenerateClouds()
    {
        do
        {
            float idle = Random.Range(spawnFrequencyMin, spawnFrequencyMax);
            yield return new WaitForSeconds(idle);
            float roll = Random.Range(0f, 1f);
            if (roll <= probability * humidity)
            {

                var cloud = Instantiate(cloudPrefab, transform.position, transform.rotation, transform);
                cloud.angle = Random.Range(0f, 360f);
                cloud.radius = Random.Range(spawnRadiusMin, spawnRadiusMax);
                cloud.speed = Random.Range(speedMin, speedMax);
                cloud.lifetime = Random.Range(durationMin, durationMax);
                cloud.fadeInDuration = fadeInDuration;
                cloud.fadeOutDuration = fadeOutDuration;
                cloud.opacityMax = opacityMax;
            }
        } while (enabled);
    }



    void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, spawnRadiusMin);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, spawnRadiusMax);
    }


    public void SetHumidity(float newHumidity)
    {
        humidity = newHumidity;
    }
}
