using System.Collections;
using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
public class HexagonTile : MonoBehaviour
{
    [SerializeField, HideInInspector]
    MeshFilter meshFilter;
    [SerializeField]
    private Vector3[] vertices;
    [SerializeField]
    private int[] triangles;
    [SerializeField]
    private Vector3 centerPoint;
    [SerializeField]
    private float sideSize = 1f;
    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}
