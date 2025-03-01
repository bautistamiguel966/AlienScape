using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [Header("Movement Settings")]
    public float walkSpeed = 5f;       // Velocidad al caminar
    public float sprintSpeed = 8f;     // Velocidad al correr
    public float jumpForce = 1.5f;     // Fuerza de salto (ajustada)
    public float gravity = -25f;       // Gravedad aumentada para un salto más controlado
    public float lookSensitivity = 2f; // Sensibilidad de la cámara

    [Header("References")]
    public Transform cameraTransform;  // Referencia a la cámara (para rotación)
    public CharacterController characterController; // Referencia al CharacterController

    private Vector2 _moveInput;        // Input de movimiento (WASD o joystick)
    private Vector2 _lookInput;        // Input de rotación (mouse o joystick)
    private bool _isJumping;           // ¿Está saltando?
    private bool _isSprinting;         // ¿Está corriendo?
    private Vector3 _velocity;         // Velocidad vertical (para gravedad y salto)
    private float _cameraPitch = 0f;  // Ángulo actual de la cámara en el eje X

    private void Update()
    {
        HandleMovement();
        HandleRotation();
    }

    private void HandleMovement()
    {
        // Calcular la velocidad de movimiento
        float speed = _isSprinting ? sprintSpeed : walkSpeed;
        Vector3 moveDirection = (transform.forward * _moveInput.y + transform.right * _moveInput.x) * speed;

        // Aplicar gravedad
        if (characterController.isGrounded)
        {
            _velocity.y = -2f; // Pequeña fuerza hacia abajo para asegurar que está en el suelo
            if (_isJumping)
            {
                _velocity.y = Mathf.Sqrt(jumpForce * -2f * gravity); // Fórmula de salto
            }
        }
        else
        {
            _velocity.y += gravity * Time.deltaTime; // Aplicar gravedad
        }

        // Mover el CharacterController
        characterController.Move((moveDirection + _velocity) * Time.deltaTime);
    }

    private void HandleRotation()
    {
        // Rotar el jugador en el eje Y (izquierda/derecha)
        float yaw = _lookInput.x * lookSensitivity;
        transform.Rotate(Vector3.up, yaw);

        // Rotar la cámara en el eje X (arriba/abajo) con límites
        float pitchDelta = _lookInput.y * lookSensitivity;
        _cameraPitch -= pitchDelta; // Invertir para un movimiento natural
        _cameraPitch = Mathf.Clamp(_cameraPitch, -90f, 90f); // Limitar el ángulo de la cámara

        // Aplicar la rotación a la cámara
        cameraTransform.localEulerAngles = Vector3.right * _cameraPitch;
    }

    // Métodos para el nuevo sistema de input
    public void OnMove(InputAction.CallbackContext context)
    {
        _moveInput = context.ReadValue<Vector2>();
    }

    public void OnLook(InputAction.CallbackContext context)
    {
        _lookInput = context.ReadValue<Vector2>();
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        if (context.performed && characterController.isGrounded)
        {
            _isJumping = true;
        }
        else if (context.canceled)
        {
            _isJumping = false;
        }
    }

    public void OnSprint(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            _isSprinting = true;
        }
        else if (context.canceled)
        {
            _isSprinting = false;
        }
    }
}