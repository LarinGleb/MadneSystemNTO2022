using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[AddComponentMenu("Advanced Mesh/Tile System/Tileset Editor")]
public class TilesetEditor : TilesetController
{
    
    protected List<MeshTile> prevTiles;

    protected override void Start () {
        base.Start ();
        prevTiles = new List<MeshTile>();

        foreach (MeshTile tile in tileset.tiles) {
            prevTiles.Add(tile.copy);
        }
    }

    void Update () {
        for (int i = 0; i < tileControllers.Count; i++) {
            if (tileControllers[i] == null) {
                tileset.tiles.RemoveAt(i);
                prevTiles.RemoveAt(i);
                tileControllers.RemoveAt(i);
                i--;
            }
            tileControllers[i].RefreshPosition ();
            MeshTile tile = tileControllers[i].tile;
            if (!tile.Compare(prevTiles[i])) {
                tileControllers[i].RefreshMesh ();
                prevTiles[i] = tile.copy;
            }
        }

        tileset.FindBounds ();
    }

    override public void AddTile (TileController tc) {
        base.AddTile (tc);
        prevTiles.Add (tc.tile.copy);
    }
}
