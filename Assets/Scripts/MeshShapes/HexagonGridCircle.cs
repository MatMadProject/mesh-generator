using System;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]
public class HexagonGridCircle : MonoBehaviour
{
    private Mesh mesh;
    [SerializeField]
    private bool UpdateShape = false;
    [SerializeField]
    private Vector3 startPoint = Vector3.zero;
    [SerializeField]
    private int gridRadius = 1;
    [SerializeField]
    private float sideSize = 1f;
    [SerializeField]
    private float tileMargin = 0f;
    [SerializeField]
    private Vector3[] vertices = { };
    private int[] triangles = { };
    private int addIndexInArray = 0;
    private readonly List<Hexagon> createdHexagons = new();
    private readonly List<Vector3> centerPointOfCreatedHexagons = new();
    void Start()
    {
        mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = mesh;

        if (tileMargin == 0)
            CreateMesh();
        else
            CreateMeshWithMargin();

        UpdateMesh();
    }


    // Update is called once per frame
    void Update()
    {
        if (UpdateShape)
        {
            ClearMeshTrainglesAndVertices();

            if (tileMargin == 0)
                CreateMesh();
            else
                CreateMeshWithMargin();

            UpdateMesh();

            UpdateShape = false;
        }
    }

    private void CreateMeshWithMargin()
    {
        CreateHexagonsWithMargin();
    }

    private void CreateMesh()
    {
        CreateHexagons();
    }

    //Tworzy p³ytki hexagonalne wokó³ œrodkowej p³ytki hexagonalne.
    //Kolejnoœæ tworzenia jest zgodna z ruchem wskazówek zegara, zaczynaj¹c od godz. 12
    private void CreateHexagons()
    {
        CreateCenterHexagon();
        int rowsHexagonToCreate = gridRadius - 1;

        for(int i = 0; i < rowsHexagonToCreate; i++)
        {
            List<Hexagon> tmpCreatedHexagons = new(createdHexagons.ToArray());
            foreach(Hexagon hexagon in tmpCreatedHexagons)
                CreateOuterHexagons(hexagon);
        }
    }

    private void CreateHexagonsWithMargin()
    {
        CreateCenterHexagon();
        int rowsHexagonToCreate = gridRadius - 1;

        for (int i = 0; i < rowsHexagonToCreate; i++)
        {
            List<Hexagon> tmpCreatedHexagons = new(createdHexagons.ToArray());
            foreach (Hexagon hexagon in tmpCreatedHexagons)
                CreateOuterHexagons(hexagon);
        }
    }

    private void CreateCenterHexagon()
    {
        AddSingleHexagon(startPoint);
    }

    private void CreateOuterHexagons(Hexagon centerHexagon)
    {
        int outerHexagons = 6;
        int outerHexagonsCounter = 1;
        Vector3 startPointOfMesh = Vector3.zero;
        while (outerHexagonsCounter <= outerHexagons)
        {
            switch (outerHexagonsCounter)
            {
                case 1:
                    startPointOfMesh = new Vector3(
                      centerHexagon.CenterPoint.x,
                      centerHexagon.CenterPoint.y,
                      centerHexagon.CenterPoint.z + (4/3f * centerHexagon.Height));
                    break;
                case 2:
                    startPointOfMesh = new Vector3(
                      centerHexagon.CenterPoint.x + (1.5f * sideSize),
                      centerHexagon.CenterPoint.y,
                      centerHexagon.CenterPoint.z + 1 / 3f * centerHexagon.Height);
                    break;
                case 3:
                    startPointOfMesh = new Vector3(
                      centerHexagon.CenterPoint.x + (1.5f * sideSize),
                      centerHexagon.CenterPoint.y,
                      centerHexagon.CenterPoint.z - 5/3f * centerHexagon.Height);
                    break;
                case 4:
                    startPointOfMesh = new Vector3(
                      centerHexagon.CenterPoint.x,
                      centerHexagon.CenterPoint.y,
                      centerHexagon.CenterPoint.z - (8/3f * centerHexagon.Height));
                    break;
                case 5:
                    startPointOfMesh = new Vector3(
                      centerHexagon.CenterPoint.x - (1.5f * sideSize),
                      centerHexagon.CenterPoint.y,
                      centerHexagon.CenterPoint.z - 5 / 3f * centerHexagon.Height);
                    break;
                case 6:
                    startPointOfMesh = new Vector3(
                      centerHexagon.CenterPoint.x - (1.5f * sideSize),
                      centerHexagon.CenterPoint.y,
                      centerHexagon.CenterPoint.z + 1 / 3f * centerHexagon.Height);
                    break;
            }
            AddSingleHexagon(startPointOfMesh);
            outerHexagonsCounter++;
        }
    }

    private void CreateOuterHexagonsWithMargin(Hexagon centerHexagon)
    {
        int outerHexagons = 6;
        int outerHexagonsCounter = 1;
        Vector3 startPointOfMesh = Vector3.zero;
        while (outerHexagonsCounter <= outerHexagons)
        {
            switch (outerHexagonsCounter)
            {
                case 1:
                    startPointOfMesh = new Vector3(
                      centerHexagon.CenterPoint.x,
                      centerHexagon.CenterPoint.y,
                      centerHexagon.CenterPoint.z + (4 / 3f * centerHexagon.Height));
                    break;
                case 2:
                    startPointOfMesh = new Vector3(
                      centerHexagon.CenterPoint.x + (1.5f * sideSize),
                      centerHexagon.CenterPoint.y,
                      centerHexagon.CenterPoint.z + 1 / 3f * centerHexagon.Height);
                    break;
                case 3:
                    startPointOfMesh = new Vector3(
                      centerHexagon.CenterPoint.x + (1.5f * sideSize),
                      centerHexagon.CenterPoint.y,
                      centerHexagon.CenterPoint.z - 5 / 3f * centerHexagon.Height);
                    break;
                case 4:
                    startPointOfMesh = new Vector3(
                      centerHexagon.CenterPoint.x,
                      centerHexagon.CenterPoint.y,
                      centerHexagon.CenterPoint.z - (8 / 3f * centerHexagon.Height));
                    break;
                case 5:
                    startPointOfMesh = new Vector3(
                      centerHexagon.CenterPoint.x - (1.5f * sideSize),
                      centerHexagon.CenterPoint.y,
                      centerHexagon.CenterPoint.z - 5 / 3f * centerHexagon.Height);
                    break;
                case 6:
                    startPointOfMesh = new Vector3(
                      centerHexagon.CenterPoint.x - (1.5f * sideSize),
                      centerHexagon.CenterPoint.y,
                      centerHexagon.CenterPoint.z + 1 / 3f * centerHexagon.Height);
                    break;
            }
            AddSingleHexagon(startPointOfMesh);
            outerHexagonsCounter++;
        }
    }
    private void UpdateMesh()
    {
        mesh.Clear();
        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.RecalculateNormals();
    }

    private void ClearMeshTrainglesAndVertices()
    {
        vertices = new Vector3 [] {};
        triangles = new int[] {};
        createdHexagons.Clear();
        centerPointOfCreatedHexagons.Clear();
        addIndexInArray = 0;
    }

    private void AddSingleHexagon(Vector3 centerPoint)
    {
        Hexagon hexagon = new Hexagon(centerPoint, sideSize);
        if (!createdHexagons.Contains(hexagon))
        {
            hexagon.UpdateTraingles(addIndexInArray);
            AddVertices(hexagon);
            AddTriangles(hexagon);
            createdHexagons.Add(hexagon);
            centerPointOfCreatedHexagons.Add(centerPoint);
            addIndexInArray++;
        }  
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
