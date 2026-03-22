using UnityEngine;
using PlayerInput;

[RequireComponent(typeof(CharacterController))]
public class PlayerMovement : MonoBehaviour
{
    private PlayerInputSystem controls;
    private CharacterController controller;
    public float moveSpeed = 5f;
    public float gravity = -9.81f;
    public Transform playerCamera;
    public float lookSensitivity = 1f;
    public float maxLookAngle = 90f;

    private Vector2 moveInput;
    private Vector2 lookInput;
    private float xRotation = 0f;

    private Vector3 velocity;
    
    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
    
    private void Awake()
    {
        controls = new PlayerInputSystem();
        controller = GetComponent<CharacterController>();

        // Ruch
        controls.Player.Move.performed += ctx => moveInput = ctx.ReadValue<Vector2>();
        controls.Player.Move.canceled += ctx => moveInput = Vector2.zero;

        // Obrót
        controls.Player.Look.performed += ctx => lookInput = ctx.ReadValue<Vector2>();
        controls.Player.Look.canceled += ctx => lookInput = Vector2.zero;
    }

    private void OnEnable()
    {
        controls.Player.Enable();
    }

    private void OnDisable()
    {
        controls.Player.Disable();
    }

    private void Update()
    {
        HandleMovement();
        HandleLook();
        ApplyGravity();
    }

    private void HandleMovement()
    {
        Vector3 move = transform.right * moveInput.x + transform.forward * moveInput.y;
        controller.Move(move * moveSpeed * Time.deltaTime);
    }

    private void HandleLook()
    {
        //os Y
        transform.Rotate(Vector3.up * lookInput.x * lookSensitivity);

        //os X
        xRotation -= lookInput.y * lookSensitivity;
        xRotation = Mathf.Clamp(xRotation, -maxLookAngle, maxLookAngle);
        playerCamera.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
    }

    private void ApplyGravity()
    {
        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);

        if (controller.isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }
    }
}