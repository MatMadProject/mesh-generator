using UnityEngine;

namespace MeshShapes
{
    public class EquilateralTriangle
    {
        public Vector3[] Vertices {get; private set;}
        public int[] Triangles { get; private set; }

        private float sideLength = 1f;
        private float height = 0f;

        private Vector3 centerPoint = new Vector3(0f, 0f, 0f);
        public EquilateralTriangle(float sideLength)
        {
            this.sideLength = sideLength;
            height = CalculateHeight(sideLength);
            CreateShape();

        }
        private float CalculateHeight(float sideLength)
        {
            return (Mathf.Sqrt(3f) * sideLength) / 2f;
        }
        private void CreateShape()
        {
            Vertices = new Vector3[]
            {
                //bottom side
                new Vector3(-sideLength/2, centerPoint.y, centerPoint.z),
                new Vector3(centerPoint.x, centerPoint.y, height),
                new Vector3(sideLength/2, centerPoint.y, centerPoint.z)
            };

            Triangles = new int[]
            {
            0,1,2,
            };
        }
    }
}

