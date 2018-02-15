using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZoomLayer : MonoBehaviour
{

    public float scalingSpeed;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        var scale = transform.localScale;
        scale *= 1f + (scalingSpeed * Time.deltaTime);
        transform.localScale = scale;
    }
}
