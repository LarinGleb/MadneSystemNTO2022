using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LoadingText : MonoBehaviour
{
    public float frequency = 2;
    public int maxDots = 3;
    public string defaultText = "Загрузка";
    
    private TMP_Text text;

    void Awake () {
        text = GetComponent<TMP_Text>();
    }

    void Update () {
        int dotsCount = Mathf.FloorToInt(Time.time * frequency) % (maxDots + 1);

        text.text = defaultText + new string ('.', dotsCount);
    }
}
