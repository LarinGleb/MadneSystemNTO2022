using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[AddComponentMenu("Advanced Mesh/Tile System/Tileset Controller")]
public class TilesetController : MonoBehaviour, IGenerationCallbackSender
{
    public Tileset tileset;
    public GameObject tilePrefab;

    public bool spawnObjects;

    protected IGenerationCallbackListener _callbackListener;
    public IGenerationCallbackListener callbackListener {
        get {return _callbackListener;}
        set {_callbackListener = value;}
    }

    protected List<TileController> tileControllers;
    
    protected virtual void Start () {
        tileControllers = new List<TileController> ();
        for (int i = 0; i < tileset.tiles.Count; i++) {
            MeshTile tile = tileset.tiles[i];
            GameObject tileObj = Instantiate (tilePrefab);
            TileController tc = tileObj.GetComponent<TileController>();
            tileControllers.Add (tc);
            tc.tile = tile;
            tc.tlsc = this;
            tileObj.transform.SetParent (transform);

            if (callbackListener != null) {
                tc.callbackListener = callbackListener;
                callbackListener.callbackSenders.Add (tc);
            }
        }
    }

    public virtual void AddTile (TileController tc) {
        tileControllers.Add (tc);
        tileset.tiles.Add (tc.tile);
    }
}
