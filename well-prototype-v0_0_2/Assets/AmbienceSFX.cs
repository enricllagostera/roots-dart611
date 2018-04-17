using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmbienceSFX : MonoBehaviour
{

    public AudioSource audioSource;
    [Range(0f, 1f)]
    public float maxVolume;

    void Update()
    {

    }

    public void SetVolume(float factor = 0f)
    {
        audioSource.volume = Mathf.Clamp01(factor * maxVolume);
    }
}
