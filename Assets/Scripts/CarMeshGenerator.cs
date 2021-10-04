using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarMeshGenerator : MonoBehaviour
{
    float width = 2f, height = 0.6f, trunk = 0.1f, backWindow = 0.1f, frontWindow = 0.3f, hood = 0.35f, cabinHeight = .6f;
    float depth = 1f; // to distend
    float inset = 0.1f, borderProportion = 0.90f;
    float tireSize = 0f;

    enum Parameter {
        width,
        height,
        depth,
        cabinHeight,
        inset,
        trunk,
        hood,
        backWindow,
        frontWindow,
        tireSize
    }

    Dictionary<Parameter, (float, float)> ranges = new Dictionary<Parameter, (float, float)> {
        {Parameter.width, (2f, 3f)},
        {Parameter.height, (0.15f, 0.4f)}, // factor --> width
        {Parameter.depth, (0.5f, 0.7f)}, // factor --> width
        {Parameter.cabinHeight, (0.2f, 0.5f)}, // factor --> width
        {Parameter.inset, (0.0f, 0.2f)},
        {Parameter.trunk, (0.0f, 0.5f)}, // factor --> width
        {Parameter.hood, (0.0f, 0.5f)}, // factor --> width
        {Parameter.backWindow, (0.0f, 0.1f)}, // factor --> width
        {Parameter.frontWindow, (0.1f, 0.2f)}, // factor --> width
        {Parameter.tireSize, (1f, 1.5f)}, // factor --> width

    };

    float GetRandomParameter (Parameter p) {
        return Random.Range(ranges[p].Item1, ranges[p].Item2);
    }

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

    public void AppendToSubMesh (ref Mesh mesh, int subMesh, List<int> triangles) {
        List<int> oldTriangles = new List<int> (mesh.GetTriangles(subMesh));
        oldTriangles.AddRange(triangles);
        mesh.SetTriangles(oldTriangles.ToArray(), subMesh);
    }

    // might eventually pass anchor points to distend from
    public void Distend (ref Mesh mesh /*, ref Vector3[] vs, ref int[] triangles*/) {
        List<Vector3> vs = new List<Vector3> (mesh.vertices);

        int T = vs.Count; //Amount of vertices in original shape to distend
        List<Vector3> dvs = new List<Vector3>(vs);
        for (int i = 0; i < T; i++) {
            Vector3 v = dvs[i];
            v.z = depth - v.z;
            dvs[i] = v;
        }

        vs.AddRange(dvs);

        mesh.vertices = vs.ToArray();

        for (int i = 0; i < mesh.subMeshCount; i++) {
            List<int> triangles = new List<int>(mesh.GetTriangles(i)), newTriangles = new List<int>();
            for (int j = triangles.Count - 1; j >= 0; j--) {
                newTriangles.Add(triangles[j] + T);
            }
            AppendToSubMesh(ref mesh, i, newTriangles);
        }

        List<int> connectingQuads = new List<int> ();
        for (int i = 0; i < T; i++) {
            int ip = (i + 1) % T;
            List<int> surface = new List<int> {i, i + T, ip + T, ip};
            
            if (i == 4 || i == 2) 
                WindowifyQuad(ref mesh, surface, borderProportion);
            else
                AppendToSubMesh(ref mesh, 0, GenerateQuad(vs.ToArray(), surface));
        }
    }

    // vs = vertices, indicies = the 4 points of the quad to windowify
    // should take a quad, and return 5 quads, a central window and 4 bounding thin quads
    // proportion [0-1]
    // this needs to be uncommited triangles. Only the triangles of the quad...
    public void WindowifyQuad (ref Mesh mesh, List<int> indicies, float proportion) {
        Vector3 center = new Vector3();
        List<Vector3> vs = new List<Vector3> (mesh.vertices);
        for (int i = 0; i < indicies.Count; i++) {
            center += vs[indicies[i]];
        }

        center /= indicies.Count;

        List<Vector3> innerVs = new List<Vector3>();
        for (int i = 0; i < indicies.Count; i++) {
            innerVs.Add(center + (vs[indicies[i]] - center) * proportion);
        }

        int T = vs.Count;
        List<int> windowVertices = new List<int> {T, T+1, T+2, T+3};
        vs.AddRange(innerVs);

        mesh.vertices = vs.ToArray();

        AppendToSubMesh(ref mesh, 1, GenerateQuad(vs.ToArray(), windowVertices)); //1 is windows

        for (int i = 0; i < indicies.Count; i++) {
            int ip = (i+1) % indicies.Count;
            List<int> borderVertices = new List<int> {indicies[i], indicies[ip], T + ip, T+i};
            AppendToSubMesh(ref mesh, 0, GenerateQuad(vs.ToArray(), borderVertices));
        }
    }

    List<Color> CarColors = new List<Color> {
        Color.red,
        Color.yellow,
        Color.green,
        Color.cyan,
        Color.gray,
        Color.white
    };

    public void GenerateCar () {
        GameObject car = new GameObject("Car");
        MeshRenderer meshRenderer = car.AddComponent<MeshRenderer>();
        Material body = new Material(Shader.Find("Standard")), window = new Material(Shader.Find("Standard"));
        window.color = Color.blue;
        body.color = CarColors[Random.Range(0, CarColors.Count)];
        meshRenderer.materials = new Material[] {body, window};

        width = GetRandomParameter(Parameter.width);
        height = width * GetRandomParameter(Parameter.height);
        depth = width * GetRandomParameter(Parameter.depth);
        cabinHeight = width * GetRandomParameter(Parameter.cabinHeight);
        inset = GetRandomParameter(Parameter.inset);
        trunk = width * GetRandomParameter(Parameter.trunk);
        hood = width * (GetRandomParameter(Parameter.hood));
        backWindow = width * GetRandomParameter(Parameter.backWindow);
        frontWindow = width * GetRandomParameter(Parameter.frontWindow);
        tireSize = width * GetRandomParameter(Parameter.tireSize);

        if (trunk > hood)
            hood /= 3f;
        else
            trunk /= 3f;


        MeshFilter meshFilter = car.AddComponent<MeshFilter>();

        Mesh mesh = new Mesh();
        mesh.subMeshCount = 2;

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

        mesh.vertices = vertices;

        List<int> quads = new List<int>();
        quads.AddRange(GenerateQuad(vertices, new List<int> {0, 1, 6, 7}));
        AppendToSubMesh(ref mesh, 0, quads);
        WindowifyQuad(ref mesh, new List<int> {2, 3, 4, 5}, borderProportion);
        int[] triangles = quads.ToArray();

        Distend(ref mesh);

        mesh.uv = GenerateUV(mesh.vertices);
        mesh.RecalculateNormals();
        mesh.RecalculateBounds();

        meshFilter.mesh = mesh;

        List<GameObject> wheels = new List<GameObject>();
        for (int i = 0; i < 4; i++) {
            GameObject w = Instantiate(wheel);
            float j = (i % 2), k = (i >= 2) ? 0 : 1;

            w.transform.parent = car.transform;
            w.transform.localScale = new Vector3(tireSize, tireSize, tireSize);

            w.transform.localPosition =  new Vector3(width/4+(width/2*j), 0, depth-0.1f-((depth-0.2f)*k));
            w.transform.rotation = new Quaternion(0, 180*(-k), 0, 0);
        }

        car.transform.parent = gameObject.transform;
        // var v = car.transform.position;
        // v.y = tireSize/10;
        car.transform.localPosition = new Vector3 (0f, tireSize/10, 0f);

        var collider = gameObject.GetComponent<BoxCollider>();
        collider.size = new Vector3(width, tireSize/10+height+cabinHeight, depth);
        collider.center = new Vector3(width/2, (tireSize/10+height+cabinHeight)/2, depth/2);

        // return car;
    }

    public void Start () {
       GenerateCar();
    }

    // GameObject car = null;
    [SerializeField] GameObject wheel;

    public void Update () { 

    }
    //     if (Input.GetKey("space")) {
    //         if (car != null) {
    //             Destroy(car);
    //         }

    //         car = GenerateCar();
    //     }
    // }
}

