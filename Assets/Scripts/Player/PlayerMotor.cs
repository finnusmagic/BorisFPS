using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerMotor : MonoBehaviour {

    [SerializeField] Camera playerCamera;

    Vector3 velocity = Vector3.zero;
    Vector3 rotation = Vector3.zero;
    Vector3 cameraRotation = Vector3.zero;
    Rigidbody rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    public void Move (Vector3 _velocity)
    {
        velocity = _velocity;
    }

    private void FixedUpdate()
    {
        PerformMovement();
        PerformRotation();
    }

    void PerformMovement()
    {
        if (velocity != Vector3.zero)
        {
            rb.MovePosition(rb.position + velocity * Time.fixedDeltaTime);
        }
    }

    public void Rotate(Vector3 _rotation)
    {
        rotation = _rotation;
    }

    void PerformRotation()
    {
        if (rotation != Vector3.zero)
        {
            rb.MoveRotation(rb.rotation * Quaternion.Euler(rotation));
        }
    }

    public void RotateCamera(Vector3 _cameraRotation)
    {
        cameraRotation = _cameraRotation;

        if (playerCamera != null)
        {
            playerCamera.transform.Rotate(-cameraRotation);
        }
    }
}
