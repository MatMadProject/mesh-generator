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
    public PlanetOctahedronModel.Face Face { get; private set; }
    private HexagonTile[] hexagonTiles;
    private MeshFilter[] hexagonTilesMeshFilters;
    public Material material;
    public bool Sphere = true;
    public bool DrawTriangleFaceCenterPoint = false;

    
    public PlanetOctahedronModelFace(Mesh mesh, int resolution, PlanetOctahedronModel.Face face, float sideSize)
    {
        this.mesh = mesh;
        this.resolution = resolution;
        this.Face = face;
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
        this.Face = face;
        triangleFace = PlanetOctahedronModel.TriangleFace.SelectFace(face);
        this.sideSize = sideSize;
        localUp = triangleFace.direction;
        axisY = triangleFace.axisY;
        axisX = Vector3.Cross(localUp, axisY) / 2f;
        CreateGridData();
    }

    public PlanetOctahedronModelFace(GameObject parent, int resolution, PlanetOctahedronModel.Face face, float sideSize)
    {
        this.parent = parent;
        this.resolution = resolution;
        Face = face;
        triangleFace = PlanetOctahedronModel.TriangleFace.SelectFace(face);
        this.sideSize = sideSize;
        localUp = triangleFace.direction;
        axisY = triangleFace.axisY;
        axisX = Vector3.Cross(localUp, axisY) / 2f;
        CreateGridData();
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

    private void CreateGridData()
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
    }
    public void CreateGrid()
    {
        if(hexagonTiles.Length > 0)
            CreateHexagonTiles();
    }

    public Vector3[] LeftBorderHexagonTilesVertices()
    {
        Vector3[] hexagonTileVertices = new Vector3[TriangelsInRow(rowsOfTriangels)];
        int index = 0;
        int indexTabCenterPointOfTriangels = 0;
        int triangelsInRow;
        for (int i = rowsOfTriangels; i > 0; i--)
        {
            triangelsInRow = TriangelsInRow(i);
            if (i == rowsOfTriangels)
                hexagonTileVertices[index] = centerPointsOfTriangels[indexTabCenterPointOfTriangels];
            if(i != 1)
            {
                hexagonTileVertices[index + 1] = centerPointsOfTriangels[indexTabCenterPointOfTriangels + 1];
                hexagonTileVertices[index + 2] = centerPointsOfTriangels[indexTabCenterPointOfTriangels + triangelsInRow];
            }

            index += 2;
            indexTabCenterPointOfTriangels += triangelsInRow;
        }

        return hexagonTileVertices;
    }

    public Vector3[] RightBorderHexagonTilesVertices()
    {
        Vector3[] hexagonTileVertices = new Vector3[TriangelsInRow(rowsOfTriangels)];
        int index = 0;

        int triangelsInNextRow = TriangelsInRow(rowsOfTriangels - 1);
        int indexTabCenterPointOfTriangels = TriangelsInRow(rowsOfTriangels) - 1;
        for (int i = rowsOfTriangels; i > 1; i--)
        { 
            if (i == rowsOfTriangels)
                hexagonTileVertices[index] = centerPointsOfTriangels[indexTabCenterPointOfTriangels];
            if (i != 1)
            {
                hexagonTileVertices[index + 1] = centerPointsOfTriangels[indexTabCenterPointOfTriangels - 1];
                hexagonTileVertices[index + 2] = centerPointsOfTriangels[indexTabCenterPointOfTriangels + triangelsInNextRow];
            }


            index += 2;
            indexTabCenterPointOfTriangels += triangelsInNextRow;
            triangelsInNextRow = TriangelsInRow(i - 2);
        }

        return hexagonTileVertices;
    }

    public Vector3[] DownBorderHexagonTilesVertices()
    {
        Vector3[] hexagonTileVertices = new Vector3[TriangelsInRow(rowsOfTriangels)];
        
        int triangelsInRow = TriangelsInRow(rowsOfTriangels);
        for (int i = 0; i < triangelsInRow ; i++)
        {
            hexagonTileVertices[i] = centerPointsOfTriangels[i];
        }

        return hexagonTileVertices;
    }

    private void CreateVertices()
    {
        int rows = rowsOfVertices;
        int index = 0;
        horizontalVector = (axisX * sideSize / (rowsOfVertices - 1));
        verticalVector = SelectVerticalVector(Face);

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

        if (rowsOfVertices >= 5)
        {

            for (int rowOfTriangels = rowsOfTriangels; rowOfTriangels > 0; rowOfTriangels--)
            {
                int triangelsInRow = TriangelsInRow(rowOfTriangels);
                int hexagonsInRow = HexagonsInRow(triangelsInRow);
                hexagonTilesValue += hexagonsInRow;

            }
        }
        hexagonTiles = new HexagonTile[hexagonTilesValue];
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
            for (int triangelIndex = minTriangelsIndexInRow; triangelIndex < maxTriangelsIndexInRow; triangelIndex += 2)
            {
                if (triangelIndex + 2 < maxTriangelsIndexInRow)
                {
                    hexagonTiles[hexagonIndex] = GenerateCenterHexagonTile(triangelIndex, triangelsInRow, centerPointsOfTriangels);
                    hexagonIndex++;
                }
            }
            minTriangelsIndexInRow += triangelsInRow;
        }
    }

    public void UpdateGridMesh()
    {
        hexagonTilesMeshFilters = new MeshFilter[hexagonTiles.Length];
        for (int i = 0; i < hexagonTiles.Length; i++)
        {
            if (hexagonTilesMeshFilters[i] == null)
            {
                GameObject tile = new GameObject($"HexagonTile[{i}]");
                tile.transform.parent = parent.transform;

                tile.AddComponent<MeshRenderer>().sharedMaterial = material;//new Material(Shader.Find("Standard"));
                hexagonTilesMeshFilters[i] = tile.AddComponent<MeshFilter>();
                hexagonTilesMeshFilters[i].sharedMesh = new Mesh();
            }
            //Debug.Log($"hexagons[{i}] vertices: {hexagonTiles[i].vertices}");
            hexagonTiles[i].mesh = hexagonTilesMeshFilters[i].sharedMesh;
            hexagonTiles[i].UpdateMesh();
        }
    }

    private HexagonTile GenerateCenterHexagonTile(int triangelIndex, int triangelsInRow, Vector3[] centerPointsOfTriangels)
    {
        Vector3[] vertices = new Vector3[7];

        vertices[0] = centerPointsOfTriangels[triangelIndex];
        vertices[1] = centerPointsOfTriangels[triangelIndex + 1];
        vertices[2] = centerPointsOfTriangels[triangelIndex + 2];
        vertices[3] = centerPointsOfTriangels[triangelIndex + triangelsInRow - 1];
        vertices[4] = centerPointsOfTriangels[triangelIndex + triangelsInRow];
        vertices[5] = centerPointsOfTriangels[triangelIndex + triangelsInRow + 1];

        //return new HexagonTile(vertices, mesh);
        return new HexagonTile(vertices);
    }

    public void GenerateDownHorizontalRowHexagonTiles(PlanetOctahedronModelFace upperFace)
    {
        Vector3[] downFaceVertices = DownBorderHexagonTilesVertices();
        Vector3[] upperFaceVertices = upperFace.DownBorderHexagonTilesVertices();
        Array.Reverse(upperFaceVertices);
        Vector3[] vertices = new Vector3[7];
        //Debug.Log($"{Face}:\t downFaceVertices:{downFaceVertices.Length}\t");
        int tabLength = downFaceVertices.Length;
        for (int i = 0; i < tabLength - 1; i += 2)
        {
            vertices[0] = downFaceVertices[i + 2];
            vertices[1] = downFaceVertices[i + 1];
            vertices[2] = downFaceVertices[i];
            vertices[3] = upperFaceVertices[i + 2];
            vertices[4] = upperFaceVertices[i + 1];
            vertices[5] = upperFaceVertices[i];

            Vector3[] copy = new Vector3[7];
            vertices.CopyTo(copy, 0);
            AddHexagonTile(new HexagonTile(copy));
            //AddHexagonTile(new HexagonTile(copy, mesh));
        }
    }

    public void GenerateVerticalRowHexagonTiles(PlanetOctahedronModelFace leftFace, PlanetOctahedronModelFace rightFace)
    {
        Vector3[] thisLeftBorderTilesVertices = LeftBorderHexagonTilesVertices();
        Vector3[] thisRightBorderTilesVertices = RightBorderHexagonTilesVertices();
        Vector3[] leftBorderTilesVertices = leftFace.RightBorderHexagonTilesVertices();
        Vector3[] rightBorderTilesVertices = rightFace.LeftBorderHexagonTilesVertices();

        Vector3[] vertices = new Vector3[7];

        int tabLength = thisLeftBorderTilesVertices.Length;
        //Left border
        for (int i = 0; i < tabLength - 1; i += 2)
        {
            vertices[0] = leftBorderTilesVertices[i];
            vertices[1] = thisLeftBorderTilesVertices[i];
            vertices[2] = thisLeftBorderTilesVertices[i + 1];
            vertices[3] = leftBorderTilesVertices[i + 1];
            vertices[4] = leftBorderTilesVertices[i + 2];
            vertices[5] = thisLeftBorderTilesVertices[i + 2];

            Vector3[] copy = new Vector3[7];
            vertices.CopyTo(copy, 0);

            //AddHexagonTile(new HexagonTile(copy, mesh));
            AddHexagonTile(new HexagonTile(copy));
        }
        //Right border
        for (int i = 0; i < tabLength - 1; i += 2)
        {
            vertices[0] = thisRightBorderTilesVertices[i];
            vertices[1] = rightBorderTilesVertices[i];         
            vertices[2] = rightBorderTilesVertices[i + 1];
            vertices[3] = thisRightBorderTilesVertices[i + 1];
            vertices[4] = thisRightBorderTilesVertices[i + 2];
            vertices[5] = rightBorderTilesVertices[i + 2];

            Vector3[] copy = new Vector3[7];
            vertices.CopyTo(copy, 0);

            //AddHexagonTile(new HexagonTile(copy, mesh));
            AddHexagonTile(new HexagonTile(copy));
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
            default:
                return Vector3.zero;
        } 
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
        return Vector3.Lerp(centerOfBottom, upVertice, 1f/3f);
    }

    public void SetSize(float sideSize)
    {
        this.sideSize = sideSize;
    }

    public void SetResolution(int resolution)
    {
        this.resolution = resolution;
    }

    private void AddHexagonTile(HexagonTile hexagonTile)
    {
        Array.Resize(ref hexagonTiles, hexagonTiles.Length + 1);
        hexagonTiles[hexagonTiles.Length - 1] = hexagonTile;
    }

    public int HexagonTileCount()
    {
        return hexagonTiles.Length;
    }
}
