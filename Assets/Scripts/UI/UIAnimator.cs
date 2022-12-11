using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIAnimator : MonoBehaviour
{
    public List<Sprite> frames;
    public float animationFps;

    private Image img;
    private int lastFrame = 0;
    void Awake () {
        img = GetComponent<Image>();
    }

    void Update () {
        int frame = Mathf.FloorToInt(Time.time * animationFps % frames.Count);

        if (lastFrame != frame) {
            img.sprite = frames[frame];
            lastFrame = frame;
        }
    }
}
