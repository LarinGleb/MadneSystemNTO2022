using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BackgroundPulsing : MonoBehaviour
{
    public Color c1;
    public Color c2;

    public float frequency;

    private Image img;

    void Awake () {
        img = GetComponent<Image>();
    }

    void Update () {
        float u = Mathf.Sin(Time.time * frequency * Mathf.PI) + 0.5f;
        img.color = Color.Lerp (c1, c2, u);
    }
}
