﻿using System.Collections;
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

    public PlanetOctahedronModelFace(Mesh mesh, int resolution, PlanetOctahedronModel.Face face, float sideSize)
    {
        this.mesh = mesh;
        this.resolution = resolution;
        this.face = face;
        this.triangleFace = PlanetOctahedronModel.TriangleFace.SelectFace(face);
        this.sideSize = sideSize;
        localUp = triangleFace.direction;
        axisY = triangleFace.axisY;
        axisX = Vector3.Cross(localUp, axisY) / 2f;
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

        startPoint = triangleFace.leftDownVertices;
        for (int vertical = rows; vertical > 0; vertical--)
        {
            for (int horizontal = 0; horizontal < vertical; horizontal++)
            {
                Vector3 verticesPoint = startPoint + horizontalVector * horizontal;
                Vector3 pointOnSphere = verticesPoint.normalized;

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
                    verticalVector = new Vector3(
                        axisY.z / 2f / (rowsOfVertices - 1),
                        CalculateHeight(sideSize) / (rowsOfVertices - 1),
                        axisY.z / 2f / (rowsOfVertices - 1));
            case PlanetOctahedronModel.Face.BackUp:
                return
                    verticalVector = new Vector3(
                        axisY.z / 2f / (rowsOfVertices - 1),
                        CalculateHeight(sideSize) / (rowsOfVertices - 1),
                        axisY.z / 2f / (rowsOfVertices - 1));
            case PlanetOctahedronModel.Face.LeftUp:
                return
                    verticalVector = new Vector3(
                        axisY.x / 2f / (rowsOfVertices - 1),
                        CalculateHeight(sideSize) / (rowsOfVertices - 1),
                        axisY.x / 2f / (rowsOfVertices - 1) * -1f);
            case PlanetOctahedronModel.Face.RightUp:
                return
                    verticalVector = new Vector3(
                        axisY.x / 2f / (rowsOfVertices - 1),
                        CalculateHeight(sideSize) / (rowsOfVertices - 1),
                        axisY.x / 2f / (rowsOfVertices - 1) * -1f);
            case PlanetOctahedronModel.Face.ForwardDown:
                return
                   verticalVector = new Vector3(
                       axisY.z / 2f / (rowsOfVertices - 1) * -1f,
                       CalculateHeight(sideSize) / (rowsOfVertices - 1) * -1f,
                       axisY.z / 2f / (rowsOfVertices - 1));
            case PlanetOctahedronModel.Face.BackDown:
                return
                verticalVector = new Vector3(
                       axisY.z / 2f / (rowsOfVertices - 1) * -1f,
                       CalculateHeight(sideSize) / (rowsOfVertices - 1) * -1f,
                       axisY.z / 2f / (rowsOfVertices - 1));
            case PlanetOctahedronModel.Face.LeftDown:
                return
                    verticalVector = new Vector3(
                        axisY.x / 2f / (rowsOfVertices - 1),
                        CalculateHeight(sideSize) / (rowsOfVertices - 1) * -1f,
                        axisY.x / 2f / (rowsOfVertices - 1));
            case PlanetOctahedronModel.Face.RightDown:
                return
                    verticalVector = new Vector3(
                        axisY.x / 2f / (rowsOfVertices - 1),
                        CalculateHeight(sideSize) / (rowsOfVertices - 1) * -1f,
                        axisY.x / 2f / (rowsOfVertices - 1));
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
}