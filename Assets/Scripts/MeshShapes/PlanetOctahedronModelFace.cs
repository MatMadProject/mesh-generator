using System;
using UnityEngine;


public class PlanetOctahedronModelFace
{
    readonly Mesh mesh;
    private GameObject parent;
    private int resolution;
    private Vector3[] vertices;
    private int[] triangles;
    private int rowsOfVertices;
    private int hexagonTilesValue;
    private int rowsOfTriangels;
    private Vector3[] centerPointsOfTriangels;
    private float sideSize;
    private Vector3 horizontalVector;
    private Vector3 verticalVector;
    private Vector3 localUp;
    private Vector3 startPoint;
    private Vector3 axisX;
    private Vector3 axisY;
    private PlanetOctahedronModel.TriangleFace triangleFace;
    public PlanetOctahedronModel.Face face { get; private set; }
    private HexagonTile[] hexagonTiles;
    private MeshFilter[] hexagonTilesMeshFilters;
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

    public PlanetOctahedronModelFace(Mesh mesh, GameObject parent, int resolution, PlanetOctahedronModel.Face face, float sideSize)
    {
        this.mesh = mesh;
        this.parent = parent;
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
        centerPointsOfTriangels = new Vector3[trainglesValue];

        CreateVertices();
        CreateTriangles();
        AddCenterPointOfTriangles();

        UpdateMesh(mesh);
    }

    public void CreateGridMesh()
    {
        rowsOfVertices = Mathf.CeilToInt(Mathf.Pow(2, resolution - 1)) + 1;
        rowsOfTriangels = rowsOfVertices - 1;
        int trainglesValue = Mathf.CeilToInt(Mathf.Pow(4, resolution - 1));
        int verticesValue = SumOfConsecutiveNaturalNumbers(rowsOfVertices);

        vertices = new Vector3[verticesValue];
        triangles = new int[trainglesValue * 3];
        centerPointsOfTriangels = new Vector3[trainglesValue];

        CreateVertices();
        CreateTriangles();
        AddCenterPointOfTriangles();
        CalculateHexagonTilesValue();

        if(hexagonTiles.Length > 0)
        {

            CreateHexagonTiles();
            UpdateHexagonMesh();
        }
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
    private void AddCenterPointOfTriangles()
    {
        int index = 0;
        for (int i = 0; i < triangles.Length; i += 3)
        {
            centerPointsOfTriangels[index] =
            CenterOfGravityOfSingleTriangleFace(
                vertices[triangles[i]],
                vertices[triangles[i + 2]],
                vertices[triangles[i + 1]]);
            index++;
        }
    }
    public void CalculateHexagonTilesValue()
    {
        hexagonTilesValue = 0;
        HexagonTile[] old = null;
        //Debug.Log($"Resolution: {resolution}\t hexagonTiles != null: {hexagonTiles != null}");
        if (hexagonTiles != null && hexagonTiles.Length > 0)
        {
            old = new HexagonTile[hexagonTiles.Length];
            hexagonTiles.CopyTo(old, 0);
        }

        if (rowsOfVertices >= 5)
        {

            for (int rowOfTriangels = rowsOfTriangels; rowOfTriangels > 0; rowOfTriangels--)
            {
                int triangelsInRow = TriangelsInRow(rowOfTriangels);
                int hexagonsInRow = HexagonsInRow(triangelsInRow);
                hexagonTilesValue += hexagonsInRow;
                //Debug.Log($"RowOfTriangles: {rowOfTriangels}\t TrainglesInRow: {triangelsInRow}\t HexagonsInRow: {hexagonsInRow}");
            }
        }
        hexagonTiles = new HexagonTile[hexagonTilesValue];
        if(old != null)
            old.CopyTo(hexagonTiles, 0);
    }

    private int TriangelsInRow(int verticalTriangelsRowIndex)
    {
        return (2 * verticalTriangelsRowIndex) - 1;
    }
    private int HexagonsInRow(int triangelsInRow)
    {
        int hexagonsValue = 0;
        int triangelIndex = 1;

        while (triangelIndex + 2 < triangelsInRow)
        {
            hexagonsValue++;
            triangelIndex += 2;
        }
        return hexagonsValue;
    }
    public void CreateHexagonTiles()
    {
        int hexagonIndex = 0;
        int maxTriangelsIndexInRow = 0;
        int minTriangelsIndexInRow = 1;
        for (int row = rowsOfTriangels; row > 1; row--)
        {
            int triangelsInRow = TriangelsInRow(row);
            maxTriangelsIndexInRow += triangelsInRow;
            //Debug.Log($"Row:{row}\t TriangelsInRow:{triangelsInRow}\t MinIndex:{minTriangelsIndexInRow}\t MaxIndex:{maxTriangelsIndexInRow}");
            for (int triangelIndex = minTriangelsIndexInRow; triangelIndex < maxTriangelsIndexInRow; triangelIndex += 2)
            {
                if (triangelIndex + 2 < maxTriangelsIndexInRow)
                {
                    hexagonTiles[hexagonIndex] = GenerateHexagonTile(triangelIndex, triangelsInRow, centerPointsOfTriangels);
                    hexagonIndex++;
                }

            }
            minTriangelsIndexInRow += triangelsInRow;
        }
    }

    private void UpdateHexagonMesh()
    {
        MeshFilter[] old = null;
        
        if (hexagonTilesMeshFilters != null && hexagonTilesMeshFilters.Length > 0)
        {
            old = new MeshFilter[hexagonTilesMeshFilters.Length];
            hexagonTilesMeshFilters.CopyTo(old, 0);
        }
        //Debug.Log($"Resolution: {resolution}\t old: {old != null}");
        hexagonTilesMeshFilters = new MeshFilter[hexagonTiles.Length];
        if(old != null)
        {
            //Debug.Log($"old.Length: {old.Length}\t ");
            old.CopyTo(hexagonTilesMeshFilters, 0);
        }

        for (int i = 0; i < hexagonTiles.Length; i++)
        {
            if (hexagonTilesMeshFilters[i] == null)
            {
                GameObject tile = new GameObject($"HexagonTile[{i}]");
                tile.transform.parent = parent.transform;

                tile.AddComponent<MeshRenderer>().sharedMaterial = new Material(Shader.Find("Standard"));
                hexagonTilesMeshFilters[i] = tile.AddComponent<MeshFilter>();
                hexagonTilesMeshFilters[i].sharedMesh = new Mesh();
            }
            //Debug.Log($"hexagons[{i}] vertices: {hexagonTiles[i].vertices}");
            hexagonTiles[i].mesh = hexagonTilesMeshFilters[i].sharedMesh;
            hexagonTiles[i].UpdateMesh();
        }
    }

    private void CopyHexagonTileMeshFilters(MeshFilter[] oldTab, MeshFilter[] newTab)
    {
        Array.Resize(ref hexagonTilesMeshFilters, hexagonTiles.Length);
    }

    private HexagonTile GenerateHexagonTile(int triangelIndex, int triangelsInRow, Vector3[] centerPointsOfTriangels)
    {
        Vector3[] vertices = new Vector3[7];

        vertices[0] = centerPointsOfTriangels[triangelIndex];
        vertices[1] = centerPointsOfTriangels[triangelIndex + 1];
        vertices[2] = centerPointsOfTriangels[triangelIndex + 2];
        vertices[3] = centerPointsOfTriangels[triangelIndex + triangelsInRow - 1];
        vertices[4] = centerPointsOfTriangels[triangelIndex + triangelsInRow];
        vertices[5] = centerPointsOfTriangels[triangelIndex + triangelsInRow + 1];


        //Debug.Log($"Triangels ({triangelIndex},{triangelIndex + 1},{triangelIndex + 2}," +
        //    $"{triangelIndex + triangelsInRow - 1},{triangelIndex + triangelsInRow},{triangelIndex + triangelsInRow + 1})");
        return new HexagonTile(vertices, mesh);
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

    public void SetSize(float sideSize)
    {
        this.sideSize = sideSize;
    }

    public void SetResolution(int resolution)
    {
        this.resolution = resolution;
    }
}
