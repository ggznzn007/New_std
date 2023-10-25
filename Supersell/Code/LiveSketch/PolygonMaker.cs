using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PolygonMaker : MonoBehaviour
{
    void Start()
    {
        MakePolygon(new Vector3(0, 0, 0), new Vector3(1, 0, 0), new Vector3(1, 0, 1));
        MakeQuad(new Vector3(0, 0, 0), new Vector3(0, 0, 1), new Vector3(1, 0, 0), new Vector3(1, 0, 1));
        MakeSubmeshQuad(new Vector3(0, 0, 0), new Vector3(1, 0, 0), new Vector3(0, 0, 1), new Vector3(0, 0, 1), new Vector3(1, 0, 0), new Vector3(1, 0, 1));
    }

    public GameObject MakePolygon(Vector3 p1, Vector3 p2, Vector3 p3)
    {
        GameObject go = new GameObject("Poligon");
        Mesh mesh = new Mesh();
        MeshFilter mf = go.AddComponent<MeshFilter>();
        MeshRenderer mr = go.AddComponent<MeshRenderer>();
        Material mt = new Material(Shader.Find("Standard"));

        Vector3[] vertices = { p1, p2, p3 };
        Vector2[] uvs = { new Vector2(0, 0), new Vector2(1, 0), new Vector2(1, 1) };

        int[] tris = { 0, 2, 1 };
        mt.mainTexture = (Texture)Resources.Load("Texture/1941165_0");
        mesh.vertices = vertices;
        mesh.triangles = tris;
        mesh.uv = uvs;
        mf.mesh = mesh;
        mr.material = mt;

        return go;
    }

    public GameObject MakeQuad(Vector3 p1, Vector3 p2, Vector3 p3, Vector3 p4)
    {
        GameObject go = new GameObject("Quad");
        Mesh mesh = new Mesh();
        MeshFilter mf = go.AddComponent<MeshFilter>();
        MeshRenderer mr = go.AddComponent<MeshRenderer>();
        Material mt = new Material(Shader.Find("Standard"));
        mt.mainTexture = (Texture)Resources.Load("Texture/corodinateChecker");

        Vector3[] vertices = { p1, p2, p3, p4 };
        Vector2[] uvs = { new Vector2(0, 0), new Vector2(1, 0), new Vector2(0, 1), new Vector2(1, 1) };

        int[] tris = { 0, 1, 2, 2, 1, 3 };
        mesh.vertices = vertices;
        mesh.triangles = tris;
        mesh.uv = uvs;
        mf.mesh = mesh;
        mr.material = mt;

        return go;
    }

    public GameObject MakeSubmeshQuad(Vector3 p1, Vector3 p2, Vector3 p3, Vector3 p4, Vector3 p5, Vector3 p6)
    {
        GameObject go = new GameObject("subQuad");
        Mesh mesh = new Mesh();
        MeshFilter mf = go.AddComponent<MeshFilter>();
        MeshRenderer mr = go.AddComponent<MeshRenderer>();
        Material[] mts = new Material[2];

        mts[0] = new Material(Shader.Find("Standard"));
        mts[1] = new Material(Shader.Find("Standard"));
        mts[0].mainTexture = (Texture)Resources.Load("Texture/corodinateChecker");
        mts[1].mainTexture = (Texture)Resources.Load("Texture/corodinate");

        Vector3[] vertices = { p1, p2, p3, p4, p5, p6 };
        Vector2[] uvs = { new Vector2(0, 0), new Vector2(1, 0), new Vector2(0, 1), new Vector2(0, 1), new Vector2(1, 0), new Vector2(1, 1) };

        int[] subTris1 = { 0, 2, 1 };
        int[] subTris2 = { 4, 3, 5 };
        mesh.vertices = vertices;
        mesh.subMeshCount = 2;
        mesh.SetTriangles(subTris1, 0);
        mesh.SetTriangles(subTris2, 1);
        mesh.uv = uvs;

        mf.mesh = mesh;
        mr.materials = mts;
        return go;
    }
}
