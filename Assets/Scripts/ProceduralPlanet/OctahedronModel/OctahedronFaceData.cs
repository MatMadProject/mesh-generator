using System.Collections;
using UnityEngine;


public sealed class OctahedronFaceData
{
    public Vector3 direction;
    public Vector3 axisY;
    public Vector3 leftDownVertices;

    public OctahedronFaceData(Vector3 direction, Vector3 axisY, Vector3 leftDownVertices)
    {
        this.direction = direction;
        this.axisY = axisY;
        this.leftDownVertices = leftDownVertices;
    }

    public static readonly OctahedronFaceData ForwardUp = new(
        Vector3.forward + Vector3.up,
        Vector3.back + Vector3.up,
        new Vector3(.5f, 0, .5f)
        );
    public static readonly OctahedronFaceData BackUp = new (
        Vector3.back + Vector3.up,
        Vector3.forward + Vector3.up,
        new Vector3(-.5f, 0, -.5f)
        );
    public static readonly OctahedronFaceData LeftUp = new (
        Vector3.left + Vector3.up,
        Vector3.right + Vector3.up,
        new Vector3(-.5f, 0, .5f)
        );
    public static readonly OctahedronFaceData RightUp = new (
        Vector3.right + Vector3.up,
        Vector3.left + Vector3.up,
        new Vector3(.5f, 0, -.5f)
        );
    public static readonly OctahedronFaceData ForwardDown = new (
        Vector3.forward + Vector3.down,
        Vector3.back + Vector3.down,
        new Vector3(-.5f, 0, .5f)
        );
    public static readonly OctahedronFaceData BackDown = new (
        Vector3.back + Vector3.down,
        Vector3.forward + Vector3.down,
        new Vector3(.5f, 0, -.5f)
        );
    public static readonly OctahedronFaceData LeftDown = new (
        Vector3.left + Vector3.down,
        Vector3.right + Vector3.down,
        new Vector3(-.5f, 0, -.5f)
        );
    public static readonly OctahedronFaceData RightDown = new (
        Vector3.right + Vector3.down,
        Vector3.left + Vector3.down,
        new Vector3(.5f, 0, .5f)
        );

    public static OctahedronFaceData SelectFace(OctahedronFace face)
    {
        switch (face)
        {
            case OctahedronFace.ForwardUp:
                return ForwardUp;
            case OctahedronFace.BackUp:
                return BackUp;
            case OctahedronFace.LeftUp:
                return LeftUp;
            case OctahedronFace.RightUp:
                return RightUp;
            case OctahedronFace.ForwardDown:
                return ForwardDown;
            case OctahedronFace.BackDown:
                return BackDown;
            case OctahedronFace.LeftDown:
                return LeftDown;
            case OctahedronFace.RightDown:
                return RightDown;
        }
        return null;
    }
}
