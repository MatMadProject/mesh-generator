
using UnityEngine;
using Assets.Scripts.MeshShapes;

[RequireComponent(typeof(MeshFilter))]
public class EquilateralTriangle : MonoBehaviour
{
    private Mesh mesh;
    [SerializeField]
    private Vector3[] vertices;
    private int[] triangles;

    [SerializeField]
    private float sideLength = 1f;
    [SerializeField]
    private float height = 0f; // z axis
    [SerializeField]
    private float heightOfSmallTraingle;
    [SerializeField]
    private Vector3 centerPoint = new Vector3(0f, 0f, 0f);
    [SerializeField]
    private bool UpdateShape = false;
    // Start is called before the first frame update
    void Start()
    {
        mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = mesh;
    }

    private void Update()
    {
        if(UpdateShape)
        {
            height = CalculateHeight(sideLength);
            CreateShape();
            UpdateMesh();
        }
    }

    //private void OnDrawGizmos()
    //{
    //    if (UpdateShape)
    //    {
    //        DrawHeightLineFromVertices();
    //    }
    //}


    private void CreateShape()
    {
        transform.position = centerPoint;
        vertices = new Vector3[]
        {
            //bottom side
            new Vector3(
                centerPoint.x - (sideLength/2f),
                centerPoint.y,
                centerPoint.z - (height/3f)),
            new Vector3(
                centerPoint.x,
                centerPoint.y,
                centerPoint.z + ((2/3f)*height)),
            new Vector3(
                centerPoint.x + (sideLength/2f),
                centerPoint.y,
                centerPoint.z - (height/3f)),
     
        };

        triangles = new int[]
        {
            0,1,2,
            3,1,0
        };
    }
   

    private void UpdateMesh()
    {
        mesh.Clear();
        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.RecalculateNormals();
    }

    private float CalculateHeight(float sideLength)
    {
        return (Mathf.Sqrt(3f) * sideLength)/2f;
    }

    private void DrawHeightLineFromVertices()
    {
        heightOfSmallTraingle = Mathf.Sin(Mathf.Deg2Rad * 60f) * (sideLength / 2f);
        float sideOfSmallTriangle = sideLength - (Mathf.Cos(Mathf.Deg2Rad * 60f) * (sideLength / 2f));

        Gizmos.color = Color.white;
        Gizmos.DrawLine(vertices[0], new Vector3(vertices[0].x + sideOfSmallTriangle, vertices[0].y, vertices[0].z + heightOfSmallTraingle));
        Gizmos.DrawLine(vertices[1], new Vector3(vertices[0].x + sideLength/2f, vertices[1].y, vertices[0].z + heightOfSmallTraingle));
        Gizmos.DrawLine(vertices[2], new Vector3(vertices[2].x - sideOfSmallTriangle, vertices[2].y, vertices[2].z + heightOfSmallTraingle));
    }
}