
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Player poroperties")]
    [SerializeField] private float speed = 1f;
    [SerializeField] private float speedGainPerSecond = 0.2f;
    [SerializeField] private float turnSpeed = 200f;
    private int steerValue;

    public Transform orientation;
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        Steer();

        speed += speedGainPerSecond * Time.deltaTime;
        transform.Rotate(0f, steerValue * turnSpeed * Time.deltaTime, 0f);
        transform.Translate(Vector3.forward * speed * Time.deltaTime);
    }

    public void Steer()
    {
        if (Input.GetKeyDown(KeyCode.LeftArrow))
            steerValue = -1;

        if (Input.GetKeyDown(KeyCode.RightArrow))
            steerValue = 1;

        if (Input.GetKeyUp(KeyCode.LeftArrow) || Input.GetKeyUp(KeyCode.RightArrow))
            steerValue = 0;
    }
}
