using System.Collections;
using UnityEngine;


public class PlanetOctahedronModelFace
{
    Mesh mesh;
    int resolution;
    private Vector3[] vertices;
    private int[] triangles;
    private int rowsOfVertices;
    private float sideSize;
    private Vector3 horizontalVector;
    private Vector3 verticalVector;
    private Vector3 localUp;
    private Vector3 startPoint;
    private Vector3 axisX;
    private Vector3 axisY;
    private PlanetOctahedronModel.TriangleFace triangleFace;
    private PlanetOctahedronModel.Face face;
    public bool Sphere = false;
    public bool DrawTriangleFaceCenterPoint = false;

    
    public PlanetOctahedronModelFace(Mesh mesh, int resolution, PlanetOctahedronModel.Face face, float sideSize)
    {
        this.mesh = mesh;
        this.resolution = resolution;
        this.face = face;
        triangleFace = PlanetOctahedronModel.TriangleFace.SelectFace(face);
        this.sideSize = sideSize;
        localUp = triangleFace.direction;
        axisY = triangleFace.axisY;
        axisX = Vector3.Cross(localUp, axisY) / 2f;
    }

    public void DrawCenterOfGavityOfSingleTriangleFace()
    {
        if (DrawTriangleFaceCenterPoint)
        {
            for(int i = 0; i < triangles.Length; i += 3)
            {
                Gizmos.color = Color.red;
                Gizmos.DrawSphere(
                    CenterOfGravityOfSingleTriangleFace(
                        vertices[triangles[i]],
                        vertices[triangles[i + 2]],
                        vertices[triangles[i + 1]])
                    , 0.01f / resolution);
            }
        }
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
        horizontalVector = (axisX * sideSize / (rowsOfVertices - 1));
        verticalVector = SelectVerticalVector(face);

        startPoint = triangleFace.leftDownVertices * sideSize;
        for (int vertical = rows; vertical > 0; vertical--)
        {
            for (int horizontal = 0; horizontal < vertical; horizontal++)
            {
                Vector3 verticesPoint = startPoint + horizontalVector * horizontal;
                Vector3 pointOnSphere = verticesPoint.normalized * sideSize;

                if (Sphere)
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
            int trianglesInRow = TrianglesInRow(vertical);
            for (int horizontal = 0; horizontal < trianglesInRow; horizontal++)
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
    private int TrianglesInRow(int verticalRowIndex)
    {
        return rowsOfVertices - verticalRowIndex - 1;
    }
    private void UpdateMesh(Mesh mesh)
    {
        mesh.Clear();
        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.RecalculateNormals();
    }

    private Vector3 SelectVerticalVector(PlanetOctahedronModel.Face face)
    {
        switch (face)
        {
            case PlanetOctahedronModel.Face.ForwardUp:
                return
                   new Vector3(
                        (axisY.z / 2f / (rowsOfVertices - 1) * sideSize),
                        CalculateHeight(sideSize) / (rowsOfVertices - 1),
                        (axisY.z / 2f / (rowsOfVertices - 1) * sideSize));
            case PlanetOctahedronModel.Face.BackUp:
                return
                    new Vector3(
                        (axisY.z / 2f / (rowsOfVertices - 1) * sideSize),
                        CalculateHeight(sideSize) / (rowsOfVertices - 1),
                        (axisY.z / 2f / (rowsOfVertices - 1)) * sideSize);
            case PlanetOctahedronModel.Face.LeftUp:
                return
                    new Vector3(
                        (axisY.x / 2f / (rowsOfVertices - 1) * sideSize),
                        CalculateHeight(sideSize) / (rowsOfVertices - 1),
                        (axisY.x / 2f / (rowsOfVertices - 1) * -1f) * sideSize);
            case PlanetOctahedronModel.Face.RightUp:
                return
                    new Vector3(
                        (axisY.x / 2f / (rowsOfVertices - 1)) * sideSize,
                        CalculateHeight(sideSize) / (rowsOfVertices - 1),
                        (axisY.x / 2f / (rowsOfVertices - 1) * -1f) * sideSize);
            case PlanetOctahedronModel.Face.ForwardDown:
                return
                   new Vector3(
                       (axisY.z / 2f / (rowsOfVertices - 1) * -1f) * sideSize,
                       CalculateHeight(sideSize) / (rowsOfVertices - 1) * -1f,
                       (axisY.z / 2f / (rowsOfVertices - 1)) * sideSize);
            case PlanetOctahedronModel.Face.BackDown:
                return
                new Vector3(
                       (axisY.z / 2f / (rowsOfVertices - 1) * -1f) * sideSize,
                       CalculateHeight(sideSize) / (rowsOfVertices - 1) * -1f,
                       (axisY.z / 2f / (rowsOfVertices - 1)) * sideSize);
            case PlanetOctahedronModel.Face.LeftDown:
                return
                    new Vector3(
                        (axisY.x / 2f / (rowsOfVertices - 1)) * sideSize,
                        CalculateHeight(sideSize) / (rowsOfVertices - 1) * -1f,
                        (axisY.x / 2f / (rowsOfVertices - 1)) * sideSize);
            case PlanetOctahedronModel.Face.RightDown:
                return
                    new Vector3(
                        (axisY.x / 2f / (rowsOfVertices - 1)) * sideSize,
                        CalculateHeight(sideSize) / (rowsOfVertices - 1) * -1f,
                        (axisY.x / 2f / (rowsOfVertices - 1)) * sideSize);
        }
        return Vector3.zero;
    }

    private float CalculateHeight(float sideLength)
    {
        return (Mathf.Sqrt(3f) * sideLength) / 2f;
    }

    private int SumOfConsecutiveNaturalNumbers(int lastNumber)
    {
        return lastNumber * (lastNumber + 1) / 2;
    }
    
    private Vector3 CenterOfGravityOfSingleTriangleFace(Vector3 leftDownVertice, Vector3 rightDownVertice, Vector3 upVertice)
    {
        Vector3 centerOfBottom = Vector3.Lerp(leftDownVertice, rightDownVertice, 0.5f);
        return Vector3.Lerp(centerOfBottom, upVertice, 0.3333f);
    }
}
