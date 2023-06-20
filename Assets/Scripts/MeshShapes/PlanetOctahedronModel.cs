using System.Collections;
using UnityEngine;


public class PlanetOctahedronModel : MonoBehaviour
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
        planetOctahedronModelFaces = new PlanetOctahedronModelFace[8];

        Face[] faces = {
            Face.ForwardUp,
            Face.BackUp,
            Face.LeftUp,
            Face.RightUp,
            Face.ForwardDown,
            Face.BackDown,
            Face.LeftDown,
            Face.RightDown
        };

        for (int i = 0; i < 8; i++)
        {
            if (meshFilters[i] == null)
            {
                GameObject meshObj = new GameObject($"mesh_{i}_{faces[i]}");
                meshObj.transform.parent = transform;

                meshObj.AddComponent<MeshRenderer>().sharedMaterial = new Material(Shader.Find("Standard"));
                meshFilters[i] = meshObj.AddComponent<MeshFilter>();
                meshFilters[i].sharedMesh = new Mesh();
            }

            planetOctahedronModelFaces[i] = new PlanetOctahedronModelFace(
                meshFilters[i].sharedMesh, 
                resolution, 
                faces[i], 
                sideSize);
            planetOctahedronModelFaces[i].Sphere = Sphere;
        }
    }

    void GenerateMesh()
    {
        foreach (PlanetOctahedronModelFace face in planetOctahedronModelFaces)
        {
            if (face != null)
                face.CreateMesh();
        }
    }

    public sealed class TriangleFace
    {
        public Vector3 direction;
        public Vector3 axisY;
        public Vector3 leftDownVertices;

        public TriangleFace(Vector3 direction, Vector3 axisY, Vector3 leftDownVertices)
        {
            this.direction = direction;
            this.axisY = axisY;
            this.leftDownVertices = leftDownVertices;
        }

        public static readonly TriangleFace ForwardUp = new TriangleFace(
            Vector3.forward + Vector3.up,
            Vector3.back + Vector3.up,
            new Vector3(.5f, 0, .5f)
            );
        public static readonly TriangleFace BackUp = new TriangleFace(
            Vector3.back + Vector3.up,
            Vector3.forward + Vector3.up,
            new Vector3(-.5f, 0, -.5f)
            );
        public static readonly TriangleFace LeftUp = new TriangleFace(
            Vector3.left + Vector3.up,
            Vector3.right + Vector3.up,
            new Vector3(-.5f, 0, .5f)
            );
        public static readonly TriangleFace RightUp = new TriangleFace(
            Vector3.right + Vector3.up,
            Vector3.left + Vector3.up,
            new Vector3(.5f, 0, -.5f)
            );
        public static readonly TriangleFace ForwardDown = new TriangleFace(
            Vector3.forward + Vector3.down,
            Vector3.back + Vector3.down,
            new Vector3(-.5f, 0, .5f)
            );
        public static readonly TriangleFace BackDown = new TriangleFace(
            Vector3.back + Vector3.down,
            Vector3.forward + Vector3.down,
            new Vector3(.5f, 0, -.5f)
            );
        public static readonly TriangleFace LeftDown = new TriangleFace(
            Vector3.left + Vector3.down,
            Vector3.right + Vector3.down,
            new Vector3(-.5f, 0, -.5f)
            );
        public static readonly TriangleFace RightDown = new TriangleFace(
            Vector3.right + Vector3.down,
            Vector3.left + Vector3.down,
            new Vector3(.5f, 0, .5f)
            );

        public static TriangleFace SelectFace(Face face)
        {
            switch (face)
            {
                case Face.ForwardUp:
                    return ForwardUp;
                case Face.BackUp:
                    return BackUp;
                case Face.LeftUp:
                    return LeftUp;
                case Face.RightUp:
                    return RightUp;
                case Face.ForwardDown:
                    return ForwardDown;
                case Face.BackDown:
                    return BackDown;
                case Face.LeftDown:
                    return LeftDown;
                case Face.RightDown:
                    return RightDown;
            }
            return null;
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
