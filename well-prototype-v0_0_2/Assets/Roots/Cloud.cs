using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cloud : MonoBehaviour
{
    public float speed;
    public float lifetime;
    public float fadeInDuration;
    public float fadeOutDuration;
    public float radius;
    public float angle;
    public float opacityMax;
    public Sprite[] allSprites;
    private SpriteRenderer _sprite;
    private float _timer;
    private CloudsFX _fx;

    void Start()
    {
        _timer = lifetime;
        _sprite = GetComponent<SpriteRenderer>();
        _sprite.sprite = allSprites[UnityEngine.Random.Range(0, allSprites.Length)];
        Destroy(gameObject, lifetime);
        StartCoroutine(Fade(0f, opacityMax, fadeInDuration));
        _fx = transform.parent.GetComponent<CloudsFX>();
    }

    void Update()
    {
        _timer -= Time.deltaTime;
        if (_timer <= fadeOutDuration)
        {
            _timer = 1000f;
            StartCoroutine(Fade(opacityMax, 0f, fadeOutDuration));
        }
        angle += speed * Time.deltaTime;
        var x = radius * Mathf.Cos(angle * Mathf.Deg2Rad);
        var y = radius * Mathf.Sin(angle * Mathf.Deg2Rad);
        var newPosition = transform.localPosition;
        newPosition.x = x;
        newPosition.y = y;
        transform.localPosition = newPosition;
        transform.up = transform.parent.position - transform.position;
        _sprite.sortingOrder = _fx.sorting;
    }

    private IEnumerator Fade(float startValue, float targetValue, float duration)
    {
        float timer = 0f;
        while (timer <= duration)
        {
            Color color = _sprite.color;
            color.a = Mathf.Lerp(startValue, targetValue, timer / duration);
            _sprite.color = color;
            yield return null;
            timer += Time.deltaTime;
        }
    }
}