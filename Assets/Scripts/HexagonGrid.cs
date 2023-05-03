using System;
using UnityEngine;


namespace Assets.Scripts
{
    [RequireComponent(typeof(MeshFilter))]
    public class HexagonGrid : MonoBehaviour
    {
        private Mesh mesh;
        [SerializeField]
        private bool UpdateShape = false;
        [SerializeField]
        private int gridWidth = 5;
        [SerializeField]
        private int gridHeight = 2;
        [SerializeField]
        private Vector3 startPoint = Vector3.zero;
        [SerializeField]
        private float sideLength = 1f;
        private Vector3[] vertices = { };
        private int[] triangles = { };
        int addIndexInArray = 0;

        // Use this for initialization
        void Start()
        {
            mesh = new Mesh();
            GetComponent<MeshFilter>().mesh = mesh;

            CreateRectangleMesh();
            UpdateMesh();
        }

        // Update is called once per frame
        void Update()
        {
            if (UpdateShape)
            {
                CreateRectangleMesh();
                UpdateMesh();
            }
        }

        private void CreateRectangleMesh()
        {
            Vector3 startPointOfMesh = startPoint;
            float heightOffSet;
            float widthOffSet = 3 * sideLength; 

            for (int hGrid = 0; hGrid < gridHeight; hGrid++)
            {
                heightOffSet = hGrid * 2 * Hexagon.CalculateHeight(sideLength);
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

            startPointOfMesh = startPoint;
            for (int hGrid = 0; hGrid < gridHeight-1; hGrid++)
            {
                heightOffSet = hGrid * 2 * Hexagon.CalculateHeight(sideLength);
                startPointOfMesh = new Vector3(
                    startPoint.x +1.5f * sideLength,
                    startPoint.y, 
                    startPoint.z + Hexagon.CalculateHeight(sideLength) + heightOffSet);
                for (int i = 0; i < gridWidth-1; i++)
                {
                    startPointOfMesh += new Vector3(0f + widthOffSet, 0f, 0f);
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
            Hexagon hexagon = new Hexagon(centerPoint, sideLength);
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