
using UnityEngine;

public class OctahedronFace
{
    Mesh mesh;
    int resolution;
    private Vector3[] vertices;
    private int[] triangles;
    private int rowsOfVertices;
    private float sideSize = 1f;
    private Vector3 horizontalVector;
    private Vector3 verticalVector;
    private Vector3 localUp;
    private Vector3 axisX;
    private Vector3 axisY;
    private Vector3 leftDownCorner;
    private static int iterator = 0;

    public OctahedronFace(Mesh mesh, int resolution, Vector3 localUp, float sideSize)
    {
        this.mesh = mesh;
        this.resolution = resolution;
        this.localUp = localUp;
        this.sideSize = sideSize;

        axisX = new Vector3(localUp.y, localUp.z, localUp.x); //?
        axisY = Vector3.Cross(localUp, axisX); //?
    }

    public OctahedronFace(Mesh mesh, int resolution, Vector3 localUp, Vector3 axisX, Vector3 leftDownCorner)
    {

        this.mesh = mesh;
        this.resolution = resolution;
        this.localUp = localUp;
        this.axisX = axisX;
        this.leftDownCorner = leftDownCorner;
        axisY = Vector3.Cross(localUp, axisX);
        Debug.Log($"{iterator}:\tlocalUp:{localUp}\t\taxisX:{axisX}\t\taxisY:{axisY}");
        iterator++;
    }

    public void CreateMesh()
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
        horizontalVector = new Vector3(sideSize / (rowsOfVertices - 1), 0f, 0f);
        verticalVector = new Vector3((sideSize / 2) / (rowsOfVertices - 1), 0f, CalculateHeight(sideSize) / (rowsOfVertices - 1));
        //horizontalVector = new Vector3(axisX.x + sideSize / (rowsOfVertices - 1), axisX.y, axisX.z);
        //verticalVector = new Vector3((sideSize / 2) / (rowsOfVertices - 1), 0f, CalculateHeight(sideSize) / (rowsOfVertices - 1));
        Vector3 startPoint = leftDownCorner;

        for (int vertical = rows; vertical > 0; vertical--)
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
}
