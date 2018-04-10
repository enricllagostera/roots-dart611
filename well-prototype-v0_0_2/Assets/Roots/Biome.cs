using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Biome
{
    public float humidity; // 0..1
    public int plantCount; // 0..n
    public Garden garden; // for hooking up feedback and GOs

    public Biome()
    {

    }
}
