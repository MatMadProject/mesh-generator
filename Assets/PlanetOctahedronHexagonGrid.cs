using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlanetOctahedronHexagonGrid : MonoBehaviour
{
    [Range(1, 7)]
    public int resolution = 1;
    [SerializeField, HideInInspector]
    private MeshFilter[] meshFilters;
    private PlanetOctahedronModelFace[] planetOctahedronModelFaces;
    [SerializeField]
    private float sideSize = 1f;
    [SerializeField]
    private bool Sphere = false;
    [SerializeField]
    private bool DrawTriangleFaceCenterPoint = false;

    private void OnValidate()
    {
        //Initialize();
        //GenerateMesh();
        //GenerateHexagonGridMesh();
    }

    private void Start()
    {
        Initialize();
        GenerateHexagonGridMesh();
    }

    private void Update()
    {
        //transform.RotateAround(Vector3.up, 0.001f);
        transform.Rotate(new Vector3(0, 0.001f, 0));

    }

    void Initialize()
    {
        if (meshFilters == null || meshFilters.Length == 0)
        {
            meshFilters = new MeshFilter[8];
        }
        planetOctahedronModelFaces = new PlanetOctahedronModelFace[8];

        PlanetOctahedronModel.Face[] faces = {
            PlanetOctahedronModel.Face.ForwardUp,
            PlanetOctahedronModel.Face.BackUp,
            PlanetOctahedronModel.Face.LeftUp,
            PlanetOctahedronModel.Face.RightUp,
            PlanetOctahedronModel.Face.ForwardDown,
            PlanetOctahedronModel.Face.BackDown,
            PlanetOctahedronModel.Face.LeftDown,
            PlanetOctahedronModel.Face.RightDown
        };

        for (int i = 0; i < 8; i++)
        {
            if (meshFilters[i] == null)
            {
                GameObject meshObj = new GameObject($"Face_{faces[i]}");
                meshObj.transform.parent = transform;

                meshObj.AddComponent<MeshRenderer>().sharedMaterial = new Material(Shader.Find("Standard"));
                meshFilters[i] = meshObj.AddComponent<MeshFilter>();
                meshFilters[i].sharedMesh = new Mesh();
            }

            if(planetOctahedronModelFaces[i] == null)
            {
                planetOctahedronModelFaces[i] = new PlanetOctahedronModelFace(
                meshFilters[i].sharedMesh,
                meshFilters[i].gameObject,
                resolution,
                faces[i],
                sideSize)
                {
                    Sphere = Sphere,
                    DrawTriangleFaceCenterPoint = DrawTriangleFaceCenterPoint
                };
            }
            else
            {
                planetOctahedronModelFaces[i].SetResolution(resolution);
                planetOctahedronModelFaces[i].SetSize(resolution);
            }
            
        }
    }


    private void OnDrawGizmos()
    {
        if (DrawTriangleFaceCenterPoint)
        {
            foreach (PlanetOctahedronModelFace face in planetOctahedronModelFaces)
            {
                //if (face != null)
                    //face.DrawCenterOfGavityOfSingleTriangleFace();
            }
        }

    }
    private void GenerateMesh()
    {
        foreach (PlanetOctahedronModelFace face in planetOctahedronModelFaces)
        {
            if (face != null)
                face.CreateMesh();
        }
    }

    private void GenerateHexagonGridMesh()
    {
        foreach (PlanetOctahedronModelFace face in planetOctahedronModelFaces)
        {
            if (face != null)
            {
                //DeleteAllOldHexagonTiles(face);
                face.CreateGridMesh();
            }   
        }
    }

    private void DeleteAllOldHexagonTiles(PlanetOctahedronModelFace face)
    {
        GameObject modelFace = GameObject.Find($"Face_{face.face}");
        for (int i = modelFace.transform.childCount - 1; i >= 0; i--)
            DestroyImmediate(modelFace.transform.GetChild(i));
    }
}
