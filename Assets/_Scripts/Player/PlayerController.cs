using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlayerMotor))]
public class PlayerController : MonoBehaviour {

    [SerializeField] float speed = 3f;
    [SerializeField] float sprintSpeed = 6f;
    public float lookSensitivity = 2f;

    PlayerMotor motor;

    void Start()
    {
        motor = GetComponent<PlayerMotor>();

        Cursor.lockState = CursorLockMode.Locked;   // keep confined to center of screen
    }

    void Update()
    {
        float currentSpeed;

        if (Input.GetKey(KeyCode.LeftShift))
        {
            currentSpeed = sprintSpeed;
        }
        else
        {
            currentSpeed = speed;
        }

        float _xMove = Input.GetAxis("Horizontal");
        float _zMove = Input.GetAxis("Vertical");

        Vector3 _moveHorizontal = transform.right * _xMove;
        Vector3 _moveVertical = transform.forward * _zMove;
        Vector3 _velocity = (_moveHorizontal + _moveVertical) * currentSpeed;

        motor.Move(_velocity);

        float _yRotation = Input.GetAxis("Mouse X");
        Vector3 _rotation = new Vector3(0f, _yRotation, 0f) * lookSensitivity;

        motor.Rotate(_rotation);

        float _xRotation = Input.GetAxis("Mouse Y");
        Vector3 _cameraRotation = new Vector3(_xRotation, 0f, 0f) * lookSensitivity;

        motor.RotateCamera(_cameraRotation);
    }

}
