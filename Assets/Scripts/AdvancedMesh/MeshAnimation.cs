using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "MeshAnimation_0", menuName = "Advanced Mesh/Mesh Animation", order = 1)]
public class MeshAnimation : ScriptableObject
{
    public List<Mesh> frames = new List<Mesh>();
    public float animationFPS = 12;

    public int framesCount {
        get {return frames.Count;}
    }
}
