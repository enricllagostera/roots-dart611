using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleFX : MonoBehaviour
{
    ParticleSystem emitter;
    ParticleSystemRenderer psrenderer;

    void Start()
    {
        emitter = GetComponent<ParticleSystem>();
        psrenderer = GetComponent<ParticleSystemRenderer>();
    }

    public void Run(bool value, int sorting)
    {
        psrenderer.sortingOrder = sorting;
        if (value)
        {
            if (!emitter.isEmitting)
            {
                emitter.Play();
            }
        }
        else
        {
            emitter.Stop(false, ParticleSystemStopBehavior.StopEmitting);
        }
    }
}
