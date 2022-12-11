using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "TiledMesh_0", menuName = "Advanced Mesh/Tiled Mesh", order = 2)]
public class TiledMesh : ScriptableObject
{
    public Mesh meshSpritesheet;
    public Texture texture;

    public float animationFPS = 12;

    public Vector2 tileSize;

    public bool refresh = false;

    public List<List<Mesh>> meshes {
        get {
            if (_meshes == null) {
                if (bakedMeshes.Count == 0 || refresh) {
                    SplitMeshes ();

                    bakedMeshes = new List<MeshLine>();
                    foreach (List<Mesh> ml in _meshes) {
                        bakedMeshes.Add(new MeshLine(ml));
                    }
                } else {
                    _meshes = new List<List<Mesh>> ();
                    foreach (MeshLine ml in bakedMeshes) {
                        _meshes.Add(ml.ToMeshes());
                    }
                }
            }
            return _meshes;
        }
    }

    private List<List<Mesh>> _meshes;

    [SerializeField]
    private List<MeshLine> bakedMeshes = new List<MeshLine>();

    private MeshAnimation[] animations = new MeshAnimation[0];

    public float voxelSize = 0.1f;
    public float newVoxelSize = -1;

    private List<List<ListMesh>> listMeshes;

    private Dictionary<int, int> vertexNewMeshX;
    private Dictionary<int, int> vertexNewMeshY;
    private Dictionary<int, int> vertexNewID;

    public MeshAnimation GetAnimation (int y) {
        if (animations == null || animations.Length == 0) animations = new MeshAnimation[meshes.Count];
        MeshAnimation anim = animations[y];
        if (anim == null) {
            anim = ScriptableObject.CreateInstance<MeshAnimation>();
            anim.animationFPS = animationFPS;
            anim.frames = meshes[y];
            animations[y] = anim;
        }

        return anim;
    }

    public void SplitMeshes () {
        listMeshes = new List<List<ListMesh>> ();
        vertexNewMeshX = new Dictionary<int, int>();
        vertexNewMeshY = new Dictionary<int, int>();
        vertexNewID = new Dictionary<int, int>();

        float scaleMod = newVoxelSize < 0 ? 1 : newVoxelSize / voxelSize;
        Vector2 size = voxelSize * tileSize * scaleMod;
        Vector3 pivot = meshSpritesheet.bounds.min;
        Vector3 tilePivot = new Vector3 (size.x / 2, 0, size.y / 2);
        
        Vector3[] vertices = meshSpritesheet.vertices;
        Vector3[] normals = meshSpritesheet.normals;
        List<Vector2> uv = new List<Vector2>();
        meshSpritesheet.GetUVs(0, uv);

        for (int i = 0; i < meshSpritesheet.vertexCount; i++) {
            Vector3 v = (vertices[i] - pivot) * scaleMod;
            if (v.magnitude < 1.75f * voxelSize * scaleMod) {
                vertexNewMeshX.Add (i, -1);
            } else {
                int x = Mathf.FloorToInt (v.x / size.x);
                int y = Mathf.FloorToInt (v.z / size.y);

                if (v.x % size.x == 0) x--;
                if (v.z % size.y == 0) y--;

                ValidateMesh (x, y);

                ListMesh lm = listMeshes[y][x];

                vertexNewMeshX.Add (i, x);
                vertexNewMeshY.Add (i, y);
                vertexNewID.Add (i, lm.vertices.Count);

                lm.vertices.Add (v - tilePivot - new Vector3(x * size.x, 0, y * size.y));
                lm.normals.Add (normals[i]);
                lm.uv.Add (uv[i]);
            }
        }

        for (int i = 0; i < meshSpritesheet.triangles.Length; i++) {
            int vID = meshSpritesheet.triangles[i];

            int newMeshX = vertexNewMeshX[vID];

            if (newMeshX != -1) {
                int newMeshY = vertexNewMeshY[vID];
                int newID = vertexNewID[vID];

                listMeshes[newMeshY][newMeshX].triangles.Add(newID);
            }
        }

        _meshes = new List<List<Mesh>> ();
        for (int y = 0; y < listMeshes.Count; y++) {
            List<Mesh> ml = new List<Mesh>();
            for (int x = 0; x < listMeshes[y].Count; x++) {
                ListMesh lm = listMeshes[y][x];
                if (lm == null) {
                    ml.Add (null);
                } else {
                    ml.Add (lm.ToMesh());
                }
            }
            _meshes.Add (ml);
        }

        listMeshes.Clear ();
    }

    void ValidateMesh (int x, int y) {
        int overflowY = y + 1 - listMeshes.Count;
        if (overflowY > 0) {
            for (int i = 0; i < overflowY; i++) {
                listMeshes.Add(new List<ListMesh>());
            }
        }

        List<ListMesh> meshLine = listMeshes[y];
        int overflowX = x + 1 - meshLine.Count;

        if (overflowX > 0) {
            for (int i = 0; i < overflowX; i++) {
                meshLine.Add(null);
            }
        }

        ListMesh mesh = meshLine[x];
        if (mesh == null) {
            meshLine[x] = new ListMesh ();
            listMeshes[y] = meshLine;
        }
    }
}

public class ListMesh
{
    public List<Vector3> vertices = new List<Vector3> ();
    public List<Vector3> normals = new List<Vector3> ();
    public List<int> triangles = new List<int> ();
    public List<Vector2> uv = new List<Vector2> ();

    public Mesh ToMesh () {
        Mesh mesh = new Mesh();

        mesh.vertices = vertices.ToArray ();
        mesh.normals = normals.ToArray ();
        mesh.triangles = triangles.ToArray ();
        mesh.SetUVs (0, uv);

        return mesh;
    }
}

[System.Serializable]
public class MeshLine
{
    public List<SerializableMesh> meshes;

    public MeshLine (List<Mesh> ml) {
        meshes = new List<SerializableMesh>();
        foreach (Mesh msh in ml) {
            meshes.Add(new SerializableMesh (msh));
        }
    }

    public List<Mesh> ToMeshes () {
        List<Mesh> meshList = new List<Mesh> ();
        foreach (SerializableMesh mesh in meshes) {
            meshList.Add(mesh.ToMesh());
        }

        return meshList;
    }
}

[System.Serializable]
public struct SerializableMesh
{
    public Vector3[] vertices;
    public Vector3[] normals;
    public List<Vector2> uvs;
    public int[] triangles;

    public SerializableMesh (Mesh mesh) {
        vertices = mesh.vertices;
        normals = mesh.normals;
        uvs = new List<Vector2>();
        mesh.GetUVs(0, uvs);
        triangles = mesh.triangles;
    }

    public Mesh ToMesh () {
        Mesh mesh = new Mesh ();
        mesh.vertices = vertices;
        mesh.normals = normals;
        mesh.SetUVs (0, uvs);
        mesh.triangles = triangles;

        return mesh;
    }
}