using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BridgeGenerator : MonoBehaviour, IGenerationCallbackListener, IGenerationCallbackSender
{
    public float targetSize;
    public float overlapDistance;

    public GameObject tilesetPrefab;
    public List<Tileset> bridgeTilesets;

    private float size;

    private IGenerationCallbackListener _callbackListener;
    public IGenerationCallbackListener callbackListener {
        get {return _callbackListener;}
        set {_callbackListener = value;}
    }

    private List<IGenerationCallbackSender> _callbackSenders;
    public List<IGenerationCallbackSender> callbackSenders {
        get {return _callbackSenders;}
        set {_callbackSenders = value;}
    }

    void Awake () {
        callbackSenders = new List<IGenerationCallbackSender> ();
    }

    void Start () {
        size = targetSize;
        List<Tileset> brigde = PickTilesets ();

        float offset = -size / 2;
        for (int i = 0; i < brigde.Count; i++) {
            Tileset tls = brigde[i];

            GameObject tilesetObj = Instantiate (tilesetPrefab);
        
            TilesetController tlsc = tilesetObj.GetComponent<TilesetController> ();
            tlsc.tileset = tls;
            tlsc.callbackListener = this;

            tilesetObj.transform.SetParent (transform);
            tilesetObj.name = "BrigdeTileset_" + i;

            float tlsSize = tls.bounds.extents.x * 2 - overlapDistance;

            tilesetObj.transform.localPosition = new Vector3 (offset + tlsSize / 2 - tls.bounds.center.x, 0, 0);
            tilesetObj.transform.localRotation = Quaternion.identity;

            offset += tlsSize;
        }
    }

    List<Tileset> PickTilesets () {
        List<Tileset> res = new List<Tileset> ();
        float currentSize = 0;

        Tileset tls;
        while (currentSize < size) {
            tls = bridgeTilesets[Random.Range(0, bridgeTilesets.Count)];

            currentSize += tls.bounds.extents.x * 2 - overlapDistance;
            res.Add (tls);
        }

        for (int i = 0; i < res.Count; i++) {
            tls = res[i];

            Tileset bestTls = tls;
            float best = currentSize - size;

            for (int j = 0; j < bridgeTilesets.Count; j++) {
                Tileset newTls = bridgeTilesets[j];

                float difference = (newTls.bounds.extents.x - bestTls.bounds.extents.x) * 2 - overlapDistance;

                if (Mathf.Abs (best) > Mathf.Abs(best + difference)) {
                    bestTls = newTls;
                    best += difference;
                }
            }

            res[i] = bestTls;
            currentSize = best + size;
        }
        size = currentSize;

        return res;
    }

    public void OnGenerationCallback (IGenerationCallbackSender sender) {
        callbackSenders.Remove (sender);
        if (callbackSenders.Count == 0) {
            callbackListener.OnGenerationCallback (this);
            callbackListener = null;
        }
    }
}
