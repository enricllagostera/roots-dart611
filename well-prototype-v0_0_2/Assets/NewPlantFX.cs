using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewPlantFX : MonoBehaviour
{
    public SpriteRenderer sprite;

    // Update is called once per frame
    public void CleanUp()
    {
        Destroy(gameObject);
    }
}
