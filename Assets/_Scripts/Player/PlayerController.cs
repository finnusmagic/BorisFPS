using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlayerMotor))]
public class PlayerController : MonoBehaviour {

    [SerializeField] float speed = 3f;
    [SerializeField] float sprintSpeed = 6f;
    public float lookSensitivity = 2f;

    [SerializeField] float gravity = 10.0f;
    [SerializeField] float maxVelocityChange = 10.0f;
    [SerializeField] bool canJump = true;
    [SerializeField] float jumpHeight = 2.0f;

    bool grounded = false;
    float currentSpeed;

    PlayerMotor motor;
    Rigidbody rb;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
        rb.useGravity = false;

        motor = GetComponent<PlayerMotor>();
        Cursor.lockState = CursorLockMode.Locked;
    }


    void FixedUpdate()
    {
        Rotate();

        if (grounded)
        {
            Sprint();
            Movement();
        }

        rb.AddForce(new Vector3(0, -gravity * rb.mass, 0));
        grounded = false;
    }

    private void Movement()
    {
        Vector3 targetVelocity = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
        targetVelocity = transform.TransformDirection(targetVelocity);
        targetVelocity *= currentSpeed;

        Vector3 velocity = rb.velocity;
        Vector3 velocityChange = (targetVelocity - velocity);
        velocityChange.x = Mathf.Clamp(velocityChange.x, -maxVelocityChange, maxVelocityChange);
        velocityChange.z = Mathf.Clamp(velocityChange.z, -maxVelocityChange, maxVelocityChange);
        velocityChange.y = 0;
        rb.AddForce(velocityChange, ForceMode.VelocityChange);

        if (canJump && Input.GetButton("Jump"))
        {
            rb.velocity = new Vector3(velocity.x, CalculateJumpVerticalSpeed(), velocity.z);
        }
    }

    private void Sprint()
    {
        if (Input.GetKey(KeyCode.LeftShift))
        {
            currentSpeed = sprintSpeed;
        }
        else
        {
            currentSpeed = speed;
        }
    }

    private void Rotate()
    {
        float _yRotation = Input.GetAxis("Mouse X");
        Vector3 _rotation = new Vector3(0f, _yRotation, 0f) * lookSensitivity;

        motor.Rotate(_rotation);

        float _xRotation = Input.GetAxis("Mouse Y");
        Vector3 _cameraRotation = new Vector3(_xRotation, 0f, 0f) * lookSensitivity;

        motor.RotateCamera(_cameraRotation);
    }

    void OnCollisionStay()
    {
        grounded = true;
    }

    float CalculateJumpVerticalSpeed()
    {
        return Mathf.Sqrt(2 * jumpHeight * gravity);
    }

}
