using System;
using UnityEngine;


public class PlanetOctahedronHexagonGrid2 : MonoBehaviour
{
    [Range(3, 7)]
    public int resolution = 1;
    private PlanetOctahedronHexagonGridFace2[] planetOctahedronHexagonGridFaces;
    private GameObject grid;
    [SerializeField]
    private float sideSize = 1f;
    [SerializeField]
    private int tilesCount = 0;
    [SerializeField]
    private Material material;
    public static bool FlatSurface = true;
    public static float MaximumDepth;
    public static float MaximumHeight;
    [SerializeField]
    private bool flatSurface = true;
    [SerializeField]
    private float maximumDepth = 0.01f;
    [SerializeField]
    private float maximumHeight = 0.05f;
    private HexagonTile[] hexagonTiles = new HexagonTile[0];
    [Header("Debug")]
    [Header("HexagonTileMesh")]
    [SerializeField]
    private Vector3[] vertices;
    [SerializeField]
    private int[] triangles;
    private void Start()
    {
        Initialize();
        //GenerateHexagonGridMesh();
        //GenerateHexagonTilesMesh();
        GenerateHexagonTiles();

        grid.transform.position = transform.position;
    }

    void Initialize()
    {
        planetOctahedronHexagonGridFaces = new PlanetOctahedronHexagonGridFace2[8];
        FlatSurface = flatSurface;
        MaximumDepth = maximumDepth;
        MaximumHeight = maximumHeight;

        OctahedronFace[] faces = {
            OctahedronFace.ForwardUp,
            OctahedronFace.BackUp,
            OctahedronFace.LeftUp,
            OctahedronFace.RightUp,
            OctahedronFace.ForwardDown,
            OctahedronFace.BackDown,
            OctahedronFace.LeftDown,
            OctahedronFace.RightDown
        };

        grid = new("GridOld");
        grid.transform.parent = transform;

        for (int i = 0; i < 8; i++)
        {
            if (planetOctahedronHexagonGridFaces[i] == null)
            {
                planetOctahedronHexagonGridFaces[i] = new PlanetOctahedronHexagonGridFace2(
                grid,
                material,
                resolution,
                faces[i],
                sideSize); 
            }
        }
    }
    private void OnDrawGizmos()
    {

    }

    private void GenerateHexagonGridMesh()
    {
        foreach (PlanetOctahedronHexagonGridFace2 face in planetOctahedronHexagonGridFaces)
        {
            if (face != null)
            {
                face.material = material;
                CreateHexagonTilesVertices(face);
                face.UpdateGridMesh();
            }
        }
        GenerateSquareMesh();
    }
    private void GenerateHexagonTilesMesh()
    {
        foreach (PlanetOctahedronHexagonGridFace2 face in planetOctahedronHexagonGridFaces)
            if (face != null)
            {
                CreateHexagonTilesVertices(face);
                AddHexagonTile(face.hexagonTiles);
            }

        PlanetOctahedronHexagonGridMesh gridMesh = new(hexagonTiles, material);
        gridMesh.UpdateVerticesAndTriangles();

        GameObject gameObject = gridMesh.GenerateMesh();
        gameObject.transform.parent = transform;
        gameObject.transform.position = transform.position;

        vertices = gameObject.GetComponent<MeshFilter>().sharedMesh.vertices;
        triangles = gameObject.GetComponent<MeshFilter>().sharedMesh.triangles;

        GenerateSquareMesh();
    }

    private void GenerateHexagonTiles()
    {
        foreach (PlanetOctahedronHexagonGridFace2 face in planetOctahedronHexagonGridFaces)
            if (face != null)
            {
                CreateHexagonTilesVertices(face);
                AddHexagonTile(face.hexagonTiles);
            }

        TileGrid gridMesh = new(hexagonTiles, material);

        GameObject gameObject = gridMesh.GenerateGrid();
        gameObject.transform.parent = transform;
        gameObject.transform.position = transform.position;

        GenerateSquareMesh();
    }

    private void CreateHexagonTilesVertices(PlanetOctahedronHexagonGridFace2 face)
    {
        face.CreateGrid();
        HorizontalBorderRow(face);
        VerticalBorderRow(face);
        CalculateTile(face);
    }
    private void HorizontalBorderRow(PlanetOctahedronHexagonGridFace2 face)
    {
        switch (face.Face)
        {
            case OctahedronFace.ForwardDown:
                face.GenerateDownHorizontalRowHexagonTiles(planetOctahedronHexagonGridFaces[0]);
                break;
            case OctahedronFace.BackDown:
                face.GenerateDownHorizontalRowHexagonTiles(planetOctahedronHexagonGridFaces[1]);
                break;
            case OctahedronFace.LeftDown:
                face.GenerateDownHorizontalRowHexagonTiles(planetOctahedronHexagonGridFaces[2]);
                break;
            case OctahedronFace.RightDown:
                face.GenerateDownHorizontalRowHexagonTiles(planetOctahedronHexagonGridFaces[3]);
                break;
        }
    }

    private void VerticalBorderRow(PlanetOctahedronHexagonGridFace2 face)
    {
        switch (face.Face)
        {
            case OctahedronFace.ForwardUp:
                face.GenerateVerticalRowHexagonTiles(planetOctahedronHexagonGridFaces[3], planetOctahedronHexagonGridFaces[2]);
                break;
            case OctahedronFace.BackUp:
                face.GenerateVerticalRowHexagonTiles(planetOctahedronHexagonGridFaces[2], planetOctahedronHexagonGridFaces[3]);
                break;
            case OctahedronFace.ForwardDown:
                face.GenerateVerticalRowHexagonTiles(planetOctahedronHexagonGridFaces[6], planetOctahedronHexagonGridFaces[7]);
                break;
            case OctahedronFace.BackDown:
                face.GenerateVerticalRowHexagonTiles(planetOctahedronHexagonGridFaces[7], planetOctahedronHexagonGridFaces[6]);
                break;
        }
    }

    private void CalculateTile(PlanetOctahedronHexagonGridFace2 face)
    {
        tilesCount += face.HexagonTileCount();
    }

    private void GenerateSquareMesh()
    {
        GameObject squareMesh = new GameObject("SquareMesh");
        squareMesh.transform.parent = transform;
        squareMesh.transform.position = transform.position;
        squareMesh.AddComponent<MeshRenderer>().sharedMaterial = material;
        MeshCollider meshCollider = squareMesh.AddComponent<MeshCollider>();
        

        MeshFilter meshFilter = squareMesh.AddComponent<MeshFilter>();
        Mesh mesh = meshFilter.sharedMesh = new Mesh();
        meshCollider.sharedMesh = mesh;
        //meshCollider.convex = true;
        Vector3[] vertices = new Vector3[24];

        vertices[0] = planetOctahedronHexagonGridFaces[1].CenterPointOfUpperTriangel();
        vertices[1] = planetOctahedronHexagonGridFaces[2].CenterPointOfUpperTriangel();
        vertices[2] = planetOctahedronHexagonGridFaces[3].CenterPointOfUpperTriangel();
        vertices[3] = planetOctahedronHexagonGridFaces[0].CenterPointOfUpperTriangel();

        vertices[4] = planetOctahedronHexagonGridFaces[5].CenterPointOfUpperTriangel();
        vertices[5] = planetOctahedronHexagonGridFaces[6].CenterPointOfUpperTriangel();
        vertices[6] = planetOctahedronHexagonGridFaces[7].CenterPointOfUpperTriangel();
        vertices[7] = planetOctahedronHexagonGridFaces[4].CenterPointOfUpperTriangel();

        vertices[8] = planetOctahedronHexagonGridFaces[7].CenterPointOfLeftDownTriangel();
        vertices[9] = planetOctahedronHexagonGridFaces[3].CenterPointOfRightDownTriangel();
        vertices[10] = planetOctahedronHexagonGridFaces[0].CenterPointOfLeftDownTriangel();
        vertices[11] = planetOctahedronHexagonGridFaces[4].CenterPointOfRightDownTriangel();

        vertices[12] = planetOctahedronHexagonGridFaces[4].CenterPointOfLeftDownTriangel();
        vertices[13] = planetOctahedronHexagonGridFaces[0].CenterPointOfRightDownTriangel();
        vertices[14] = planetOctahedronHexagonGridFaces[2].CenterPointOfLeftDownTriangel();
        vertices[15] = planetOctahedronHexagonGridFaces[6].CenterPointOfRightDownTriangel();

        vertices[16] = planetOctahedronHexagonGridFaces[6].CenterPointOfLeftDownTriangel();
        vertices[17] = planetOctahedronHexagonGridFaces[2].CenterPointOfRightDownTriangel();
        vertices[18] = planetOctahedronHexagonGridFaces[1].CenterPointOfLeftDownTriangel();
        vertices[19] = planetOctahedronHexagonGridFaces[5].CenterPointOfRightDownTriangel();

        vertices[20] = planetOctahedronHexagonGridFaces[5].CenterPointOfLeftDownTriangel();
        vertices[21] = planetOctahedronHexagonGridFaces[1].CenterPointOfRightDownTriangel();
        vertices[22] = planetOctahedronHexagonGridFaces[3].CenterPointOfLeftDownTriangel();
        vertices[23] = planetOctahedronHexagonGridFaces[7].CenterPointOfRightDownTriangel();

        int[] triangels = new int[]
        {
            0,1,2,
            3,2,1,
            6,5,4,
            5,6,7,
            8,9,10,
            8,10,11,
            12,13,14,
            12,14,15,
            16,17,18,
            16,18,19,
            20,21,22,
            20,22,23
        };

        mesh.Clear();
        mesh.vertices = vertices;
        mesh.triangles = triangels;
        mesh.RecalculateNormals();
    }

    private void AddHexagonTile(HexagonTile[] hexagonTiles)
    {
        int oldTabLength = this.hexagonTiles.Length;
        Array.Resize(ref this.hexagonTiles, this.hexagonTiles.Length + hexagonTiles.Length);
        hexagonTiles.CopyTo(this.hexagonTiles, oldTabLength);
    }
}
