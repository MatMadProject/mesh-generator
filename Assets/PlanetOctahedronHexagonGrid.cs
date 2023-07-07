
using UnityEngine;

public class PlanetOctahedronHexagonGrid : MonoBehaviour
{
    [Range(1, 7)]
    public int resolution = 1;
    [SerializeField, HideInInspector]
    private MeshFilter[] meshFilters;
    private GameObject[] gridFaces;
    private PlanetOctahedronModelFace[] planetOctahedronModelFaces;
    [SerializeField]
    private float sideSize = 1f;
    [SerializeField]
    private int tile = 0;
    [SerializeField]
    private Material material;
    //[SerializeField]
    //private bool Sphere = false;
    //[Header("Gizmo Data")]
    //[SerializeField]
    //private bool DrawTriangleFaceCenterPoint = false;

    private void OnValidate()
    {
        //Initialize();
        //GenerateMesh();
        //GenerateHexagonGridMesh();
    }

    private void Start()
    {
        Initialize3();
        GenerateHexagonGridMesh();
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
                GameObject meshObj = new($"Face_{faces[i]}");
                meshObj.transform.parent = transform;

                meshObj.AddComponent<MeshRenderer>().sharedMaterial = material;//new Material(Shader.Find("Standard"));
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
                sideSize);
            }
            else
            {
                planetOctahedronModelFaces[i].SetResolution(resolution);
                planetOctahedronModelFaces[i].SetSize(resolution);
            } 
        }
    }

    void Initialize2()
    {
        if (gridFaces == null || gridFaces.Length == 0)
        {
            gridFaces = new GameObject[8];
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
            if (gridFaces[i] == null)
            {
                GameObject face = new($"Face_{faces[i]}");
                face.transform.parent = transform;
                gridFaces[i] = face;
            }

            if (planetOctahedronModelFaces[i] == null)
            {
                planetOctahedronModelFaces[i] = new PlanetOctahedronModelFace(
                gridFaces[i],
                resolution,
                faces[i],
                sideSize);
            }
        }
    }

    void Initialize3()
    {
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

        GameObject grid = new("Grid");
        grid.transform.parent = transform;

        for (int i = 0; i < 8; i++)
        {
            if (planetOctahedronModelFaces[i] == null)
            {
                planetOctahedronModelFaces[i] = new PlanetOctahedronModelFace(
                grid,
                resolution,
                faces[i],
                sideSize);
            }
        }
    }
    private void OnDrawGizmos()
    {
        //if (DrawTriangleFaceCenterPoint)
        //{
            //foreach (PlanetOctahedronModelFace face in planetOctahedronModelFaces)
            //{
                //if (face != null)
                    //face.DrawCenterOfGavityOfSingleTriangleFace();
            //}
        //}

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
                face.material = material;
                face.CreateGrid();
                HorizontalBorderRow(face);
                VerticalBorderRow(face);
                CalculateTile(face);
                face.UpdateGridMesh();
            }   
        }
    }

    private void DeleteAllOldHexagonTiles(PlanetOctahedronModelFace face)
    {
        GameObject modelFace = GameObject.Find($"Face_{face.Face}");
        for (int i = modelFace.transform.childCount - 1; i >= 0; i--)
            DestroyImmediate(modelFace.transform.GetChild(i));
    }

    private PlanetOctahedronModelFace GetPlanetOctahedronModelFace (PlanetOctahedronModel.Face face)
    {
        foreach (PlanetOctahedronModelFace model in planetOctahedronModelFaces)
        {
            if (model.Face == face)
                return model;
        }
        return null;
    }

    private void HorizontalBorderRow(PlanetOctahedronModelFace face)
    {
        switch (face.Face)
        {
            case PlanetOctahedronModel.Face.ForwardDown:
                face.GenerateDownHorizontalRowHexagonTiles(planetOctahedronModelFaces[0]);
                break;
            case PlanetOctahedronModel.Face.BackDown:
                face.GenerateDownHorizontalRowHexagonTiles(planetOctahedronModelFaces[1]);
                break;
            case PlanetOctahedronModel.Face.LeftDown:
                face.GenerateDownHorizontalRowHexagonTiles(planetOctahedronModelFaces[2]);
                break;
            case PlanetOctahedronModel.Face.RightDown:
                face.GenerateDownHorizontalRowHexagonTiles(planetOctahedronModelFaces[3]);
                break;
        }
    }

    private void VerticalBorderRow(PlanetOctahedronModelFace face)
    {
        switch (face.Face)
        {
            case PlanetOctahedronModel.Face.ForwardUp:
                face.GenerateVerticalRowHexagonTiles(planetOctahedronModelFaces[3], planetOctahedronModelFaces[2]);
                break;
            case PlanetOctahedronModel.Face.BackUp:
                face.GenerateVerticalRowHexagonTiles(planetOctahedronModelFaces[2], planetOctahedronModelFaces[3]);
                break;
            case PlanetOctahedronModel.Face.ForwardDown:
                face.GenerateVerticalRowHexagonTiles(planetOctahedronModelFaces[6], planetOctahedronModelFaces[7]);
                break;
            case PlanetOctahedronModel.Face.BackDown:
                face.GenerateVerticalRowHexagonTiles(planetOctahedronModelFaces[7], planetOctahedronModelFaces[6]);
                break;
        }
    }



    private void CalculateTile(PlanetOctahedronModelFace face)
    {
        tile += face.HexagonTileCount();
    }
}
