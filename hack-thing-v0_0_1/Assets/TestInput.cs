using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TestInput : MonoBehaviour
{

    public KeyCode thumb, index, middle, ring, pinky;
    public Text debug;

    // Update is called once per frame
    void Update()
    {
        debug.text = "";
        debug.text += "T " + Input.GetKey(thumb);
        debug.text += " | I " + Input.GetKey(index);
        debug.text += " | M " + Input.GetKey(middle);
        debug.text += " | R " + Input.GetKey(ring);
        debug.text += " | P " + Input.GetKey(pinky);
    }
}
