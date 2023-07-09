using System.Collections.Generic;
using UnityEngine;


public class TreeTileManager : MonoBehaviour
{
    public static TreeTileManager Instance;
    [SerializeField]
    private List<GameObject> trees;
    void Awake()
    {
        if (Instance == null)
            Instance = this;
    }

    public GameObject GetFirstTree()
    {
        return trees[0];
    }

    public GameObject RandomTree()
    {
        int randomIndex = Mathf.RoundToInt(Random.Range(0, trees.Count));
        return trees[randomIndex];
    }

    public GameObject SpawnTree(Vector3 spawnPosition, Transform parent)
    {
        return GameObject.Instantiate(GetFirstTree(), 
            spawnPosition, 
            Quaternion.FromToRotation(Vector3.up,spawnPosition), 
            parent);
    }

    public GameObject SpawnRandomTree(Vector3 spawnPosition, Transform parent)
    {
        return GameObject.Instantiate(RandomTree(),
            spawnPosition,
            Quaternion.FromToRotation(Vector3.up, spawnPosition),
            parent);
    }

    public GameObject SpawnRandomTree(Vector3 spawnPosition, Transform parent, Vector3 scale)
    {
        GameObject tree = GameObject.Instantiate(RandomTree(),
            spawnPosition,
            Quaternion.FromToRotation(Vector3.up, spawnPosition),
            parent);

        tree.transform.localScale = scale;
        tree.AddComponent<MeshCollider>();
        return tree;
    }

    public GameObject SpawnTree(Vector3 spawnPosition, Quaternion quaternion, Transform parent)
    {
        return GameObject.Instantiate(GetFirstTree(), spawnPosition, quaternion, parent);
    }
}
