using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarMeshGenerator : MonoBehaviour
{
    public float width = 3f, height = 1f, trunk = 0.1f, backWindow = 0.1f, frontWindow = 0.1f, hood = 0.3f, cabinHeight = 1f;
    public float depth = 1f; // to distend
    public float inset = 0.2f;

    public Vector2[] GenerateUV (Vector3[] vs) {
        Vector2[] uvs = new Vector2[vs.Length];

        for (int i = 0; i < uvs.Length; i++) {
            uvs[i] = new Vector2(vs[i].x, vs[i].z);
        }

        return uvs;
    }

    // Assumes a flat (co-planar), bounded, quad in clockwise order
    public List<int> GenerateQuad (Vector3[] vs, List<int> indices) {
        return new List<int> {
            indices[0], indices[1], indices[2],
            indices[0], indices[2], indices[3]
        };
    }   

    public void Distend (ref Vector3[] vs, ref int[] triangles) {
        int T = vs.Length; //Amount of vertices in original shape to distend
        List<Vector3> dvs = new List<Vector3>(vs);
        for (int i = 0; i < T; i++) {
            Vector3 v = dvs[i];
            v.z = depth - v.z;
            dvs.Add(v);
        }

        vs = dvs.ToArray();

        List<int> faceQuads = new List<int>(triangles), newFaceQuads = new List<int>();

        for (int i = faceQuads.Count - 1; i >= 0; i--) {
            newFaceQuads.Add(faceQuads[i] + T);
        }

        List<int> connectingQuads = new List<int> ();
        for (int i = 0; i < T; i++) {
            int ip = (i + 1) % T;
            connectingQuads.AddRange(
                GenerateQuad(vs, new List<int> {i, i + T, ip + T, ip})
            );
        }

        List<int> quads = new List<int>();
        quads.AddRange(faceQuads);
        quads.AddRange(newFaceQuads);
        quads.AddRange(connectingQuads);

        triangles = quads.ToArray();
    }

    public void Start () {
        GameObject car = new GameObject("Car");
        MeshRenderer meshRenderer = car.AddComponent<MeshRenderer>();
        meshRenderer.sharedMaterial = new Material(Shader.Find("Diffuse"));

        MeshFilter meshFilter = car.AddComponent<MeshFilter>();

        Mesh mesh = new Mesh();

        Vector3[] vertices = new Vector3[8]
        {
            new Vector3(0, 0, 0), //0
            new Vector3(0, height, 0), //1
            new Vector3(trunk, height, 0), //2
            new Vector3(trunk + backWindow, cabinHeight + height, inset), //3

            new Vector3(width - (frontWindow + hood), cabinHeight + height, inset), //4
            new Vector3(width - hood, height, 0), //5
            new Vector3(width, height, 0), //6
            new Vector3(width, 0, 0) //7
        };

        List<int> quads = new List<int>();
        quads.AddRange(GenerateQuad(vertices, new List<int> {0, 1, 6, 7}));
        quads.AddRange(GenerateQuad(vertices, new List<int> {2, 3, 4, 5}));
        int[] triangles = quads.ToArray();

        Distend(ref vertices, ref triangles);
        
        mesh.vertices = vertices;
        mesh.triangles = triangles;

        mesh.uv = GenerateUV(vertices);
        mesh.RecalculateNormals();
        mesh.RecalculateBounds();

        meshFilter.mesh = mesh;
    }

    public void Update () {
        // Mesh mesh = GetComponent<MeshFilter>().mesh;

        // Vector3[] vertices = new Vector3[8]
        // {
        //     new Vector3(0, 0, 0), //0
        //     new Vector3(0, height, 0), //1
        //     new Vector3(trunk, height, 0), //2
        //     new Vector3(trunk + backWindow, cabinHeight + height, 0), //3

        //     new Vector3(width - (frontWindow + hood), cabinHeight + height, 0), //4
        //     new Vector3(width - hood, height, 0), //5
        //     new Vector3(width, height, 0), //6
        //     new Vector3(width, 0, 0) //7
        // };

        // mesh.vertices = vertices;
    }
}

