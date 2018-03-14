using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ground : MonoBehaviour
{
    public Sprite[] allSprites;
    private SpriteRenderer _sprite;


    void Start()
    {
        _sprite = GetComponent<SpriteRenderer>();
        _sprite.sprite = allSprites[Random.Range(0, allSprites.Length)];
    }
}
