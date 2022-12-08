using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Random = System.Random;
public class GeneratorPoints : MonoBehaviour
{
    [Header("Generate points for island")]
    [Range(40, 80)]
    [SerializeField] private int _CountRooms;
    [Range(200, 400)]
    [SerializeField] private int _MinX;
    [Range(400, 1000)]
    [SerializeField] private int _MaxX;

    [Range(200, 400)]
    [SerializeField] private int _MinY;
    [Range(400, 1000)]
    [SerializeField] private int _MaxY;
    

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


    private List<GameObject> _points = new List<GameObject>();
    
    private Random _rndClass;
    private MovePoints _movePointsScript;

    private float _waiTime;

    private void Start() {
        _rndClass = new Random(_Seed);
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
        GameObject _pointStatic;
        GameObject _pointDinamic;

        float _distance = 0f;

        int _radiusStatic;
        int _radiusDinamic;   

        Vector3 _direction; 
        bool itersects = false;
        while(true) {
            for (int i = 0; i < _points.Count; i ++) {
                _pointStatic = _points[i];

                _radiusStatic = _pointStatic.GetComponent<Island>().SizeIsland;
                itersects = false;

                for (int j = 0; j < _points.Count; j++) {
                    if (i == j) {
                        continue;
                    }
                    _pointDinamic = _points[j];
                    _radiusDinamic = _pointDinamic.GetComponent<Island>().SizeIsland;

                    _distance = Vector3.Distance(_pointStatic.transform.position, _pointDinamic.transform.position);

                    itersects = (_distance < (min_distance + _radiusDinamic+ _radiusStatic));
                    _direction = new Vector3(rnd.Next(-1, 2), 0, rnd.Next(-1, 2)); 
                    if (_direction.x == 0 && _direction.z == 0) {
                        _direction.x = 1;
                    }
                    
                    while (itersects)
                    {
                        itersects = (_distance < (min_distance + _radiusDinamic+ _radiusStatic));
                        _pointDinamic.transform.position += _direction*20;
                        _distance = Vector3.Distance(_pointStatic.transform.position, _pointDinamic.transform.position);
                    }
                }
            }
           
            if (!itersects) {
                for (int i = 0; i < _points.Count; i ++) {
                    _pointStatic = _points[i];

                    for (int j = 0; j < _points.Count; j++) {
                        if (i == j) continue;
        
                        _pointDinamic = _points[j];
                        _distance = Vector3.Distance(_pointStatic.transform.position, _pointDinamic.transform.position);
                        itersects = (_distance < (min_distance + _pointDinamic.GetComponent<Island>().SizeIsland+ _pointStatic.GetComponent<Island>().SizeIsland));
                        if (itersects) {
                            Destroy(_pointDinamic);
                            _points.RemoveAt(j);
                        }
                    }
                }
                break;
            }
        }
    }
    public void GeneratePoints() {
        Vector3 _positionPoint;
        int _sizeIsland;
        Island _pointIslandScript;
        GameObject _point;
        for (int i = 0; i < _CountRooms; i++) {
            _positionPoint = new Vector3(_rndClass.Next(_MinX, _MaxX+1), 0, _rndClass.Next(_MinY, _MaxY+1));
            _sizeIsland = _rndClass.Next(_MinSizeIsland, _MaxSizeIsland);
            _point = Instantiate(_PrefabPoint, _positionPoint, Quaternion.identity); 
            _point.name = "Island " + Convert.ToString(_positionPoint.x) + " " + Convert.ToString(_positionPoint.z);
            _pointIslandScript = _point.GetComponent<Island>();
            _pointIslandScript.SizeIsland = _sizeIsland;
            if (_DebugMode) {
                _pointIslandScript.DebugStart();
            }
            else {
                _pointIslandScript.DisableDebug(); 
            }
            _points.Add(_point);

        }
        StartMovement(new Vector3(_MaxX, 0, _MaxY).magnitude, _rndClass);
        
        GenerateIsland();
    }

    public void GenerateIsland() {
        
        foreach (GameObject point in _points)
        {
            GameObject island = Instantiate(_islandPrefab, point.transform.position, Quaternion.identity);
            Island srciptIsland = point.GetComponent<Island>();
            island.transform.localScale  = new Vector3(srciptIsland.SizeIsland*5, island.transform.localScale.y, srciptIsland.SizeIsland*5);
            island.AddComponent<MeshCollider>();

            float min_distance = 10000000000;
            GameObject to = new GameObject();
            foreach (GameObject point_to in _points)   {
                if (point_to == point) continue;

                float distance = Vector3.Distance(point_to.transform.position, point.transform.position);

                if (distance < min_distance) {
                    to = point_to;
                    min_distance = distance;
                }   
            }     

            GameObject bridge = Instantiate(_brigd, point.transform.position, Quaternion.identity);
            bridge.transform.localScale = new Vector3(min_distance, 0.1f, 1f);
            bridge.transform.localRotation = Quaternion.LookRotation((point.transform.position - to.transform.position).normalized);
            bridge.transform.Rotate(0, 90, 0);


        }
        _game.transform.position = _points[0].transform.position + new Vector3(0, 4f, 0);

        foreach (GameObject point in _points) {Destroy(point);}
    
    }
}
