using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Leaf : MonoBehaviour
{
    public LeafStatus status;
    public Color youngLeaf;
    public Color matureLeaf;
    public Color witheringLeaf;
    private SpriteRenderer _visual;
    private Color _targetColor;
    public float colorFadeFactor;
    public Sprite[] statusSprite;

    void Start()
    {
        status = LeafStatus.DORMANT;
        _visual = GetComponentInChildren<SpriteRenderer>();
        _targetColor = Color.clear;
        _visual.color = _targetColor;
    }

    void Update()
    {
        switch (status)
        {
            case LeafStatus.DORMANT:
                _targetColor = Color.white;
                _visual.sprite = null;
                break;
            case LeafStatus.YOUNG:
                _targetColor = Color.white; ;
                _visual.sprite = statusSprite[0];
                break;
            case LeafStatus.MATURE:
                _targetColor = Color.white; ;
                _visual.sprite = statusSprite[1];
                break;
            case LeafStatus.WITHERING:
                _targetColor = Color.white; ;
                _visual.sprite = statusSprite[2];
                break;
        }
        _visual.color = Color.Lerp(_visual.color, _targetColor, Time.deltaTime * colorFadeFactor);
    }

    public void CreateLeaf()
    {
        status = LeafStatus.YOUNG;
    }

    public void MatureLeaf()
    {
        status = LeafStatus.MATURE;
    }

    public void WitherLeaf()
    {
        status = LeafStatus.WITHERING;
    }

    public void KillLeaf()
    {
        status = LeafStatus.DORMANT;
    }
}