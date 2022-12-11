using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[CreateAssetMenu(fileName = "Baked_Nav_Mesh", menuName = "BakedNav Mesh")]
public class BakedNavMesh : ScriptableObject
{
    public NavMeshData navMeshData;
}
