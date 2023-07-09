using System.Collections.Generic;
using UnityEngine;


public class MaterialTileManager : MonoBehaviour
{
    public static MaterialTileManager Instance;
    [SerializeField]
    private List<Material> materials;
    [SerializeField]
    private Material treeTile;
    void Awake()
    {
        if (Instance == null)
            Instance = this;
    }

    public Material RandomMaterial()
    {
        int randomIndex = Mathf.RoundToInt(Random.Range(0, materials.Count));
        return materials[randomIndex];
    }

    public Material TreeTile()
    {
        return treeTile;
    }
}
