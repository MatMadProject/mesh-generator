using UnityEngine;

public class PlanetRotation : MonoBehaviour
{
    public float rotationSpeed = 0.1f;
    public GameObject centerPointOfMassGravity;

    // Update is called once per frame
    void Update()
    {
       //transform.Rotate(new Vector3(0, rotationSpeed, 0) * Time.deltaTime);

        //if (centerPointOfMassGravity != null)
        //    transform.RotateAround(centerPointOfMassGravity.transform.position, Vector3.up, rotationSpeed * Time.deltaTime);
    }

    private void FixedUpdate()
    {
        if (centerPointOfMassGravity != null)
            transform.RotateAround(centerPointOfMassGravity.transform.position, Vector3.up, rotationSpeed * Time.deltaTime);
    }
}
