using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainGameController : MonoBehaviour
{
    static private MainGameController _S; 
    static public MainGameController S {
        get {
            if (_S != null) return _S;

            CreateController ();
            return _S;
        }
        private set {_S = value;}
    }

    private TransitionScreen transitionScreen;

    static void CreateController () {
        GameObject controllerObj = new GameObject ();
        controllerObj.name = "MainGameController";
        controllerObj.AddComponent<MainGameController>();
    }

    void Awake () {
        if (_S != null) {
            print ("Main game controller already exists!");
            return;
        }

        _S = this;
        GameObject screenObj = GameObject.FindWithTag ("TransitionScreen");
        transitionScreen = screenObj.GetComponent<TransitionScreen>();
        StartCoroutine (transitionScreen.FadeOut (0.5f));
        DontDestroyOnLoad (gameObject);
    }

    public IEnumerator SwitchScene (string sceneName) {
        yield return transitionScreen.FadeIn (0.5f);
        yield return SceneManager.LoadSceneAsync (sceneName);
        yield return transitionScreen.FadeOut (0.5f);
    }

    public void Exit () {
        Application.Quit ();
    }
}