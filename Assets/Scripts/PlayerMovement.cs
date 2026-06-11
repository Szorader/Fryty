using UnityEngine;
using PlayerInput;
using FMODUnity;
using FMOD.Studio;
using STOP_MODE = FMOD.Studio.STOP_MODE;

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
    public bool isDead = false;
    
    [Header("AUDIO")]
    [SerializeField] private EventReference footsteps_indoors;
    
    private EventInstance footstepInstance;
    private bool isFootstepPlaying = false;
    
    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        SaveData data = SaveSystem.Instance.LoadGame();

        if (data != null && data.sensitivity > 0)
        {
            lookSensitivity = data.sensitivity;
        }

        // audio
        footstepInstance = RuntimeManager.CreateInstance(footsteps_indoors);
        footstepInstance.set3DAttributes(RuntimeUtils.To3DAttributes(gameObject));
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
        HandleFootsteps();
        
        if (footstepInstance.isValid())
        {
            footstepInstance.set3DAttributes(RuntimeUtils.To3DAttributes(gameObject));
        }
    }
    
    public void SetSensitivity(float value)
    {
        lookSensitivity = value;
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
    
    public void Die()
    {
        isDead = true;

        StopFootsteps();
        
        

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
    
    // audio logic
    
    private bool IsMoving()
    {
        return moveInput.magnitude > 0.1f && controller.isGrounded;
    }
    
    private void HandleFootsteps()
    {
        if (isDead) return;

        bool moving = IsMoving();

        // start footsteps loop
        if (moving && !isFootstepPlaying)
        {
            footstepInstance.start();
            isFootstepPlaying = true;
        }
        
        // stop footsteps
        if (!moving && isFootstepPlaying)
        {
            footstepInstance.stop(STOP_MODE.ALLOWFADEOUT);
            isFootstepPlaying = false;
        }
    }
    private void OnDestroy()
    {
        if (footstepInstance.isValid())
        {
            footstepInstance.stop(STOP_MODE.IMMEDIATE);
            footstepInstance.release();
        }
    }
    
    public void StopFootsteps()
    {
        if (footstepInstance.isValid())
        {
            footstepInstance.stop(STOP_MODE.ALLOWFADEOUT);
            footstepInstance.release();
        }

        isFootstepPlaying = false;
    }
    
}