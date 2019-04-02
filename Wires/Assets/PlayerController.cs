using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    public float MovementSpeed = 500.0f;
    public float LookSensitivity = 3.0f;
    public Camera cam;

    Rigidbody rb;

    void Awake()
    {
        rb = transform.GetComponent<Rigidbody>();
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    Vector3 velocity;
    Vector3 rotation;

    // Update is called once per frame
    void Update()
    {
        // movement.x = Input.GetAxis("Straif");
        Vector3 movementHorizontal = transform.right * Input.GetAxis("Horizontal");
        Vector3 movementVertical = transform.forward * Input.GetAxis("Vertical");

        velocity = (movementHorizontal + movementVertical) * MovementSpeed;

        rotation.x = Input.GetAxis("Mouse X") * LookSensitivity;
        rotation.y = -Input.GetAxis("Mouse Y") * LookSensitivity;

    }

    void FixedUpdate()
    {        
        rb.velocity = new Vector3(velocity.x * Time.fixedDeltaTime, rb.velocity.y, velocity.z * Time.fixedDeltaTime);

        rb.MoveRotation(rb.rotation * Quaternion.Euler(0, rotation.x, 0));
        
        cam.transform.Rotate(new Vector3(rotation.y, 0, 0));





    }
}
