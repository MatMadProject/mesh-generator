using System.Collections.Generic;
using UnityEngine;


public class TreeTileManager : MonoBehaviour
{
    public static TreeTileManager Instance;
    [SerializeField]
    private List<GameObject> trees;
    void Start()
    {
        if (Instance == null)
            Instance = this;
    }

    public GameObject GetFirstTree()
    {
        return trees[0];
    }
}
