
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Player poroperties")]
    [SerializeField] private float speed = 1f;
    [SerializeField] private float speedGainPerSecond = 0.2f;
    [SerializeField] private float turnSpeed = 200f;
    private int steerValue;

    //[Header("Camera poroperties")]
    //[SerializeField] private float sensitiveX;
    //[SerializeField] private float sensitiveY;
    //[SerializeField] private float rotationX;
    //[SerializeField] private float rotationY;

    public Transform orientation;
    // Start is called before the first frame update
    void Start()
    {
        //Cursor.lockState = CursorLockMode.Locked;
        //Cursor.visible = false;
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
