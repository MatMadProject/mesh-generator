
using UnityEngine;

public class LinearFunction
{
    public float ParametrA { get; set; } = 1f;
    public float ParametrB { get; set; } = 0f;

    public LinearFunction(float parametrA, float parametrB) : this(parametrA)
    {
        this.ParametrB = parametrB;
    }
    public LinearFunction(float parametrA)
    {
        this.ParametrA = parametrA;
    }

    public LinearFunction()
    {
       
    }

    public Vector3 getPoint(float xCoordinate)
    {
        return new Vector3(xCoordinate, 0f, ParametrA * xCoordinate + ParametrB);
    }
}
