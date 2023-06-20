using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Octahedron : MonoBehaviour
{
    [Range(1, 10)]
    public int resolution = 1;
    [SerializeField, HideInInspector]
    private MeshFilter[] meshFilters;
    private OctahedronFace[] octahedronFaces;
    [SerializeField]
    private Vector3 centerPoint = new Vector3(0f,0f,0f);
    [SerializeField]
    private float sideSize = 1f;
    private void OnValidate()
    {
        Initialize();
        GenerateMesh();
        centerPoint = transform.position;
    }

    void Initialize()
    {
        if (meshFilters == null || meshFilters.Length == 0)
        {
            meshFilters = new MeshFilter[8];
        }
        octahedronFaces = new OctahedronFace[8];

        Vector3[] directions = {
            Vector3.forward + Vector3.up,
            Vector3.back + Vector3.up,
            Vector3.left + Vector3.up,
            Vector3.right + Vector3.up,
            Vector3.forward + Vector3.down,
            Vector3.back + Vector3.down,
            Vector3.left + Vector3.down,
            Vector3.right + Vector3.down
        };

        for (int i = 0; i < 2; i++)
        {
            //Debug.Log($"directions{i}: {directions[i]}");
            if (meshFilters[i] == null)
            {
                GameObject meshObj = new GameObject($"mesh_{i}");
                meshObj.transform.parent = transform;

                meshObj.AddComponent<MeshRenderer>().sharedMaterial = new Material(Shader.Find("Standard"));
                meshFilters[i] = meshObj.AddComponent<MeshFilter>();
                meshFilters[i].sharedMesh = new Mesh();
            }

           octahedronFaces[i] = new OctahedronFace(meshFilters[i].sharedMesh, resolution, directions[i], sideSize);
        }
    }

    void GenerateMesh()
    {
        foreach (OctahedronFace face in octahedronFaces)
        {
            if(face != null)
                face.CreateMesh();
        }
    }

    public enum Face
    {
        ForwardUp,
        BackUp,
        LeftUp,
        RightUp,
        ForwardDown,
        BackDown,
        LeftDown,
        RightDown,
    }
}
