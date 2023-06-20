using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Planet : MonoBehaviour
{
    //[Range(2, 256)]
    [Range(1, 10)]
    public int resolution = 10;

    [SerializeField, HideInInspector]
    MeshFilter[] meshFilters;
    TerrainFace[] terrainFaces;
    //public OctahedronFace[] octahedronFaces;

    private void OnValidate()
    {
        Initialize();
        GenerateMesh();
    }

    void Initialize()
    {
        if (meshFilters == null || meshFilters.Length == 0)
        {
            meshFilters = new MeshFilter[8];
        }
        terrainFaces = new TerrainFace[8];
        //octahedronFaces = new OctahedronFace[8];
        //Vector3[] directions = { Vector3.up, Vector3.down, Vector3.left, Vector3.right, Vector3.forward, Vector3.back };
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

        Vector3[] axisA = {
            directions[1],
            directions[0],
            directions[3],
            directions[2],
            directions[5],
            directions[4],
            directions[7],
            directions[6]
        };

        Vector3[] leftDownCorners = {
            LeftDownCorner.ForwardUp,
            LeftDownCorner.BackUp,
            LeftDownCorner.LeftUp,
            LeftDownCorner.RightUp,
            LeftDownCorner.ForwardDown,
            LeftDownCorner.BackDown,
            LeftDownCorner.LeftDown,
            LeftDownCorner.RightDown,
        };
        for (int i = 0; i < 1; i++)
        //for (int i = 0; i < 1; i++)
        {
            if (meshFilters[i] == null)
            {
                GameObject meshObj = new GameObject($"mesh{i}");
                meshObj.transform.parent = transform;

                meshObj.AddComponent<MeshRenderer>().sharedMaterial = new Material(Shader.Find("Standard"));
                meshFilters[i] = meshObj.AddComponent<MeshFilter>();
                meshFilters[i].sharedMesh = new Mesh();
            }

            terrainFaces[i] = new TerrainFace(meshFilters[i].sharedMesh, resolution, directions[i], axisA[i]);
            //octahedronFaces[i] = new OctahedronFace(meshFilters[i].sharedMesh, resolution, directions[i], axisA[i], leftDownCorners[i]);
            //octahedronFaces[i] = new OctahedronFace(meshFilters[i].sharedMesh, resolution, directions[i], 1f);
        }
    }

    void GenerateMesh()
    {
        foreach (TerrainFace face in terrainFaces)
        {
            if (face != null)
                face.ConstructOctahedronMesh();
        }
        //foreach (OctahedronFace face in octahedronFaces)
        //{
        //    if (face != null)
        //        face.CreateMesh();
        //}
    }

    public class LeftDownCorner
    {
        public static Vector3 ForwardUp = new Vector3(.5f,0,.5f);
        public static Vector3 BackUp = new Vector3(-.5f, 0, -.5f);
        public static Vector3 LeftUp = new Vector3(.5f, 0, -.5f);
        public static Vector3 RightUp = new Vector3(-.5f, 0, .5f);
        public static Vector3 ForwardDown = new Vector3(.5f, 0, .5f);
        public static Vector3 BackDown = new Vector3(.5f, 0, .5f);
        public static Vector3 LeftDown = new Vector3(.5f, 0, .5f);
        public static Vector3 RightDown = new Vector3(.5f, 0, .5f);
    }
}
