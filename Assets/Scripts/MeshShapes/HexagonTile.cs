﻿
using UnityEngine;

public class HexagonTile
{
    public Mesh mesh;
    public Vector3[] vertices;
    public int[] triangles = new int[]
    {
            0,6,1,
            1,6,2,
            2,6,5,
            5,6,4,
            4,6,3,
            3,6,0
    };
    private float sideLength;
    private float height;
    public Vector3 centerPoint;

    public HexagonTile(Vector3[] vertices)
    {
        this.vertices = vertices;
        centerPoint = Vector3.Lerp(vertices[0], vertices[5], 0.5f);
        vertices[6] = centerPoint;
        sideLength = Vector3.Distance(vertices[0], vertices[1]);
        height = (Mathf.Sqrt(3f) * sideLength) / 2f;
        mesh = new Mesh();
    }

    public HexagonTile(Vector3[] vertices, Mesh mesh)
    {
        this.mesh = mesh;
        this.vertices = vertices;
        centerPoint = Vector3.Lerp(vertices[0], vertices[5], 0.5f);
        vertices[6] = centerPoint;
        sideLength = Vector3.Distance(vertices[0], vertices[1]);
        height = (Mathf.Sqrt(3f) * sideLength) / 2f;
    }
    public void UpdateMesh()
    {
        mesh.Clear();
        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.RecalculateNormals();
    }
}

