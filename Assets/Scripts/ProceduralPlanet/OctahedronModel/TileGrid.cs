using UnityEngine;

public class TileGrid
{
    private readonly HexagonTile[] hexagonTiles;
    private readonly Material material;

    public TileGrid(HexagonTile[] hexagonTiles, Material material)
    {
        this.hexagonTiles = hexagonTiles;
        this.material = material;
    }

    public GameObject GenerateGrid()
    {
        GameObject gameObject = new GameObject("Grid");

        for (int i = 0; i < hexagonTiles.Length; i++)
        {
            GameObject tile = new($"HexagonTile[{i}]");
            tile.transform.parent = gameObject.transform;
            tile.AddComponent<MeshRenderer>().sharedMaterial = MaterialTileManager.Instance.RandomMaterial();
            MeshCollider meshCollider = tile.AddComponent<MeshCollider>();
            MeshFilter meshFilter = tile.AddComponent<MeshFilter>();
            Mesh mesh = meshFilter.sharedMesh = new Mesh();
            
            hexagonTiles[i].mesh = meshFilter.sharedMesh;
            hexagonTiles[i].UpdateMesh();

            meshCollider.sharedMesh = mesh;

            GameObject tree = GameObject.Instantiate(
                TreeTileManager.Instance.GetFirstTree(), hexagonTiles[i].centerPoint, tile.transform.rotation, tile.transform);
            
            //meshCollider.convex = true;
        }

        return gameObject;
    }
}
