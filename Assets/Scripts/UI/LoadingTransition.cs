using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class LoadingTransition : MonoBehaviour
{
    public float transitionDuration;
    public string gameSceneName;
    public TextMeshProUGUI loadingText;
    public List<Button> buttons;

    private Image img;
    private float transitionEnd;

    private bool isTransition = false;
    private bool isMainMenu = true;

    private float a1 = 1;
    private float a2 = 0; 

    void Awake () {
        img = GetComponent<Image>();
        transitionEnd = Time.time + transitionDuration;
    }

    void Update () {
        float u = (transitionEnd - Time.time) / transitionDuration;
        Color c;
        c = img.color;
        c.a = Mathf.Lerp (a2, a1, u);
        img.color = c;

        c = loadingText.color;
        c.a = Mathf.Lerp (a2, a1, u);
        loadingText.color = c;

        gameObject.SetActive(u < 1);

        if (!isTransition) return;
        if (u < 0) {
            if (isMainMenu) {
                isMainMenu = false;
                SceneManager.LoadScene (gameSceneName);
            }
        }
    }

    public void LoadGame () {
        gameObject.SetActive (true);
        isTransition = true;
        transitionEnd = Time.time + transitionDuration;
        a1 = 0;
        a2 = 1;
        foreach (Button b in buttons) {
            b.interactable = false;
        }
    }

    public void Exit () {
        Application.Quit ();
    }
}
