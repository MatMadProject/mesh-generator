
using UnityEngine;

public class Hexagon
{
    public Vector3[] Vertices { get; private set; }
    public int[] Triangles { get; private set; }
    private float sideLength = 1f;
    private float height = 0f;
    private Vector3 centerPoint = new Vector3(0f,0f,0f);
    public static int VerticesAmmmount = 7;
    public static int TrianglesAmmount = 18;
    public Vector3 CenterPoint { get; private set; }

    public Hexagon(Vector3 centerPoint, float sideLength)
    {
        this.centerPoint = centerPoint;
        this.sideLength = sideLength;
        height = CalculateHeight(sideLength);
        CreateShape();
    }

    private void CreateShape()
    {
        Vertices = new Vector3[]
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
            new Vector3(
                centerPoint.x - sideLength,
                centerPoint.y,
                centerPoint.z + ((2/3f)*height)),
            new Vector3(
                centerPoint.x - (sideLength/2f),
                centerPoint.y,
                centerPoint.z + height + ((2/3f)*height)),
            new Vector3(
                centerPoint.x + (sideLength/2f),
                centerPoint.y,
                centerPoint.z + height + ((2/3f)*height)),
            new Vector3(
                centerPoint.x + sideLength,
                centerPoint.y,
                centerPoint.z + ((2/3f)*height)),
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
}
