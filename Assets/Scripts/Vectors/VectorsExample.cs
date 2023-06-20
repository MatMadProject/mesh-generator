using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VectorsExample : MonoBehaviour
{
    [SerializeField]
    private bool drawCenterPoint = false;
    [SerializeField]
    private bool drawBasicVectorsDirection = false;
    [SerializeField]
    private bool drawFaceOctahedronVectorsDirection = false;
    [SerializeField]
    private bool drawCrossVectorsDirection = false;
    private void OnDrawGizmos()
    {
        DrawCenterPoint();
        DrawVectors();
        //DrawVectorsForward();
        //DrawVectorsForward2();
        //DrawVectorsBack();
        //DrawOctahedron();
        DrawOctahedron2();
        DrawOctahedronFaceVectorDirection();
        DrawCrossVectorDirection();
    }

    private void DrawCenterPoint()
    {
        if (drawCenterPoint)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawSphere(Vector3.zero, 0.05f);
        }
    }

    private void DrawVectors()
    {
        if (drawBasicVectorsDirection)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawLine(Vector3.zero, Vector3.forward);
            Gizmos.color = Color.black;
            Gizmos.DrawLine(Vector3.zero, Vector3.back);
            Gizmos.DrawLine(Vector3.zero, Vector3.left);
            Gizmos.DrawLine(Vector3.zero, Vector3.right);
            Gizmos.DrawLine(Vector3.zero, Vector3.up);
            Gizmos.DrawLine(Vector3.zero, Vector3.down);
        }   
    }

    private void DrawOctahedronFaceVectorDirection()
    {
        if (drawFaceOctahedronVectorsDirection)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawLine(Vector3.zero, Vector3.forward + Vector3.up);
            Gizmos.DrawLine(Vector3.zero, Vector3.back + Vector3.up);
            Gizmos.DrawLine(Vector3.zero, Vector3.forward + Vector3.down);
            Gizmos.DrawLine(Vector3.zero, Vector3.back + Vector3.down);
            Gizmos.DrawLine(Vector3.zero, Vector3.left + Vector3.up);
            Gizmos.DrawLine(Vector3.zero, Vector3.right + Vector3.up);
            Gizmos.DrawLine(Vector3.zero, Vector3.left + Vector3.down);
            Gizmos.DrawLine(Vector3.zero, Vector3.right + Vector3.down);
        }
    }

    private void DrawCrossVectorDirection()
    {
        if (drawCrossVectorsDirection)
        {
            Gizmos.color = Color.black;
            Gizmos.DrawLine(Vector3.zero, Vector3.forward + Vector3.up);
            Gizmos.DrawLine(Vector3.zero, Vector3.back + Vector3.up);
            Gizmos.color = Color.red;
            Gizmos.DrawLine(Vector3.zero, Vector3.Cross(Vector3.forward + Vector3.up, Vector3.back + Vector3.up) *-1f);
        }
    }

    private void DrawVectorsForward()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawLine(Vector3.zero, Vector3.forward+Vector3.right);
        Gizmos.DrawLine(Vector3.zero, Vector3.forward + Vector3.left);
        Gizmos.DrawLine(Vector3.zero, Vector3.forward + Vector3.up);
        Gizmos.DrawLine(Vector3.zero, Vector3.forward + Vector3.down);
    }

    private void DrawVectorsForward2()
    {
        Gizmos.color = Color.green;
        Vector3 v_fru = Vector3.forward + Vector3.right + Vector3.up;
        //Debug.Log($"Magnitude v_fru: {v_fru.magnitude}");
        Vector3 v_frd = Vector3.forward + Vector3.right + Vector3.down;
       //Debug.Log($"Magnitude v_frd: {v_frd.magnitude}");
        Vector3 v_flu = Vector3.forward + Vector3.left + Vector3.up;
        //Debug.Log($"Magnitude v_flu: {v_flu.magnitude}");
        Vector3 v_fld = Vector3.forward + Vector3.left + Vector3.down;
        //Debug.Log($"Magnitude v_fld: {v_fld.magnitude}");
        Gizmos.DrawLine(Vector3.zero, v_fru);
        Gizmos.DrawLine(Vector3.zero, v_frd);
        Gizmos.DrawLine(Vector3.zero, v_flu);
        Gizmos.DrawLine(Vector3.zero, v_fld);
    }

    //Magnitude jest zawsze liczb¹ dodatni¹
    private void DrawVectorsBack()
    {
        Gizmos.color = Color.green;
        Vector3 b_fru = Vector3.back + Vector3.right + Vector3.up;
        //Debug.Log($"Magnitude b_fru: {b_fru.magnitude}");
        Vector3 b_frd = Vector3.back + Vector3.right + Vector3.down;
        //Debug.Log($"Magnitude b_frd: {b_frd.magnitude}");
        Vector3 b_flu = Vector3.back + Vector3.left + Vector3.up;
        //Debug.Log($"Magnitude b_flu: {b_flu.magnitude}");
        Vector3 b_fld = Vector3.back + Vector3.left + Vector3.down;
        //Debug.Log($"Magnitude b_fld: {b_fld.magnitude}");
        Gizmos.DrawLine(Vector3.zero, b_fru);
        Gizmos.DrawLine(Vector3.zero, b_frd);
        Gizmos.DrawLine(Vector3.zero, b_flu);
        Gizmos.DrawLine(Vector3.zero, b_fld);
    }

    private void DrawOctahedron()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(Vector3.one, Vector3.one + Vector3.forward);
        Gizmos.DrawLine(Vector3.one, Vector3.one + Vector3.right);
        Gizmos.DrawLine(Vector3.one + Vector3.forward, Vector3.one + Vector3.forward + Vector3.right);
        Gizmos.DrawLine(Vector3.one + Vector3.right, Vector3.one + Vector3.right + Vector3.forward);
    }

    private void DrawOctahedron2()
    {
        Vector3 centerPoint = Vector3.zero;
        float sideSize = 1f;
        Gizmos.color = Color.yellow;
        Gizmos.DrawLine((Vector3.left + Vector3.back) * .5f, ((Vector3.left + Vector3.back) * .5f) + Vector3.right);
        Gizmos.DrawLine((Vector3.left + Vector3.back) * .5f, ((Vector3.left + Vector3.back) * .5f) + Vector3.forward);
        Gizmos.DrawLine((Vector3.right + Vector3.forward) * .5f, ((Vector3.right + Vector3.forward) * .5f) + Vector3.left);
        Gizmos.DrawLine((Vector3.right + Vector3.forward) * .5f, ((Vector3.right + Vector3.forward) * .5f) + Vector3.back);

        Gizmos.DrawLine((Vector3.left + Vector3.back) * .5f, Vector3.up * (Mathf.Sqrt(2)/2));
        Gizmos.DrawLine((Vector3.left + Vector3.forward) * .5f, Vector3.up * (Mathf.Sqrt(2) / 2));
        Gizmos.DrawLine((Vector3.right + Vector3.back) * .5f, Vector3.up * (Mathf.Sqrt(2) / 2));
        Gizmos.DrawLine((Vector3.right + Vector3.forward) * .5f, Vector3.up * (Mathf.Sqrt(2) / 2));

        Gizmos.DrawLine((Vector3.left + Vector3.back) * .5f, Vector3.down * (Mathf.Sqrt(2) / 2));
        Gizmos.DrawLine((Vector3.left + Vector3.forward) * .5f, Vector3.down * (Mathf.Sqrt(2) / 2));
        Gizmos.DrawLine((Vector3.right + Vector3.back) * .5f, Vector3.down * (Mathf.Sqrt(2) / 2));
        Gizmos.DrawLine((Vector3.right + Vector3.forward) * .5f, Vector3.down * (Mathf.Sqrt(2) / 2));
    }

    private void DrawOctahedronFaceDirections()
    {
        Vector3 centerPoint = Vector3.zero;
        float sideSize = 1f;
        Gizmos.color = Color.yellow;
        Gizmos.DrawLine((Vector3.left + Vector3.back) * .5f, ((Vector3.left + Vector3.back) * .5f) + Vector3.right);
        Gizmos.DrawLine((Vector3.left + Vector3.back) * .5f, ((Vector3.left + Vector3.back) * .5f) + Vector3.forward);
        Gizmos.DrawLine((Vector3.right + Vector3.forward) * .5f, ((Vector3.right + Vector3.forward) * .5f) + Vector3.left);
        Gizmos.DrawLine((Vector3.right + Vector3.forward) * .5f, ((Vector3.right + Vector3.forward) * .5f) + Vector3.back);

        Gizmos.DrawLine((Vector3.left + Vector3.back) * .5f, Vector3.up * (Mathf.Sqrt(2) / 2));
        Gizmos.DrawLine((Vector3.left + Vector3.forward) * .5f, Vector3.up * (Mathf.Sqrt(2) / 2));
        Gizmos.DrawLine((Vector3.right + Vector3.back) * .5f, Vector3.up * (Mathf.Sqrt(2) / 2));
        Gizmos.DrawLine((Vector3.right + Vector3.forward) * .5f, Vector3.up * (Mathf.Sqrt(2) / 2));

        Gizmos.DrawLine((Vector3.left + Vector3.back) * .5f, Vector3.down * (Mathf.Sqrt(2) / 2));
        Gizmos.DrawLine((Vector3.left + Vector3.forward) * .5f, Vector3.down * (Mathf.Sqrt(2) / 2));
        Gizmos.DrawLine((Vector3.right + Vector3.back) * .5f, Vector3.down * (Mathf.Sqrt(2) / 2));
        Gizmos.DrawLine((Vector3.right + Vector3.forward) * .5f, Vector3.down * (Mathf.Sqrt(2) / 2));
    }
}
