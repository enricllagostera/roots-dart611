using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flower : MonoBehaviour
{
    public FlowerStatus status;
    public float health;
    public float growthTime;

    public Color blossomColor;
    public Color matureColor;
    public Color witheringColor;
    private SpriteRenderer _visual;
    private Color _targetColor;
    public float colorFadeFactor;

    public Sprite[] spriteStatus;

    void Start()
    {
        _visual = GetComponentInChildren<SpriteRenderer>();
        health = 0f;
        _targetColor = Color.clear;
        _visual.color = _targetColor;
    }

    void Update()
    {
        switch (status)
        {
            case FlowerStatus.DORMANT:
                _targetColor = Color.white;
                _visual.sprite = null;
                break;
            case FlowerStatus.BLOSSOM:
                _targetColor = Color.white;
                _visual.sprite = spriteStatus[0];
                break;
            case FlowerStatus.MATURE:
                Garden.Instance.UpdateGarden();
                _targetColor = Color.white;
                _visual.sprite = spriteStatus[1];
                break;
            case FlowerStatus.WITHERING:
                _targetColor = Color.white;
                _visual.sprite = spriteStatus[2];
                break;
        }

        _visual.color = Color.Lerp(_visual.color, _targetColor, Time.deltaTime * colorFadeFactor);
    }


    public void CalculateStatus(float mod)
    {
        health += mod * Time.deltaTime * growthTime;
        health = Mathf.Clamp01(health);
        switch (status)
        {
            case FlowerStatus.DORMANT:
                if (health > 0.25f)
                {
                    status = FlowerStatus.BLOSSOM;
                }
                break;
            case FlowerStatus.BLOSSOM:
                if (health > 0.7f)
                {
                    status = FlowerStatus.MATURE;
                }
                if (health < 0.25f)
                {
                    status = FlowerStatus.WITHERING;
                }

                break;
            case FlowerStatus.MATURE:
                if (health <= 0.7f)
                {
                    status = FlowerStatus.WITHERING;
                }
                break;
            case FlowerStatus.WITHERING:
                if (health <= 0.05f)
                {
                    status = FlowerStatus.DORMANT;
                }
                if (health > 0.7f)
                {
                    status = FlowerStatus.MATURE;
                }
                break;
        }
    }
}