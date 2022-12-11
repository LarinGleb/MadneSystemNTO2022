using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Tileset_0", menuName = "Advanced Mesh/Tile System/Tileset", order = 4)]
public class Tileset : ScriptableObject
{
    public List<MeshTile> tiles = new List<MeshTile>();
    public Bounds bounds;

    public void FindBounds () {
        bounds = new Bounds ();
        foreach (MeshTile tile in tiles) {
            if (tile.meshSpritesheet == null) continue;
            Bounds tileBounds = tile.copy.GetMesh().bounds;
            Bounds alignedBounds = new Bounds ();
            alignedBounds.center = tile.rotation * tileBounds.center + tile.position;
            alignedBounds.size = tile.rotation * tileBounds.size;

            bounds.Encapsulate (alignedBounds);
        }
    }
}
