using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Unity.AI.Navigation;

public class NavMeshTest : MonoBehaviour, IGenerationCallbackListener
{
    public bool bake = false;

    private List<IGenerationCallbackSender> _callbackSenders;
    public List<IGenerationCallbackSender> callbackSenders {
        get {return _callbackSenders;}
        set {_callbackSenders = value;}
    }

    private NavMeshSurface surface;

    void Awake () {
        callbackSenders = new List<IGenerationCallbackSender>();
        surface = GetComponent<NavMeshSurface>();
        foreach (Transform child in transform) {
            IGenerationCallbackSender sender = child.GetComponent<IGenerationCallbackSender> ();
            sender.callbackListener = this;
        }
    }

    void Update() {
        if (bake) {
            bake = false;
            surface.BuildNavMesh ();
        }
    }

    public void OnGenerationCallback (IGenerationCallbackSender sender) {
        callbackSenders.Remove (sender);
        if (callbackSenders.Count == 0) surface.BuildNavMesh ();
    }
}
