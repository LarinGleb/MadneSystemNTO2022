using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class LocalGameController : MonoBehaviour
{
    public void SwitchScene (string sceneName) {
        StartCoroutine (MainGameController.S.SwitchScene (sceneName));
    }

    public void Exit () {
        MainGameController.S.Exit ();
    }
}
