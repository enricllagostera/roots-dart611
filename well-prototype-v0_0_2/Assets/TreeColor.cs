using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreeColor : MonoBehaviour
{
    private GroundLayer ground;
    private SpriteRenderer[] _parts;
    public bool bypassColor;

    // Use this for initialization
    void Start()
    {
        ground = transform.parent.GetComponent<GroundLayer>();
        _parts = GetComponentsInChildren<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        foreach (var part in _parts)
        {
            if (!bypassColor)
            {
                part.color = ground.visual.color;
            }
            part.sortingOrder = ground.visual.sortingOrder + 1;
        }
    }
}
