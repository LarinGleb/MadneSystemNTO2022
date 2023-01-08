using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TileTypes {
    stated,
    randomized,
    variated,
    animated
}

[AddComponentMenu("Advanced Mesh/Tile System/Tile Controller")]
public class TileController : MonoBehaviour, IGenerationCallbackSender
{
    public MeshTile tile;

    private int defaultLayer;
    private int colliderLayer;

    private IGenerationCallbackListener _callbackListener;

    public IGenerationCallbackListener callbackListener {
        get {return _callbackListener;}
        set {_callbackListener = value;}
    }

    [HideInInspector]
    public TilesetController tlsc;
    private MeshFilter mshf;
    private MeshAnimator msha;
    private MeshCollider mshc;
    private Material mat;

    void Awake () {
        defaultLayer = gameObject.layer;
        colliderLayer = LayerMask.NameToLayer ("CollidableTiles");

        mshf = GetComponent<MeshFilter>();
        msha = GetComponent<MeshAnimator>();
        mshc = GetComponent<MeshCollider>();
        mat = GetComponent<MeshRenderer>().material;
        tlsc = null;
    }

    void Start () {
        if (tlsc == null) {
            tlsc = GetComponentInParent <TilesetEditor> ();
            if (tlsc != null) tlsc.AddTile(this);
        }

        transform.localPosition = tile.position;
        transform.localRotation = tile.rotation;

        RefreshPosition ();
        RefreshMesh ();

        if (tile.spawnPool != null && tlsc.spawnObjects) {
            GameObject obj = tile.spawnPool.GetObject ();
            if (obj != null) {
                GameObject go = Instantiate (obj, transform.position, Quaternion.identity);
                go.transform.SetParent(transform);
            }
        }

        if (callbackListener != null) {
            callbackListener.OnGenerationCallback (this);
            callbackListener = null;
        }
    }

    public void RefreshPosition () {
        tile.position = transform.localPosition;
        tile.rotation = transform.localRotation;
    }

    public void RefreshMesh () {
        if (tile.meshSpritesheet == null) return;

        if (tile.type == TileTypes.animated) {
            msha.enabled = true;
            msha.speed = tile.animationSpeed;
            msha.meshAnimation = tile.GetAnimation();
        } else {
            msha.enabled = false;
        }
        mshf.mesh = tile.GetMesh();

        if (tile.hitbox != null) {
            mshc.enabled = true;
            gameObject.layer = colliderLayer;
            mshc.sharedMesh = tile.meshSpritesheet.meshes[tile.variationY][tile.variationX];
        } else {
            gameObject.layer = defaultLayer;
            mshc.enabled = false;
        }

        mat.mainTexture = tile.texture;
    }
}

[System.Serializable]
public class MeshTile
{
    public TiledMesh meshSpritesheet;
    public TiledMesh hitbox;
    public SpawnPool spawnPool;

    public int variationX = 0;
    public int variationY = 0;

    public TileTypes type;

    public float animationSpeed = 1;

    public Texture texture {
        get {return meshSpritesheet.texture;}
    }

    public Vector3 position;
    public Quaternion rotation;

    public Mesh GetMesh () {
        if (meshSpritesheet == null) return null;
        variationY = Mathf.Clamp(variationY, 0, meshSpritesheet.meshes.Count-1);
        switch (type) {
            case TileTypes.randomized:
                variationY = Random.Range(0, meshSpritesheet.meshes.Count);
                goto case TileTypes.variated;

            case TileTypes.variated:
                variationX = Random.Range(0, meshSpritesheet.meshes[variationY].Count);
                break;
        }
        variationX = Mathf.Clamp(variationX, 0, meshSpritesheet.meshes[variationY].Count-1);
        return meshSpritesheet.meshes[variationY][variationX];
    }

    public MeshAnimation GetAnimation () {
        if (meshSpritesheet == null) return null;
        variationY = Mathf.Clamp(variationY, 0, meshSpritesheet.meshes.Count-1);
        return meshSpritesheet.GetAnimation(variationY);
    }

    public MeshTile copy {
        get {
            MeshTile tl = new MeshTile ();
            tl.meshSpritesheet = meshSpritesheet;
            tl.hitbox = hitbox;
            tl.variationX = variationX;
            tl.variationY = variationY;
            tl.type = type;
            tl.animationSpeed = animationSpeed;

            tl.position = position;
            tl.rotation = rotation;
            return tl;
        }
    }

    public bool Compare (MeshTile tl) {
        return (
            meshSpritesheet == tl.meshSpritesheet
            && hitbox == tl.hitbox
            && type == tl.type
            && variationY == tl.variationY
            && variationX == tl.variationX
            && animationSpeed == tl.animationSpeed
        );
    }
}

public interface IGenerationCallbackListener
{
    void OnGenerationCallback (IGenerationCallbackSender sender);
    List<IGenerationCallbackSender> callbackSenders {get; set;}
}

public interface IGenerationCallbackSender
{
    IGenerationCallbackListener callbackListener {set;}
}