using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundLayer : MonoBehaviour
{
    public SpriteRenderer visual;

    void Awake()
    {
        visual = GetComponentInChildren<SpriteRenderer>();
    }
}
