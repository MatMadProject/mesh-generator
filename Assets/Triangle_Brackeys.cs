using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
public class Triangle_Brackeys : MonoBehaviour
{
    private Mesh mesh;
    private Vector3[] vertices;
    private int[] triangles;

    // Start is called before the first frame update
    void Start()
    {
        mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = mesh;

        CreateShape();
        UpdateMesh();
    }
    // Update is called once per frame
    void Update()
    {

    }

    private void CreateShape()
    {
        vertices = new Vector3[]
        {
            //bottom side
            new Vector3(1, 0, 0),
            new Vector3(0, 0, 0),
            new Vector3(0, 0, 1),
            new Vector3(1, 0, 1),
            //
            new Vector3(1, 0, 0),
            new Vector3(0, 0, 0),
            new Vector3(0, 1, 0),
            new Vector3(1, 1, 0),

            new Vector3(1, 0, 1),
            new Vector3(0, 0, 1),
            new Vector3(0, 1, 1),
            new Vector3(1, 1, 1),
            //
            new Vector3(0, 0, 1),
            new Vector3(0, 1, 1),
            new Vector3(0, 1, 0),
            new Vector3(0, 0, 0),

            new Vector3(1, 0, 1),
            new Vector3(1, 1, 1),
            new Vector3(1, 1, 0),
            new Vector3(1, 0, 0),

            //top side
            new Vector3(1, 1, 0),
            new Vector3(0, 1, 0),
            new Vector3(0, 1, 1),
            new Vector3(1, 1, 1),
        };

        triangles = new int[]
        {
            //bottom inside
            //0,1,2,
            //2,3,0,
            //bottom outside - revert clockwise
            2,1,0, 
            0,3,2,
            //
            4,5,6,
            6,7,4,
            //revert clockwise
            10,9,8, 
            8,11,10,
            //
            12,13,14,
            14,15,12,
            //revert clockwise
            18,17,16,
            16,19,18,
            //top outside
            20,21,22,
            22,23,20
        };
    }

    private void UpdateMesh()
    {
        mesh.Clear();
        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.RecalculateNormals();
    }

   
}
