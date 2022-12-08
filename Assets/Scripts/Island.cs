using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Island : MonoBehaviour
{
    [SerializeField] int _size;
    public int SizeIsland {
        set {_size = value;}
        get {return _size;}
    }

    [SerializeField] DebugLineRender _scriptDebug;
    private void Start() {
        _scriptDebug = gameObject.GetComponent<DebugLineRender>();
    }

    public void DebugStart() {
        //Debug.Log(_size);
        _scriptDebug.RadiusIsland = SizeIsland;
        _scriptDebug.StartRender();
    }

    public void DisableDebug() {
        _scriptDebug.enabled = false;
    }
}
