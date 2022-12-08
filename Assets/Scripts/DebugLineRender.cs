using UnityEngine;
using System.Collections;

//[RequireComponent(typeof(LineRenderer))]
public class DebugLineRender : MonoBehaviour {
    
    private int _radius;

    public int RadiusIsland {
        set {_radius = value;}
    }

    private LineRenderer line;
    private int segments = 50;
    
    public void SetComponents() {
        line = GetComponent<LineRenderer>();
        line.SetVertexCount(segments + 1);
        line.useWorldSpace = false;
        line.SetWidth(10, 10);
    }
    public void StartRender()
    {
        SetComponents();
        CreatePoints();
    }

    private void CreatePoints()
    {
        float x;
        float z;
        float angle = 20f;
        for (int i = 0; i < (segments + 1); i++)
        {
            x = Mathf.Sin(Mathf.Deg2Rad * angle) * _radius;
            z = Mathf.Cos(Mathf.Deg2Rad * angle) * _radius;
            line.SetPosition(i,new Vector3(x,0,z));
            angle += (360f / segments);
        }
    }

    private void OnDisable() {
        SetComponents();
        line.enabled = false;    
    }
}