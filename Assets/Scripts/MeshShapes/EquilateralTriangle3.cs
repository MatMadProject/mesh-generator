using System.Collections;
using UnityEngine;


[RequireComponent(typeof(MeshFilter))]
public class EquilateralTriangle3 : MonoBehaviour
{
    [Range(1, 8)]
    public int resolution = 2;
    [SerializeField, HideInInspector]
    MeshFilter meshFilter;
    [SerializeField, HideInInspector]
    MeshFilter[] hexagonsMeshFilters;
    [SerializeField]
    public int rowsOfVertices;
    [SerializeField]
    private int rowsOfTriangels;
    [SerializeField]
    private int hexagonTilesValue;
    [SerializeField]
    private Vector3[] vertices;
    [SerializeField]
    private int[] triangles;
    [SerializeField]
    private Hexagon[] hexagons;
    [SerializeField]
    private Vector3[] centerPointsOfTriangels;
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
    private Vector3 leftDowVertices;
    [SerializeField]
    private Vector3 axisX;
    [SerializeField]
    private Vector3 axisY;
    public PlanetOctahedronModel.Face OctahedronFace;
    private TriangleFace triangleFace;
    [Header("Boolean Flags")]
    [SerializeField]
    private bool Sphere = false;
    [SerializeField]
    private bool DrawGizmos = false;
    [SerializeField]
    private bool DrawHexagonsGrid = false;
    [SerializeField]
    private Material hexagonTileMaterial;

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
        if (DrawGizmos)
        {
            for (int i = 0; i < triangles.Length; i += 3)
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

    private Vector3 CenterOfGravityOfSingleTriangleFace(Vector3 leftDownVertice, Vector3 rightDownVertice, Vector3 upVertice)
    {
        Vector3 centerOfBottom = Vector3.Lerp(leftDownVertice, rightDownVertice, 0.5f);
        return Vector3.Lerp(centerOfBottom, upVertice, 1/3f);
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
    public void GenerateMesh(Mesh mesh, int resolution)
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
        CreateHexagonTiles();
        UpdateMesh(mesh);
        if (DrawHexagonsGrid && hexagonTilesValue > 0)
            UpdateHexagonMesh();

        
    }

    private void UpdateHexagonMesh()
    {
        if (hexagonsMeshFilters == null || hexagonsMeshFilters.Length == 0 || hexagonsMeshFilters.Length != hexagons.Length)
        {
            hexagonsMeshFilters = new MeshFilter[hexagons.Length];
        }

        for(int i = 0; i < hexagons.Length; i++)
        {
            if (hexagonsMeshFilters[i] == null)
            {
                GameObject tile = new GameObject($"HexagonTile[{i}]");
                tile.transform.parent = transform;

                tile.AddComponent<MeshRenderer>().sharedMaterial = hexagonTileMaterial;
                hexagonsMeshFilters[i] = tile.AddComponent<MeshFilter>();
                hexagonsMeshFilters[i].sharedMesh = new Mesh();
            }
            Debug.Log($"hexagons[{i}] vertices: {hexagons[i].vertices}");
            hexagons[i].mesh = hexagonsMeshFilters[i].sharedMesh;
            hexagons[i].UpdateMesh();
        }
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
                if(triangelIndex + 2 < maxTriangelsIndexInRow)
                {
                    hexagons[hexagonIndex] = GenerateHexagonTile(triangelIndex, triangelsInRow, centerPointsOfTriangels);
                    hexagonIndex++;
                }
                    
            }
            minTriangelsIndexInRow += triangelsInRow;
        }
    }

    private Hexagon GenerateHexagonTile(int triangelIndex, int triangelsInRow, Vector3[] centerPointsOfTriangels)
    {
        Vector3[] vertices = new Vector3[7];

        vertices[0] = centerPointsOfTriangels[triangelIndex];
        vertices[1] = centerPointsOfTriangels[triangelIndex + 1];
        vertices[2] = centerPointsOfTriangels[triangelIndex + 2];
        vertices[3] = centerPointsOfTriangels[triangelIndex + triangelsInRow - 1];
        vertices[4] = centerPointsOfTriangels[triangelIndex + triangelsInRow];
        vertices[5] = centerPointsOfTriangels[triangelIndex + triangelsInRow + 1];


        Debug.Log($"Triangels ({triangelIndex},{triangelIndex + 1},{triangelIndex + 2}," +
            $"{triangelIndex + triangelsInRow - 1},{triangelIndex + triangelsInRow},{triangelIndex + triangelsInRow + 1})");
        return new Hexagon(vertices);
    }

    public void CalculateHexagonTilesValue()
    {
        hexagonTilesValue = 0;
        if (rowsOfVertices < 5)
            return;

        for(int rowOfTriangels = rowsOfTriangels; rowOfTriangels > 0; rowOfTriangels--)
        {
            int triangelsInRow = TriangelsInRow(rowOfTriangels);
            int hexagonsInRow = HexagonsInRow(triangelsInRow);
            hexagonTilesValue += hexagonsInRow;
            //Debug.Log($"RowOfTriangles: {rowOfTriangels}\t TrainglesInRow: {triangelsInRow}\t HexagonsInRow: {hexagonsInRow}");
        }
        hexagons = new Hexagon[hexagonTilesValue];
    }

    public bool IsEvenRow(int rowIndex)
    {
        return rowIndex % 2 == 0;
    }
    private void CreateVertices()
    {
        int rows = rowsOfVertices;
        int index = 0;
        horizontalVector = (axisX * sideSize / (rowsOfVertices - 1));
        verticalVector = SelectVerticalVector(OctahedronFace);

        startPoint = triangleFace.leftDownVertices * sideSize;
        leftDowVertices = triangleFace.leftDownVertices * sideSize;
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
            int trianglesInRow = VerticesInRow(vertical);
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

    private int VerticesInRow(int verticalVerticesRowIndex)
    {
        return rowsOfVertices - verticalVerticesRowIndex - 1;
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

    private Vector3 SelectVerticalVector(PlanetOctahedronModel.Face face)
    {
        switch (face)
        {
            case PlanetOctahedronModel.Face.ForwardUp:
                return 
                    verticalVector = new Vector3(
                        (axisY.z / 2f / (rowsOfVertices - 1) * sideSize),
                        CalculateHeight(sideSize) / (rowsOfVertices - 1),
                        (axisY.z / 2f / (rowsOfVertices - 1) * sideSize));
            case PlanetOctahedronModel.Face.BackUp:
                return 
                    verticalVector = new Vector3(
                        (axisY.z / 2f / (rowsOfVertices - 1) * sideSize),
                        CalculateHeight(sideSize) / (rowsOfVertices - 1),
                        (axisY.z / 2f / (rowsOfVertices - 1))* sideSize);
            case PlanetOctahedronModel.Face.LeftUp:
                return
                    verticalVector = new Vector3(
                        (axisY.x / 2f / (rowsOfVertices - 1) * sideSize),
                        CalculateHeight(sideSize) / (rowsOfVertices - 1),
                        (axisY.x / 2f / (rowsOfVertices - 1) * -1f) * sideSize);
            case PlanetOctahedronModel.Face.RightUp:
                return
                    verticalVector = new Vector3(
                        (axisY.x / 2f / (rowsOfVertices - 1)) * sideSize,
                        CalculateHeight(sideSize) / (rowsOfVertices - 1),
                        (axisY.x / 2f / (rowsOfVertices - 1) * -1f) * sideSize);
            case PlanetOctahedronModel.Face.ForwardDown:
                return
                   verticalVector = new Vector3(
                       (axisY.z / 2f / (rowsOfVertices - 1) * -1f) * sideSize,
                       CalculateHeight(sideSize) / (rowsOfVertices - 1) * -1f,
                       (axisY.z / 2f / (rowsOfVertices - 1)) * sideSize);
            case PlanetOctahedronModel.Face.BackDown:
                return
                verticalVector = new Vector3(
                       (axisY.z / 2f / (rowsOfVertices - 1) * -1f) * sideSize,
                       CalculateHeight(sideSize) / (rowsOfVertices - 1) * -1f,
                       (axisY.z / 2f / (rowsOfVertices - 1)) * sideSize);
            case PlanetOctahedronModel.Face.LeftDown:
                return
                    verticalVector = new Vector3(
                        (axisY.x / 2f / (rowsOfVertices - 1)) * sideSize,
                        CalculateHeight(sideSize) / (rowsOfVertices - 1) * -1f,
                        (axisY.x / 2f / (rowsOfVertices - 1)) * sideSize);
            case PlanetOctahedronModel.Face.RightDown:
                return
                    verticalVector = new Vector3(
                        (axisY.x / 2f / (rowsOfVertices - 1)) * sideSize,
                        CalculateHeight(sideSize) / (rowsOfVertices - 1) * -1f,
                        (axisY.x / 2f / (rowsOfVertices - 1)) * sideSize);
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

        public static TriangleFace SelectFace(PlanetOctahedronModel.Face face)
        {
            switch (face)
            {
                case PlanetOctahedronModel.Face.ForwardUp:
                    return ForwardUp;
                case PlanetOctahedronModel.Face.BackUp:
                    return BackUp;
                case PlanetOctahedronModel.Face.LeftUp:
                    return LeftUp;
                case PlanetOctahedronModel.Face.RightUp:
                    return RightUp;
                case PlanetOctahedronModel.Face.ForwardDown:
                    return ForwardDown;
                case PlanetOctahedronModel.Face.BackDown:
                    return BackDown;
                case PlanetOctahedronModel.Face.LeftDown:
                    return LeftDown;
                case PlanetOctahedronModel.Face.RightDown:
                    return RightDown;
            }
            return null;
        }
    }

    public class Hexagon
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

        public Hexagon(Vector3[] vertices)
        {
            this.vertices = vertices;
            centerPoint = Vector3.Lerp(vertices[0], vertices[5], 0.5f);
            vertices[6] = centerPoint;
            sideLength = Vector3.Distance(vertices[0], vertices[1]);
            height = (Mathf.Sqrt(3f) * sideLength) / 2f;
        }

        public Hexagon(Vector3[] vertices, Mesh mesh)
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
}
