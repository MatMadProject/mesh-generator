using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public Joystick joystick;
    [SerializeField] private float speed = 1f;
    
    //[SerializeField] private float speedGainPerSecond = 0.2f;
    private Vector3 movementDirection;


    // Update is called once per frame
    void Update()
    {
        movementDirection = new Vector3(joystick.Horizontal, 0 , joystick.Vertical).normalized;

    }

    void FixedUpdate()
    {
        GetComponent<Rigidbody>().MovePosition(
           GetComponent<Rigidbody>().position + 
            transform.TransformDirection(movementDirection) * speed * Time.deltaTime);
    }
}
