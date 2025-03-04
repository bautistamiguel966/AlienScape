using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [Header("Movement Settings")]
    public float walkSpeed = 5f;
    public float sprintSpeed = 8f;
    public float jumpForce = 1.5f;
    public float gravity = -25f;
    public float lookSensitivity = 2f;

    [Header("Weapons")]
    public BioGun bioGun;
    public OrganThrow organThrow;
    private Weapon _currentWeapon;

    [Header("References")]
    public Transform cameraTransform;
    public CharacterController characterController;

    private Vector2 _moveInput;
    private Vector2 _lookInput;
    private bool _isJumping;
    private bool _isSprinting;
    private Vector3 _velocity;
    private float _cameraPitch = 0f;

    private void Start()
    {
        // Inicializar con el arma biológica
        _currentWeapon = bioGun;
        Debug.Log("Arma inicial: BioGun");
    }

    private void Update()
    {
        HandleMovement();
        HandleRotation();
    }

    private void HandleMovement()
    {
        float speed = _isSprinting ? sprintSpeed : walkSpeed;
        Vector3 moveDirection = (transform.forward * _moveInput.y + transform.right * _moveInput.x) * speed;

        if (characterController.isGrounded)
        {
            _velocity.y = -2f;
            if (_isJumping)
            {
                _velocity.y = Mathf.Sqrt(jumpForce * -2f * gravity);
            }
        }
        else
        {
            _velocity.y += gravity * Time.deltaTime;
        }

        characterController.Move((moveDirection + _velocity) * Time.deltaTime);
    }

    private void HandleRotation()
    {
        float yaw = _lookInput.x * lookSensitivity;
        transform.Rotate(Vector3.up, yaw);

        float pitchDelta = _lookInput.y * lookSensitivity;
        _cameraPitch -= pitchDelta;
        _cameraPitch = Mathf.Clamp(_cameraPitch, -90f, 90f);

        cameraTransform.localEulerAngles = Vector3.right * _cameraPitch;
    }

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

    public void OnShoot(InputAction.CallbackContext context)
    {
        if (context.performed && _currentWeapon != null)
        {
            _currentWeapon.Shoot();
        }
    }

    public void OnSwitchWeapon(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            Debug.Log("SwitchWeapon action performed");

            // Cambiar de arma basado en la rueda del mouse o la tecla Q
            if (context.control.displayName == "Scroll" || context.control.displayName == "Q")
            {
                SwitchWeapon();
            }
        }
    }

    public void OnReload(InputAction.CallbackContext context)
    {
        if (context.performed && _currentWeapon != null)
        {
            _currentWeapon.Reload();
        }
    }

    private void SwitchWeapon()
    {
        if (_currentWeapon == bioGun)
        {
            _currentWeapon = organThrow;
            Debug.Log("Cambiado a lanzamiento de órgano");
        }
        else
        {
            _currentWeapon = bioGun;
            Debug.Log("Cambiado a arma biológica");
        }
    }
}