using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenFlashFX : MonoBehaviour
{

    public SpriteRenderer sprite;
    public float duration;
    private float _timer;
    public AnimationCurve opacity;

    public Vector2 hueRange;
    public Vector2 saturationRange;
    public Vector2 valueRange;

    void Start()
    {
        _timer = 0;
        Color temp = Random.ColorHSV(hueRange.x, hueRange.y, saturationRange.x, saturationRange.y, valueRange.x, valueRange.y);
        temp.a = 0f;
        sprite.color = temp;
    }

    void Update()
    {
        _timer += Time.deltaTime;
        Color temp = sprite.color;
        temp.a = opacity.Evaluate(_timer / duration);
        sprite.color = temp;
        if (_timer >= duration)
        {
            Destroy(gameObject);
        }
    }
}
