using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaindropFX : MonoBehaviour
{
    public ParticleSystemRenderer render;
    Cloud cloud;

    void Start()
    {
        cloud = transform.parent.GetComponent<Cloud>();
    }

    void Update()
    {
        render.sortingOrder = cloud.sprite.sortingOrder - 1;
        // render.color = cloud.sprite.color;
    }
}