using System;
using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]
public class Hexagon3DMesh : MonoBehaviour
{
    private Mesh mesh;
    [SerializeField]
    private bool UpdateShape = false;
    
    public Vector3[] Vertices { get; private set; }
    public int[] Triangles { get; private set; }
    [SerializeField]
    private float sideSize = 1f;
    [SerializeField]
    private float thicknessSize = 1f;
    [SerializeField]
    private float height = 0f;
    public float Height { get => height; private set { height = value; }}
    [SerializeField]
    private Vector3 startPoint = new Vector3(0f, 0f, 0f);
    public Vector3 StartPoint { get => startPoint;  private set { startPoint = value; }}
    public static int VerticesAmmmount = 14;
    public static int TrianglesAmmount = 72;
    [SerializeField]
    private Vector3 centerPoint = new Vector3(0f, 0f, 0f);
    public Vector3 CenterPoint { get => centerPoint; private set { centerPoint = value; } }


    private void Start()
    {
        mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = mesh;
        Height = CalculateHeight(sideSize);
        CreateShape();
        UpdateMesh();
    }


    private void Update()
    {
        if (UpdateShape)
        {
            startPoint = transform.position;
            Height = CalculateHeight(sideSize);
            CreateShape();
            UpdateMesh();
            UpdateShape = false;
        }
    }

    private void OnDrawGizmos()
    {
        DrawVertices();
    }

    private void DrawVertices()
    {
        if(Vertices != null)
        {
            foreach (Vector3 vector in Vertices)
            {
                Gizmos.color = Color.black;
                Gizmos.DrawSphere(vector, sideSize / 25);
            }
        }
    }

    private void UpdateMesh()
    {
        mesh.Clear();
        mesh.vertices = Vertices;
        mesh.triangles = Triangles;
        mesh.RecalculateNormals();
    }

    private void CreateShape()
    {
        Vertices = new Vector3[]
        {
            //bottom side
            new Vector3(
                StartPoint.x - (sideSize/2f),
                StartPoint.y,
                StartPoint.z - (Height/3f)),
            new Vector3(
                StartPoint.x,
                StartPoint.y,
                StartPoint.z + ((2/3f)*Height)),
            new Vector3(
                StartPoint.x + (sideSize/2f),
                StartPoint.y,
                StartPoint.z - (Height/3f)),
            new Vector3(
                StartPoint.x - sideSize,
                StartPoint.y,
                StartPoint.z + ((2/3f)*Height)),
            new Vector3(
                StartPoint.x - (sideSize/2f),
                StartPoint.y,
                StartPoint.z + Height + ((2/3f)*Height)),
            new Vector3(
                StartPoint.x + (sideSize/2f),
                StartPoint.y,
                StartPoint.z + Height + ((2/3f)*Height)),
            new Vector3(
                StartPoint.x + sideSize,
                StartPoint.y,
                StartPoint.z + ((2/3f)*Height)),
            //top side
             new Vector3(
                StartPoint.x - (sideSize/2f),
                StartPoint.y + thicknessSize,
                StartPoint.z - (Height/3f)),
            new Vector3(
                StartPoint.x,
                StartPoint.y + thicknessSize,
                StartPoint.z + ((2/3f)*Height)),
            new Vector3(
                StartPoint.x + (sideSize/2f),
                StartPoint.y + thicknessSize,
                StartPoint.z - (Height/3f)),
            new Vector3(
                StartPoint.x - sideSize,
                StartPoint.y + thicknessSize,
                StartPoint.z + ((2/3f)*Height)),
            new Vector3(
                StartPoint.x - (sideSize/2f),
                StartPoint.y + thicknessSize,
                StartPoint.z + Height + ((2/3f)*Height)),
            new Vector3(
                StartPoint.x + (sideSize/2f),
                StartPoint.y + thicknessSize,
                StartPoint.z + Height + ((2/3f)*Height)),
            new Vector3(
                StartPoint.x + sideSize,
                StartPoint.y + thicknessSize,
                StartPoint.z + ((2/3f)*Height)),
        };

        Triangles = new int[]
        {
            0,1,2,
            3,1,0,
            4,1,3,
            5,1,4, 
            6,1,5, 
            2,1,6,
            //
            7,8,9,
            10,8,7,
            11,8,10,
            12,8,11,
            13,8,12,
            9,8,13,
            //
            0,7,9,
            0,9,2,
            3,10,7,
            3,7,0,
            4,11,10,
            4,10,3,
            5,12,11,
            5,11,4,
            6,13,12,
            6,12,5,
            2,9,13,
            2,13,6
        };

        CenterPoint = Vertices[1];
    }

    public void UpdateTraingles(int gridIndex)
    {
        Triangles = new int[]
                {
            0 + gridIndex * VerticesAmmmount,1 + gridIndex * VerticesAmmmount,2 + gridIndex * VerticesAmmmount,
            3 + gridIndex * VerticesAmmmount,1 + gridIndex * VerticesAmmmount,0 + gridIndex * VerticesAmmmount,
            4 + gridIndex * VerticesAmmmount,1 + gridIndex * VerticesAmmmount,3 + gridIndex * VerticesAmmmount,
            5 + gridIndex * VerticesAmmmount,1 + gridIndex * VerticesAmmmount,4 + gridIndex * VerticesAmmmount,
            6 + gridIndex * VerticesAmmmount,1 + gridIndex * VerticesAmmmount,5 + gridIndex * VerticesAmmmount,
            1 + gridIndex * VerticesAmmmount,1 + gridIndex * VerticesAmmmount,6 + gridIndex * VerticesAmmmount,
            //
            7 + gridIndex * VerticesAmmmount,8 + gridIndex * VerticesAmmmount,9 + gridIndex * VerticesAmmmount,
            10 + gridIndex * VerticesAmmmount,8 + gridIndex * VerticesAmmmount,7 + gridIndex * VerticesAmmmount,
            11 + gridIndex * VerticesAmmmount,8 + gridIndex * VerticesAmmmount,10 + gridIndex * VerticesAmmmount,
            12 + gridIndex * VerticesAmmmount,8 + gridIndex * VerticesAmmmount,11 + gridIndex * VerticesAmmmount,
            13 + gridIndex * VerticesAmmmount,8 + gridIndex * VerticesAmmmount,12 + gridIndex * VerticesAmmmount,
            9 + gridIndex * VerticesAmmmount,8 + gridIndex * VerticesAmmmount,13 + gridIndex * VerticesAmmmount,
            //
            0 + gridIndex * VerticesAmmmount,7 + gridIndex * VerticesAmmmount,9 + gridIndex * VerticesAmmmount,
            0 + gridIndex * VerticesAmmmount,9 + gridIndex * VerticesAmmmount,2 + gridIndex * VerticesAmmmount,
            3 + gridIndex * VerticesAmmmount,10 + gridIndex * VerticesAmmmount,7 + gridIndex * VerticesAmmmount,
            3 + gridIndex * VerticesAmmmount,7 + gridIndex * VerticesAmmmount,0 + gridIndex * VerticesAmmmount,
            4 + gridIndex * VerticesAmmmount,11 + gridIndex * VerticesAmmmount,10 + gridIndex * VerticesAmmmount,
            4 + gridIndex * VerticesAmmmount,10 + gridIndex * VerticesAmmmount,3 + gridIndex * VerticesAmmmount,
            5 + gridIndex * VerticesAmmmount,12 + gridIndex * VerticesAmmmount,11 + gridIndex * VerticesAmmmount,
            5 + gridIndex * VerticesAmmmount,11 + gridIndex * VerticesAmmmount,4 + gridIndex * VerticesAmmmount,
            6 + gridIndex * VerticesAmmmount,13 + gridIndex * VerticesAmmmount,12 + gridIndex * VerticesAmmmount,
            6 + gridIndex * VerticesAmmmount,12 + gridIndex * VerticesAmmmount,5 + gridIndex * VerticesAmmmount,
            2 + gridIndex * VerticesAmmmount,9 + gridIndex * VerticesAmmmount,13 + gridIndex * VerticesAmmmount,
            2 + gridIndex * VerticesAmmmount,13 + gridIndex * VerticesAmmmount,6 + gridIndex * VerticesAmmmount,
               };
    }

    public static float CalculateHeight(float sideLength)
    {
        return (Mathf.Sqrt(3f) * sideLength) / 2f;
    }

    public override bool Equals(object obj)
    {
        //Check for null and compare run-time types.
        if ((obj == null) || !this.GetType().Equals(obj.GetType()))
        {
            return false;
        }
        else
        {
            Hexagon3DMesh h = (Hexagon3DMesh)obj;
            return CenterPoint == h.CenterPoint;
        }
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(Vertices, CenterPoint);
    }
}
