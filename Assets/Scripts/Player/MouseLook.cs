using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseLook : MonoBehaviour
{
    [Range(0, 1000)] public float MouseSensitivity;
    [Range(0, 180)] public float UpperYRotationAmount;
    [Range(0, 180)] public float LowerYRotationAmount;

    private Transform _playerBody;
    private float _xRotation;

    private void Start()
    {
        _playerBody = transform.parent.transform;
        _xRotation = 0;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void Update()
    {
        float mouseX = Input.GetAxis("Mouse X") * MouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * MouseSensitivity * Time.deltaTime;

        _xRotation -= mouseY;
        _xRotation = Mathf.Clamp(_xRotation, -UpperYRotationAmount / 2, LowerYRotationAmount / 2);

        transform.localRotation = Quaternion.Euler(_xRotation, 0f, 0f);
        _playerBody.Rotate(Vector3.up * mouseX);
    }
}
