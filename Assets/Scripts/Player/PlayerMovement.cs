using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Range(0, 25)] public float Speed;
    [Range(-50, 0)] public float Gravity;
    [Tooltip("The radius for which ground is checked for")]
    [Range(0, 1)] public float GroundDistance;
    [Range(0, 10)] public float JumpHeight;
    [Range(0, 25)] public float SpeedIncrease;
    public LayerMask GroundMask;
    public Transform GroundCheck;

    private CharacterController _controller;
    private Vector3 _velocity;
    private bool _isGrounded;
    private float _regularSpeed;

    private void Awake()
    {
        _controller = GetComponent<CharacterController>();
        _velocity = new Vector3();
        _isGrounded = true;
        _regularSpeed = Speed;
    }

    private void Update()
    {
        _isGrounded = Physics.CheckSphere(GroundCheck.position, GroundDistance, GroundMask);

        if (_isGrounded && _velocity.y < 0)
            _velocity.y = -2f;

        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        Vector3 move = transform.right * x + transform.forward * z;

        _controller.Move(move * Speed * Time.deltaTime);

        if (Input.GetButton("Jump") && _isGrounded)
            _velocity.y = Mathf.Sqrt(JumpHeight * -2f * Gravity);

        _velocity.y += Gravity * Time.deltaTime;

        _controller.Move(_velocity * Time.deltaTime);

        if(Input.GetKeyDown(KeyCode.LeftShift) && _isGrounded)
            Speed += SpeedIncrease;
        else if (Input.GetKeyUp(KeyCode.LeftShift) || !_isGrounded)
            Speed = _regularSpeed;
    }
}
