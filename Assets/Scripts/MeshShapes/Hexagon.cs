
using UnityEngine;

public class Hexagon
{
    public Vector3[] Vertices { get; private set; }
    public int[] Triangles { get; private set; }
    private float sideLength = 1f;
    public float Height { get; private set; } = 0f;
    public Vector3 StartPoint { get; private set; } = new Vector3(0f,0f,0f);
    public static int VerticesAmmmount = 7;
    public static int TrianglesAmmount = 18;
    public Vector3 CenterPoint { get; private set; }

    public Hexagon(Vector3 centerPoint, float sideLength)
    {
        this.StartPoint = centerPoint;
        this.sideLength = sideLength;
        Height = CalculateHeight(sideLength);
        CreateShape();
    }

    private void CreateShape()
    {
        Vertices = new Vector3[]
        {
            //bottom side
            new Vector3(
                StartPoint.x - (sideLength/2f),
                StartPoint.y,
                StartPoint.z - (Height/3f)),
            new Vector3(
                StartPoint.x,
                StartPoint.y,
                StartPoint.z + ((2/3f)*Height)),
            new Vector3(
                StartPoint.x + (sideLength/2f),
                StartPoint.y,
                StartPoint.z - (Height/3f)),
            new Vector3(
                StartPoint.x - sideLength,
                StartPoint.y,
                StartPoint.z + ((2/3f)*Height)),
            new Vector3(
                StartPoint.x - (sideLength/2f),
                StartPoint.y,
                StartPoint.z + Height + ((2/3f)*Height)),
            new Vector3(
                StartPoint.x + (sideLength/2f),
                StartPoint.y,
                StartPoint.z + Height + ((2/3f)*Height)),
            new Vector3(
                StartPoint.x + sideLength,
                StartPoint.y,
                StartPoint.z + ((2/3f)*Height)),
        };

        Triangles = new int[]
        {
            0,1,2,
            3,1,0,
            3,4,1,
            1,4,5,
            1,5,6,
            1,6,2
        };

        CenterPoint = Vertices[1];
    }

    public void UpdateTraingles(int gridIndex)
    {
        Triangles = new int[]
                {
            0 + gridIndex * VerticesAmmmount,1 + gridIndex * VerticesAmmmount,2 + gridIndex * VerticesAmmmount,
            3 + gridIndex * VerticesAmmmount,1 + gridIndex * VerticesAmmmount,0 + gridIndex * VerticesAmmmount,
            3 + gridIndex * VerticesAmmmount,4 + gridIndex * VerticesAmmmount,1 + gridIndex * VerticesAmmmount,
            1 + gridIndex * VerticesAmmmount,4 + gridIndex * VerticesAmmmount,5 + gridIndex * VerticesAmmmount,
            1 + gridIndex * VerticesAmmmount,5 + gridIndex * VerticesAmmmount,6 + gridIndex * VerticesAmmmount,
            1 + gridIndex * VerticesAmmmount,6 + gridIndex * VerticesAmmmount,2 + gridIndex * VerticesAmmmount
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
            Hexagon h = (Hexagon)obj;
            return CenterPoint == h.CenterPoint;
        }
    }
}
