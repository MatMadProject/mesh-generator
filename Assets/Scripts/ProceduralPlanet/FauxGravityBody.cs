using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FauxGravityBody : MonoBehaviour
{
    public FauxGravityAttractor fauxGravityAttractor;
    public Rigidbody myRigidbody;

    private void Start()
    {
        myRigidbody.constraints = RigidbodyConstraints.FreezeRotation;
        myRigidbody.useGravity = false;
    }
    private void Update()
    {
        fauxGravityAttractor.Attract(transform);
    }
}
