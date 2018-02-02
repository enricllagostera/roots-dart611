using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RainController : MonoBehaviour
{
    public KeyCode key;
    private ParticleSystem particles;

    void Start()
    {
        particles = GetComponent<ParticleSystem>();
    }

    void Update()
    {
        if (Input.GetKey(key))
        {
            if (!particles.isEmitting)
            {
                particles.Play();
            }
        }
        else
        {
            particles.Stop();
        }

    }
}
