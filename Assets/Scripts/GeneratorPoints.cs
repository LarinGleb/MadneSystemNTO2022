using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.AI.Navigation;
using System;
using Random = System.Random;
public class GeneratorPoints : MonoBehaviour, IGenerationCallbackListener
{
    [Header("Generate points for island")]
    [SerializeField] private int _CountRooms;
    [Range(200, 400)]
    [SerializeField] private int _MinX;
    [Range(400, 1000)]
    [SerializeField] private int _MaxX;

    [Range(200, 400)]
    [SerializeField] private int _MinY;
    [Range(400, 1000)]
    [SerializeField] private int _MaxY;

    public float minimalDistance;
    

    [Header("Need variables for generator")]
    [SerializeField] private int _Seed;
    [SerializeField] GameObject _PrefabPoint;
    [SerializeField] bool _DebugMode = false;

    [Header("Generate size for island")]
    [Range(20, 40)]
    [SerializeField] private int _MinSizeIsland;
    [Range(40, 80)]
    [SerializeField] private int _MaxSizeIsland;

    [SerializeField] private GameObject _islandPrefab;
    [SerializeField] private GameObject _game;
    [SerializeField] private GameObject _brigd;

    public List<Tileset> islands;
    public GameObject tilesetPrefab;
    public GameObject bridgePrefab;

    private List<GameObject> _points = new List<GameObject>();
    private List<IslandPoint> islandPoints;
    
    private Random _rndClass;
    private MovePoints _movePointsScript;

    private float _waiTime;

    private NavMeshSurface nms;

    private List<IGenerationCallbackSender> _callbackSenders;
    public List<IGenerationCallbackSender> callbackSenders {
        get {return _callbackSenders;}
        set {_callbackSenders = value;}
    }

    void Awake () {
        islandPoints = new List<IslandPoint> ();
        callbackSenders = new List<IGenerationCallbackSender> ();
        nms = GetComponent<NavMeshSurface> ();
    }

    private void Start() {
        _rndClass = new Random(_Seed);
        UnityEngine.Random.InitState(_Seed);
        _movePointsScript = GetComponent<MovePoints>();
        GeneratePoints();
    }
    int max(int x, int y) {
        return (x >= y) ? x: y;
    }
    int min(int x, int y) {
        return (x >= y) ? y: x;
    }
    public void StartMovement(float min_distance, Random rnd) {
        IslandPoint _pointStatic;
        IslandPoint _pointDinamic;

        float _distance = 0f;

        float _radiusStatic;
        float _radiusDinamic;   

        Vector3 _direction; 
        bool itersects = false;
        while(true) {
            for (int i = 0; i < islandPoints.Count; i ++) {
                _pointStatic = islandPoints[i];

                _radiusStatic = _pointStatic.radius;
                itersects = false;

                for (int j = 0; j < islandPoints.Count; j++) {
                    if (i == j) {
                        continue;
                    }
                    _pointDinamic = islandPoints[j];
                    _radiusDinamic = _pointDinamic.radius;

                    _distance = Vector3.Distance(_pointStatic.position, _pointDinamic.position);

                    itersects = (_distance < (min_distance + _radiusDinamic+ _radiusStatic));
                    _direction = new Vector3(rnd.Next(-1, 2), 0, rnd.Next(-1, 2)); 
                    if (_direction.x == 0 && _direction.z == 0) {
                        _direction.x = 1;
                    }
                    
                    while (itersects)
                    {
                        itersects = (_distance < (min_distance + _radiusDinamic+ _radiusStatic));
                        _pointDinamic.position += _direction*20;
                        _distance = Vector3.Distance(_pointStatic.position, _pointDinamic.position);
                    }
                }
            }
           
            if (!itersects) {
                for (int i = 0; i < islandPoints.Count; i ++) {
                    _pointStatic = islandPoints[i];

                    for (int j = 0; j < islandPoints.Count; j++) {
                        if (i == j) continue;
        
                        _pointDinamic = islandPoints[j];
                        _distance = Vector3.Distance(_pointStatic.position, _pointDinamic.position);
                        itersects = (_distance < (min_distance + _pointDinamic.radius+ _pointStatic.radius));
                        if (itersects) {
                            islandPoints.RemoveAt(j);
                            if (i >= j) i--;
                            j--;
                        }
                    }
                }
                break;
            }
        }
    }
    public void GeneratePoints() {
        IslandPoint point;
        for (int i = 0; i < _CountRooms; i++) {
            point = new IslandPoint ();
            point.position = new Vector3(_rndClass.Next(_MinX, _MaxX+1), 0, _rndClass.Next(_MinY, _MaxY+1));
            point.islandIndex = UnityEngine.Random.Range (0, islands.Count);
            point.radius = islands[point.islandIndex].bounds.extents.x;

            islandPoints.Add(point);

        }
        StartMovement(minimalDistance, _rndClass);
        
        GenerateIsland();
    }

    public void GenerateIsland() {
        
        foreach (IslandPoint point in islandPoints)
        {
            GameObject island = Instantiate(tilesetPrefab, point.position, Quaternion.identity);
            island.transform.SetParent (transform);
            //island.transform.localScale  = new Vector3(srciptIsland.SizeIsland*5, island.transform.localScale.y, srciptIsland.SizeIsland*5);

            TilesetController tlsc = island.GetComponent<TilesetController>();
            tlsc.tileset = islands[point.islandIndex];
            tlsc.callbackListener = this;

            float min_distance = 10000000000;
            IslandPoint to = new IslandPoint();
            foreach (IslandPoint point_to in islandPoints)   {
                if (point_to == point) continue;
                if (point_to.connectedTo == point) continue;

                float distance = Vector3.Distance(point_to.position, point.position) - point.radius - point_to.radius;

                if (distance < min_distance) {
                    to = point_to;
                    min_distance = distance;
                }   
            }  

            point.connectedTo = to;   

            GameObject bridge = Instantiate(bridgePrefab, Vector3.Lerp(point.position, to.position, 0.5f), Quaternion.identity);
            BridgeGenerator brg = bridge.GetComponent<BridgeGenerator> ();
            brg.targetSize = min_distance;
            brg.callbackListener = this;
            callbackSenders.Add (brg);


            bridge.transform.SetParent (transform);
            //bridge.transform.localScale = new Vector3(min_distance, 0.1f, 1f);
            bridge.transform.localRotation = Quaternion.LookRotation((point.position - to.position).normalized);
            bridge.transform.Rotate(0, 90, 0);


        }
        _game.transform.position = islandPoints[0].position + new Vector3(0, 4f, 0);
    
    }

    public void OnGenerationCallback (IGenerationCallbackSender sender) {
        callbackSenders.Remove (sender);
        if (callbackSenders.Count == 0) nms.BuildNavMesh ();
    }
}

[System.Serializable]
public class IslandPoint
{
    public float radius;
    public Vector3 position;

    public int islandIndex;
    public IslandPoint connectedTo;
}