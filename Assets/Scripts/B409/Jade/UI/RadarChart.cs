using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using Sirenix.OdinInspector;

namespace B409.Jade.UI {
    [RequireComponent(typeof(CanvasRenderer))]
    public class RadarChart : MonoBehaviour {
        [SerializeField]
        private Material mat;
        [SerializeField]
        private LineRenderer linePrefab;

        [SerializeField]
        private Color fillColor;
        [SerializeField]
        private Color lineColor;
        [SerializeField]
        private Color backgroundColor;

        [SerializeField]
        private string sortingLayerName;
        [SerializeField]
        private int sortingOrder;

        [SerializeField]
        private int count;
        [SerializeField]
        private int segment;
        [SerializeField]
        private float chartSize;
        [SerializeField]
        private float lineWidth;
        [SerializeField]
        private float maxValue;
        [SerializeField]
        private float[] values;

        [SerializeField]
        [HideInInspector]
        private List<GameObject> lines = new List<GameObject>();
        [SerializeField]
        [HideInInspector]
        private CanvasRenderer background;
        [SerializeField]
        [HideInInspector]
        private CanvasRenderer fill;

        public void SetMaxValue(float maxValue) {
            this.maxValue = maxValue;
            SetLines();
        }

        public void SetValues(float[] values) {
            this.values = values;
            SetFillMesh();
        }

        [Button]
        public void Draw() {
            SetLines();
            SetFillMesh();
        }

        private void SetFillMesh() {
            if(fill == null) {
                var g = new GameObject();

                g.transform.SetParent(transform);
                g.transform.localPosition = Vector3.zero;
                g.transform.localScale = Vector3.one;

                fill = g.AddComponent<CanvasRenderer>();
            }

            Mesh mesh = new Mesh();

            var vertices = new Vector3[count + 1];
            var uv = new Vector2[count + 1];
            var triangles = new int[3 * count];

            float angle = 360f / count;

            vertices[0] = Vector3.zero;

            for(int i = 0; i < count; i++) {
                var vertex = Quaternion.Euler(0, 0, -angle * i) * Vector3.up * chartSize * (values[i] / maxValue);
                vertices[i + 1] = vertex;

                triangles[i * 3] = 0;
                triangles[i * 3 + 1] = i + 1;
                if(i + 2 > count) {
                    triangles[i * 3 + 2] = i + 2 - count;
                } else {
                    triangles[i * 3 + 2] = i + 2;
                }
            }

            mesh.vertices = vertices;
            mesh.uv = uv;
            mesh.triangles = triangles;

            fill.SetMesh(mesh);
            var fillMat = Instantiate(mat);
            fillMat.color = fillColor;
            fill.SetMaterial(fillMat, null);

            fill.transform.SetAsLastSibling();
        }
   
        private void SetLines() {
            foreach(var line in lines) {
                DestroyImmediate(line);
            }

            float angle = 360f / count;

            float width = lineWidth * this.transform.lossyScale.x;

            var vertexes = new Vector3[count];

            for(int i = 0; i < count; i++) {
                var line = Instantiate(linePrefab, transform);
                line.transform.localPosition = Vector3.zero;
                line.transform.localScale = Vector3.one;

                var vertex = Quaternion.Euler(0, 0, -angle * i) * Vector3.up * chartSize;
                vertexes[i] = vertex;

                line.startWidth = line.endWidth = width;
                line.positionCount = 2;
                line.SetPositions(new Vector3[] { Vector3.zero, vertex });
                var lineMat = Instantiate(mat);
                lineMat.color = lineColor;
                line.material = lineMat;

                line.sortingLayerName = sortingLayerName;
                line.sortingOrder = sortingOrder;

                lines.Add(line.gameObject);
            }

            for(int i = 0; i < segment; i++) {
                var line = Instantiate(linePrefab, transform);

                line.transform.localPosition = Vector3.zero;
                line.transform.localScale = Vector3.one;

                line.startWidth = line.endWidth = width;
                var lineMat = Instantiate(mat);
                lineMat.color = lineColor;
                line.material = lineMat;
                line.positionCount = count;

                line.loop = true;
                line.sortingLayerName = sortingLayerName;
                line.sortingOrder = sortingOrder;

                float multiplier = 1 - (float)i / segment;
                line.SetPositions(vertexes.Select(e => e * multiplier).ToArray());

                lines.Add(line.gameObject);
            }

            if(this.background == null) {
                var g = new GameObject();

                g.transform.SetParent(transform);
                g.transform.localPosition = Vector3.zero;
                g.transform.localScale = Vector3.one;

                background = g.AddComponent<CanvasRenderer>();
            }

            background.transform.SetAsFirstSibling();

            Mesh mesh = new Mesh();

            var vertices = new Vector3[count + 1];
            var uv = new Vector2[count + 1];
            var triangles = new int[3 * count];

            vertices[0] = Vector3.zero;

            for(int i = 0; i < count; i++) {
                var vertex = Quaternion.Euler(0, 0, -angle * i) * Vector3.up * chartSize;
                vertices[i + 1] = vertex;

                triangles[i * 3] = 0;
                triangles[i * 3 + 1] = i + 1;
                if(i + 2 > count) {
                    triangles[i * 3 + 2] = i + 2 - count;
                } else {
                    triangles[i * 3 + 2] = i + 2;
                }
            }

            mesh.vertices = vertices;
            mesh.uv = uv;
            mesh.triangles = triangles;

            background.SetMesh(mesh);
            var bgMat = Instantiate(mat);
            bgMat.color = backgroundColor;
            background.SetMaterial(bgMat, null);
        }
    }
}