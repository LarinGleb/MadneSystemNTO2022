using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HPBar : MonoBehaviour
{
    private float originalWidth;
    private RectTransform rt;

    void Awake () {
        rt = GetComponent<RectTransform> ();
        originalWidth = rt.offsetMax.x - rt.offsetMin.x;
    }

    public void SetProgress (float u) {
        Vector2 offset = new Vector2 (originalWidth * u + rt.offsetMin.x, rt.offsetMax.y);
        rt.offsetMax = offset;
    }
}
