using System;
using UnityEngine;


public class Square
{
    private readonly Vector3[] vertices;
    private readonly int [] triangels = new int[]
    {
        0,1,2,
        3,2,1,
    };

    public Square(Vector3 vector0, Vector3 vector1, Vector3 vector2, Vector3 vector3)
    {
        vertices = new Vector3[4];
        vertices[0] = vector0;
        vertices[1] = vector1;
        vertices[2] = vector2;
        vertices[3] = vector3;

    }

    public int [] Triangels()
    {
        return triangels;
    }

    public int[] ReverseTriangels()
    {
        Array.Reverse(triangels);
        return triangels;
    }
}
