using System.Collections.Generic;
using UnityEngine;


public class MaterialTileManager : MonoBehaviour
{
    public static MaterialTileManager Instance;
    [SerializeField]
    private List<Material> materials;
    void Start()
    {
        if (Instance == null)
            Instance = this;
    }

    public Material RandomMaterial()
    {
        int randomIndex = Mathf.RoundToInt(Random.Range(0, materials.Count));
        return materials[randomIndex];
    }
}
