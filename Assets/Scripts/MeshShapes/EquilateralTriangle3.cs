using System.Collections;
using UnityEngine;


[RequireComponent(typeof(MeshFilter))]
public class EquilateralTriangle3 : MonoBehaviour
{
    [Range(1, 8)]
    public int resolution = 2;
    [SerializeField, HideInInspector]
    MeshFilter meshFilter;
    [SerializeField]
    public int rowsOfVertices;
    [SerializeField]
    private Vector3[] vertices;
    [SerializeField]
    private int[] triangles;

    [SerializeField]
    private float sideSize = 1f;
    [SerializeField]
    private Vector3 horizontalVector;
    [SerializeField]
    private Vector3 verticalVector;
    [SerializeField]
    private Vector3 localUp;
    [SerializeField]
    private Vector3 startPoint;
    [SerializeField]
    private Vector3 axisX;
    [SerializeField]
    private Vector3 axisY;
    public Octahedron.Face OctahedronFace;
    private TriangleFace triangleFace;
    [SerializeField]
    private bool Sphere = false;

    private void OnValidate()
    {
        Initialize();
    }

    void Initialize()
    {
        triangleFace = TriangleFace.SelectFace(OctahedronFace);
        localUp = triangleFace.direction;
        axisY = triangleFace.axisY;
        axisX = Vector3.Cross(localUp, axisY) / 2f;

        if (meshFilter == null)
        {
            GameObject meshObj = new GameObject($"mesh");
            meshObj.transform.parent = transform;

            meshObj.AddComponent<MeshRenderer>().sharedMaterial = new Material(Shader.Find("Standard"));
            meshFilter = meshObj.AddComponent<MeshFilter>();
            meshFilter.sharedMesh = new Mesh();
        }

        GenerateMesh(meshFilter.sharedMesh, resolution);
    }

    private void OnDrawGizmos()
    {
        //Gizmos.color = Color.white;

        //foreach (Vector3 vertice in vertices)
            //Gizmos.DrawSphere(vertice, 0.01f);
    }

    public void GenerateMesh(Mesh mesh, int resolution)
    {

        rowsOfVertices = Mathf.CeilToInt(Mathf.Pow(2, resolution - 1)) + 1;
        int trainglesValue = Mathf.CeilToInt(Mathf.Pow(4, resolution - 1));
        int verticesValue = SumOfConsecutiveNaturalNumbers(rowsOfVertices);

        vertices = new Vector3[verticesValue];
        triangles = new int[trainglesValue * 3];

        CreateVertices();
        CreateTriangles();
        UpdateMesh(mesh);
    }

    private void CreateVertices()
    {
        int rows = rowsOfVertices;
        int index = 0;
        horizontalVector = (axisX * sideSize / (rowsOfVertices - 1));
        verticalVector = SelectVerticalVector(OctahedronFace);

        startPoint = triangleFace.leftDownVertices;
        for (int vertical = rows; vertical > 0; vertical--)
        {
            for (int horizontal = 0; horizontal < vertical; horizontal++)
            {
                Vector3 verticesPoint = startPoint + horizontalVector * horizontal;
                Vector3 pointOnSphere = verticesPoint.normalized;
                
                if(Sphere)
                    vertices[index] = pointOnSphere;
                else
                    vertices[index] = verticesPoint;
                index++;
            }
            startPoint += verticalVector;
        }
    }
    private void CreateTriangles()
    {
        int indexTriangles = 0;
        int offset = 0;

        for (int vertical = 0; vertical < rowsOfVertices - 1; vertical++)
        {
            for (int horizontal = 0; horizontal < (rowsOfVertices - vertical - 1); horizontal++)
            {
                if (horizontal == 0)
                {
                    triangles[indexTriangles] = horizontal + offset;
                    triangles[indexTriangles + 1] = horizontal + (rowsOfVertices - vertical) + offset;
                    triangles[indexTriangles + 2] = horizontal + 1 + offset;
                }
                else
                {
                    triangles[indexTriangles] = horizontal + offset;
                    triangles[indexTriangles + 1] = horizontal + (rowsOfVertices - vertical) - 1 + offset;
                    triangles[indexTriangles + 2] = horizontal + (rowsOfVertices - vertical) + offset;
                    indexTriangles += 3;
                    triangles[indexTriangles] = horizontal + offset;
                    triangles[indexTriangles + 1] = horizontal + (rowsOfVertices - vertical) + offset;
                    triangles[indexTriangles + 2] = horizontal + 1 + offset;
                }
                indexTriangles += 3;
            }
            offset += rowsOfVertices - vertical;
        }
    }

    public int SumOfConsecutiveNaturalNumbers(int lastNumber)
    {
        return lastNumber * (lastNumber + 1) / 2;
    }

    private void UpdateMesh(Mesh mesh)
    {
        mesh.Clear();
        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.RecalculateNormals();
    }

    private float CalculateHeight(float sideLength)
    {
        return (Mathf.Sqrt(3f) * sideLength) / 2f;
    }

    private Vector3 SelectVerticalVector(Octahedron.Face face)
    {
        switch (face)
        {
            case Octahedron.Face.ForwardUp:
                return 
                    verticalVector = new Vector3(
                        axisY.z / 2f / (rowsOfVertices - 1),
                        CalculateHeight(sideSize) / (rowsOfVertices - 1),
                        axisY.z / 2f / (rowsOfVertices - 1));
            case Octahedron.Face.BackUp:
                return 
                    verticalVector = new Vector3(
                        axisY.z / 2f / (rowsOfVertices - 1),
                        CalculateHeight(sideSize) / (rowsOfVertices - 1),
                        axisY.z / 2f / (rowsOfVertices - 1));
            case Octahedron.Face.LeftUp:
                return
                    verticalVector = new Vector3(
                        axisY.x / 2f / (rowsOfVertices - 1),
                        CalculateHeight(sideSize) / (rowsOfVertices - 1),
                        axisY.x / 2f / (rowsOfVertices - 1) * -1f);
            case Octahedron.Face.RightUp:
                return
                    verticalVector = new Vector3(
                        axisY.x / 2f / (rowsOfVertices - 1),
                        CalculateHeight(sideSize) / (rowsOfVertices - 1),
                        axisY.x / 2f / (rowsOfVertices - 1) * -1f);
            case Octahedron.Face.ForwardDown:
                return
                   verticalVector = new Vector3(
                       axisY.z / 2f / (rowsOfVertices - 1) * -1f,
                       CalculateHeight(sideSize) / (rowsOfVertices - 1) * -1f,
                       axisY.z / 2f / (rowsOfVertices - 1));
            case Octahedron.Face.BackDown:
                return
                verticalVector = new Vector3(
                       axisY.z / 2f / (rowsOfVertices - 1) * -1f,
                       CalculateHeight(sideSize) / (rowsOfVertices - 1) * -1f,
                       axisY.z / 2f / (rowsOfVertices - 1));
            case Octahedron.Face.LeftDown:
                return
                    verticalVector = new Vector3(
                        axisY.x / 2f / (rowsOfVertices - 1),
                        CalculateHeight(sideSize) / (rowsOfVertices - 1) * -1f,
                        axisY.x / 2f / (rowsOfVertices - 1));
            case Octahedron.Face.RightDown:
                return
                    verticalVector = new Vector3(
                        axisY.x / 2f / (rowsOfVertices - 1),
                        CalculateHeight(sideSize) / (rowsOfVertices - 1) * -1f,
                        axisY.x / 2f / (rowsOfVertices - 1));
        }
        return Vector3.zero;
    }

    public sealed class TriangleFace
    {
        public Vector3 direction;
        public Vector3 axisY;
        public Vector3 leftDownVertices;

        public TriangleFace(Vector3 direction, Vector3 axisY, Vector3 leftDownVertices)
        {
            this.direction = direction;
            this.axisY = axisY;
            this.leftDownVertices = leftDownVertices;
        }

        public static readonly TriangleFace ForwardUp = new TriangleFace(
            Vector3.forward + Vector3.up,
            Vector3.back + Vector3.up,
            new Vector3(.5f, 0, .5f)
            );
        public static readonly TriangleFace BackUp = new TriangleFace(
            Vector3.back + Vector3.up,
            Vector3.forward + Vector3.up,
            new Vector3(-.5f, 0, -.5f)
            );
        public static readonly TriangleFace LeftUp = new TriangleFace(
            Vector3.left + Vector3.up,
            Vector3.right + Vector3.up,
            new Vector3(-.5f, 0, .5f)
            );
        public static readonly TriangleFace RightUp = new TriangleFace(
            Vector3.right + Vector3.up,
            Vector3.left + Vector3.up,
            new Vector3(.5f, 0, -.5f)
            );
        public static readonly TriangleFace ForwardDown = new TriangleFace(
            Vector3.forward + Vector3.down,
            Vector3.back + Vector3.down,
            new Vector3(-.5f, 0, .5f)
            );
        public static readonly TriangleFace BackDown = new TriangleFace(
            Vector3.back + Vector3.down,
            Vector3.forward + Vector3.down,
            new Vector3(.5f, 0, -.5f)
            );
        public static readonly TriangleFace LeftDown = new TriangleFace(
            Vector3.left + Vector3.down,
            Vector3.right + Vector3.down,
            new Vector3(-.5f, 0, -.5f)
            );
        public static readonly TriangleFace RightDown = new TriangleFace(
            Vector3.right + Vector3.down,
            Vector3.left + Vector3.down,
            new Vector3(.5f, 0, .5f)
            );

        public static TriangleFace SelectFace(Octahedron.Face face)
        {
            switch (face)
            {
                case Octahedron.Face.ForwardUp:
                    return ForwardUp;
                case Octahedron.Face.BackUp:
                    return BackUp;
                case Octahedron.Face.LeftUp:
                    return LeftUp;
                case Octahedron.Face.RightUp:
                    return RightUp;
                case Octahedron.Face.ForwardDown:
                    return ForwardDown;
                case Octahedron.Face.BackDown:
                    return BackDown;
                case Octahedron.Face.LeftDown:
                    return LeftDown;
                case Octahedron.Face.RightDown:
                    return RightDown;
            }
            return null;
        }
    }

}
