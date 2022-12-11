using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
[CreateAssetMenu(fileName = "SpawnPool_0", menuName = "SpawnPool", order = 51)]
public class SpawnPool : ScriptableObject
{
    public List<GameObject> objects;

    public GameObject GetObject () {
        if (objects.Count > 0) {
            int objIndex = Random.Range(0, objects.Count);
            return objects[objIndex];
        }
        return null;
    }
}
