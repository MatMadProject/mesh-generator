using System;
using UnityEngine;

namespace Assets.Scripts.MeshShapes
{
    [RequireComponent(typeof(MeshFilter))]
    [RequireComponent(typeof(MeshRenderer))]
    public class HexagonGridRectangle : MonoBehaviour
    {
        private Mesh mesh;
        [SerializeField]
        private bool UpdateShape = false;
        [SerializeField]
        private Vector3 startPoint = Vector3.zero;
        [SerializeField]
        private int gridWidth = 5;
        [SerializeField]
        private int gridHeight = 2;
        [SerializeField]
        private float sideSize = 1f;
        [SerializeField]
        private float tileMargin = 0.1f;
        private Vector3[] vertices = { };
        private int[] triangles = { };
        int addIndexInArray = 0;

        // Use this for initialization
        void Start()
        {
            mesh = new Mesh();
            GetComponent<MeshFilter>().mesh = mesh;

            if(tileMargin == 0)
                CreateRectangleMesh();
            else
                CreateRectangleMeshWithMargin();
            UpdateMesh();
        }

        // Update is called once per frame
        void Update()
        {
            if (UpdateShape)
            {
                ClearMeshTrainglesAndVertices();
                if (tileMargin == 0)
                    CreateRectangleMesh();
                else
                    CreateRectangleMeshWithMargin();
                UpdateMesh();
                UpdateShape = false;
            }
        }

        private void OnDrawGizmos()
        {
            DrawHorizontalDistanceBeetweenTiles();
            DrawVerticalDistanceBeetweenTiles();
        }

        private void DrawHorizontalDistanceBeetweenTiles()
        {
            if(vertices.Length > 0)
            {
                int tilesValue = gridHeight * gridWidth + ((gridHeight - 1) * (gridWidth - 1));
                int index;
                for (int i = 0; i <= tilesValue; i++)
                {
                    index = 1 + (i * Hexagon.VerticesAmmmount);
                    if (index < vertices.Length)
                    {
                        Vector3 tmp = vertices[index];
                        Gizmos.color = Color.white;
                        Gizmos.DrawLine(
                        tmp,
                        new Vector3(tmp.x + (3 * sideSize) + (2 * tileMargin), tmp.y, tmp.z));
                    }
                }
            }
        }

        private void DrawVerticalDistanceBeetweenTiles()
        {
            if (vertices.Length > 0)
            {
                int tilesValue = gridHeight * gridWidth + ((gridHeight-1) * (gridWidth-1));
                int index;
                for (int i = 0; i <= tilesValue; i++)
                {
                    index = 1 + (i * Hexagon.VerticesAmmmount);
                    if (index < vertices.Length)
                    {
                        Vector3 tmp = vertices[index];
                        Gizmos.color = Color.white;
                        Gizmos.DrawLine(
                            tmp,
                            new Vector3(tmp.x, tmp.y, tmp.z + tileMargin + 2 * (Hexagon.CalculateHeight(sideSize))
                            ));
                    }
                    
                }
            }
        }
        private void ClearMeshTrainglesAndVertices()
        {
            Array.Clear(vertices, 0, vertices.Length);
            Array.Clear(triangles, 0, triangles.Length);
        }
        private void CreateRectangleMesh()
        {
            Vector3 startPointOfMesh;
            float heightOffSet;
            float widthOffSet = 3 * (sideSize);

            for (int hGrid = 0; hGrid < gridHeight; hGrid++)
            {
                heightOffSet = hGrid * 2 * (Hexagon.CalculateHeight(sideSize));
                startPointOfMesh = new Vector3(
                    startPoint.x,
                    startPoint.y,
                    startPoint.z + heightOffSet);
                for (int i = 0; i < gridWidth; i++)
                {
                    startPointOfMesh += new Vector3(0f + widthOffSet, 0f, 0f);
                    AddSingleHexagon(startPointOfMesh);
                }
            }

            for (int hGrid = 0; hGrid < gridHeight - 1; hGrid++)
            {
                heightOffSet = hGrid * 2 * (Hexagon.CalculateHeight(sideSize));
                startPointOfMesh = new Vector3(
                    startPoint.x +(1.5f * sideSize),
                    startPoint.y,
                    startPoint.z + Hexagon.CalculateHeight(sideSize) + heightOffSet);
                for (int i = 0; i < gridWidth - 1; i++)
                {
                    startPointOfMesh += new Vector3(0f + widthOffSet, 0f, 0f);
                    AddSingleHexagon(startPointOfMesh);
                }
            }
        }
        private void CreateRectangleMeshWithMargin()
        {
            Vector3 startPointOfMesh;
            float heightOffSet;
            float widthOffSet = 3 * (sideSize);

            for (int hGrid = 0; hGrid < gridHeight; hGrid++)
            {
                heightOffSet = hGrid * (2 * (Hexagon.CalculateHeight(sideSize)) + tileMargin);
                startPointOfMesh = new Vector3(
                    startPoint.x,
                    startPoint.y,
                    startPoint.z + heightOffSet);
                for (int wGrid = 0; wGrid < gridWidth; wGrid++)
                {
                    if(wGrid != 0)
                        startPointOfMesh += new Vector3(0f +  widthOffSet + (2 * Hexagon.CalculateHeight(tileMargin)), 0f, 0f);
                    AddSingleHexagon(startPointOfMesh);
                }
            }

            for (int hGrid = 0; hGrid < gridHeight - 1; hGrid++)
            {
                heightOffSet = hGrid * (2 * (Hexagon.CalculateHeight(sideSize)) + tileMargin);
                startPointOfMesh = new Vector3(
                    startPoint.x + Hexagon.CalculateHeight(tileMargin) + (1.5f * sideSize),
                    startPoint.y,
                    startPoint.z + (tileMargin/2f) +  Hexagon.CalculateHeight(sideSize) + heightOffSet);
                for (int wGrid = 0; wGrid < gridWidth - 1; wGrid++)
                {
                    if (wGrid != 0)
                        startPointOfMesh += new Vector3(0f + widthOffSet + (2 * Hexagon.CalculateHeight(tileMargin)), 0f, 0f);
                    AddSingleHexagon(startPointOfMesh);
                }
            }
        }

        private void UpdateMesh()
        {
            mesh.Clear();
            mesh.vertices = vertices;
            mesh.triangles = triangles;
            mesh.RecalculateNormals();
        }

        private void AddSingleHexagon(Vector3 centerPoint)
        {
            Hexagon hexagon = new Hexagon(centerPoint, sideSize);
            hexagon.UpdateTraingles(addIndexInArray);
            AddVertices(hexagon);
            AddTriangles(hexagon);
            addIndexInArray++;
        }

        private void AddVertices(Hexagon hexagon)
        {
            Array.Resize(ref vertices, vertices.Length + Hexagon.VerticesAmmmount);
            hexagon.Vertices.CopyTo(vertices, 0 + addIndexInArray * Hexagon.VerticesAmmmount);
        }
        private void AddTriangles(Hexagon hexagon)
        {
            Array.Resize(ref triangles, triangles.Length + Hexagon.TrianglesAmmount);
            hexagon.Triangles.CopyTo(triangles, 0 + addIndexInArray * Hexagon.TrianglesAmmount);
        }
    }
}