using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fog : MonoBehaviour
{
    public Gradient colors;
    private SpriteRenderer _sprite;

    void Start()
    {
        _sprite = GetComponent<SpriteRenderer>();
    }

    public void UpdateColor(float curvePoint, int sorting)
    {
        _sprite.color = colors.Evaluate(curvePoint);
        _sprite.sortingOrder = sorting;
    }
}
