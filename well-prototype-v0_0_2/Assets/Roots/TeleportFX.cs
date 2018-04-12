using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleportFX : MonoBehaviour
{
    public ParticleSystemRenderer render;

    public void SetSortingOrder(int newValue)
    {
        render.sortingOrder = newValue;
    }
}
