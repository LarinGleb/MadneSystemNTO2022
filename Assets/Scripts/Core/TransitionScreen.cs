using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransitionScreen : MonoBehaviour
{
    private CanvasGroup cg;

    void Awake () {
        cg = GetComponent<CanvasGroup>();
        DontDestroyOnLoad (gameObject);
    }

    public IEnumerator FadeIn (float duration) {
        StopCoroutine ("FadeOut");
        float startTime = Time.time;

        while (Time.time < startTime + duration) {
            float u = (Time.time - startTime) / duration;
            SetOpacity (u);
            yield return null;
        }
        SetOpacity (1);
    }

    public IEnumerator FadeOut (float duration) {
        print ("out");
        StopCoroutine ("FadeIn");
        float startTime = Time.time;

        while (Time.time < startTime + duration) {
            float u = (Time.time - startTime) / duration;
            SetOpacity (1-u);
            yield return null;
        }
        SetOpacity (0);
    }

    void SetOpacity (float alpha) {
        cg.alpha = Mathf.Clamp01 (alpha);
        cg.blocksRaycasts = alpha > 0;
    }
}
