
using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
public class EquilateralTriangle2 : MonoBehaviour
{
    [Range(1, 8)]
    public int resolution = 2;
    [SerializeField, HideInInspector]
    MeshFilter meshFilter;
    [SerializeField]
    public int rowsOfVertices;
    private Vector3[] vertices;
    private int[] triangles;

    [SerializeField]
    private float sideSize = 1f;
    private Vector3 horizontalVector;
    private Vector3 verticalVector;

    private void OnValidate()
    {
        Initialize();
    }

    void Initialize()
    {
        Vector3 direction = Vector3.back;

        if (meshFilter == null)
        {
            GameObject meshObj = new GameObject($"mesh");
            meshObj.transform.parent = transform;

            meshObj.AddComponent<MeshRenderer>().sharedMaterial = new Material(Shader.Find("Standard"));
            meshFilter = meshObj.AddComponent<MeshFilter>();
            meshFilter.sharedMesh = new Mesh();
        }

        GenerateMesh(meshFilter.sharedMesh, resolution, direction);
    }


    public void GenerateMesh(Mesh mesh, int resolution, Vector3 localUp)
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
        horizontalVector = new Vector3(sideSize / (rowsOfVertices -1), 0f, 0f);
        verticalVector = new Vector3((sideSize / 2) / (rowsOfVertices -1), 0f,CalculateHeight(sideSize) / (rowsOfVertices - 1));
        Vector3 startPoint = new Vector3(0,0,0);

        for (int vertical = rows; vertical > 0; vertical--)
        {
            for (int horizontal = 1; horizontal <= vertical; horizontal++)
            {
                Vector3 verticesPoint = startPoint + horizontalVector * horizontal;
                vertices[index] = verticesPoint;
                index++;
            }
            startPoint += verticalVector;
        }
    }
    //depracted
    private void CreateVertices2()
    {
        int index = 0;
        horizontalVector = new Vector3(sideSize / (rowsOfVertices - 1), 0f, 0f);
        verticalVector = new Vector3(-(sideSize / 2) / (rowsOfVertices - 1), 0f, -CalculateHeight(sideSize) / (rowsOfVertices - 1));
        Vector3 startPoint = new Vector3(0, 0, 0);

        for (int vertical = 0; vertical < rowsOfVertices; vertical++)
        {
            for (int horizontal = 0; horizontal < vertical; horizontal++)
            {
                Vector3 verticesPoint = startPoint + horizontalVector * horizontal;
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

        for (int vertical = 0; vertical < rowsOfVertices-1; vertical++)
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
        return (Mathf.Sqrt(3f) * sideLength)/2f;
    }

}