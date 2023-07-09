using UnityEngine;

public class TileGrid
{
    private readonly HexagonTile[] hexagonTiles;
    private readonly Material material;
    private readonly Transform parent;
    private float spawnObjectScale;

    public TileGrid(HexagonTile[] hexagonTiles, Material material, Transform parent, float spawnObjectScale)
    {
        this.hexagonTiles = hexagonTiles;
        this.material = material;
        this.parent = parent;
        this.spawnObjectScale = spawnObjectScale;
    }

    public GameObject GenerateGrid()
    {
        GameObject gameObject = new GameObject("Grid");
        gameObject.transform.parent = parent;

        for (int i = 0; i < hexagonTiles.Length; i++)
        {
            GameObject tile = new($"HexagonTile[{i}]");
            tile.transform.parent = gameObject.transform;
            if(hexagonTiles[i].tileType == TileType.Tree)
                tile.AddComponent<MeshRenderer>().sharedMaterial = MaterialTileManager.Instance.TreeTile();
            else
                tile.AddComponent<MeshRenderer>().sharedMaterial = MaterialTileManager.Instance.RandomMaterial();
            MeshCollider meshCollider = tile.AddComponent<MeshCollider>();
            MeshFilter meshFilter = tile.AddComponent<MeshFilter>();
            Mesh mesh = meshFilter.sharedMesh = new Mesh();
            
            hexagonTiles[i].mesh = meshFilter.sharedMesh;
            hexagonTiles[i].UpdateMesh();

            meshCollider.sharedMesh = mesh;

            if(hexagonTiles[i].tileType == TileType.Tree)
                TreeTileManager.Instance.SpawnRandomTree(
                hexagonTiles[i].centerPoint,
                tile.transform,
                Vector3.one * spawnObjectScale);
        }

        return gameObject;
    }
}
