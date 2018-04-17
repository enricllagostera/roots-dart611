using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomSampleSFX : MonoBehaviour
{
    public AudioSource audioSource;
    public List<AudioClip> samples;
    public int lastPlayed;
    public bool useVolumeCurve = false;
    public AnimationCurve volumeCurve;

    public void Play(float value = 0f)
    {
        int i = lastPlayed;
        do
        {
            i = Random.Range(0, samples.Count);
        } while (i == lastPlayed);
        audioSource.PlayOneShot(samples[i], value);
        lastPlayed = i;
    }

    public void Begin(float value = 0f)
    {
        audioSource.Stop();
        int i = lastPlayed;
        do
        {
            i = Random.Range(0, samples.Count);
        } while (i == lastPlayed);
        audioSource.clip = samples[i];
        audioSource.Play();
        lastPlayed = i;
    }


    public void Stop()
    {
        audioSource.Stop();
    }
}
