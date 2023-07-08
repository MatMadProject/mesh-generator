using UnityEngine;


public class PlanetOctahedronHexagonGridMesh
{
    private HexagonTile[] hexagonTiles;
    private Material material;
    private Vector3[] vertices;
    private int[] triangles;

    public PlanetOctahedronHexagonGridMesh(HexagonTile[] hexagonTiles, Material material)
    {
        this.hexagonTiles = hexagonTiles;
        this.material = material;
        vertices = new Vector3[hexagonTiles.Length * HexagonTile.VerticesOffset];
        triangles = new int[hexagonTiles.Length * HexagonTile.TriangelsOffset];
    }

    public void UpdateVerticesAndTriangles()
    {
        int verticesIndex = 0;
        int trianglesIndex = 0;
        foreach(HexagonTile tile in hexagonTiles)
        {
            UpdateVertices(tile.vertices, verticesIndex);
            UpdateTriangles(tile.triangles, trianglesIndex, verticesIndex);
            verticesIndex += HexagonTile.VerticesOffset;
            trianglesIndex += HexagonTile.TriangelsOffset;
        }
    }

    private void UpdateVertices(Vector3 [] vertices, int startIndex)
    {
        for (int i = 0; i < vertices.Length; i++)
            this.vertices[startIndex + i] = vertices[i];
    }

    private void UpdateTriangles(int[] triangles, int startIndex, int verticesIndex)
    {
        for (int i = 0; i < triangles.Length; i++)
            this.triangles[startIndex + i] = triangles[i] + verticesIndex;    
    }

    public GameObject GenerateMesh()
    {
        GameObject gameObject = new GameObject("Mesh");
        gameObject.AddComponent<MeshRenderer>().sharedMaterial = material;
        MeshCollider meshCollider = gameObject.AddComponent<MeshCollider>();
        MeshFilter meshFilter = gameObject.AddComponent<MeshFilter>();
        Mesh mesh = meshFilter.sharedMesh = new Mesh();
        meshCollider.sharedMesh = mesh;
        meshCollider.convex = true;

        mesh.Clear();
        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.RecalculateNormals();

        return gameObject;
    }
}
