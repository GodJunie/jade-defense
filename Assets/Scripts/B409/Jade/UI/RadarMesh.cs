using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RadarMesh : MonoBehaviour {

    private new CanvasRenderer renderer;
    [SerializeField]
    private Material material;

    private void Awake() {

        this.renderer = transform.Find("radarMesh").GetComponent<CanvasRenderer>();

    }

    // Start is called before the first frame update
    void Start() {
        Test();
    }

    // Update is called once per frame
    void Update() {

    }

    private void Test() {
        Mesh mesh = new Mesh();

        var vertices = new Vector3[3];
        var uv = new Vector2[3];
        var triangles = new int[3];

        vertices[0] = Vector3.zero;
        vertices[1] = new Vector3(0, 100);
        vertices[2] = new Vector3(100, 100);

        triangles[0] = 0;
        triangles[1] = 1;
        triangles[2] = 2;

        mesh.vertices = vertices;
        mesh.uv = uv;
        mesh.triangles = triangles;

        renderer.SetMesh(mesh);
        renderer.SetMaterial(material, null);

    }
}
