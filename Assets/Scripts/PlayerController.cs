using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [Header("Movement Settings")]
    public float walkSpeed = 8f;
    public float sprintSpeed = 10f;
    public float jumpForce = 1.5f; // Ajustado para un salto más controlado
    public float gravity = -25f;   // Gravedad base
    public float gravityAcceleration = 2f; // Aceleración de la gravedad
    public float lookSensitivity = 2f;

    [Header("Weapons")]
    public BioGun bioGun;
    public OrganThrow organThrow;
    private Weapon _currentWeapon;

    [Header("References")]
    public Transform cameraTransform;
    public CharacterController characterController;

    [Header("UI Elements")]
    public TextMeshProUGUI bioAmmoText;
    public TextMeshProUGUI organAmmoText;

    private PlayerInput _playerInput;

    private Vector2 _moveInput;
    private Vector2 _lookInput;
    private bool _isJumping;
    private bool _isSprinting;
    private Vector3 _velocity;
    private float _cameraPitch = 0f;

    // Animaciones
    Animator anim;



    private void Awake()
    {
        // 🔹 Ocultar y bloquear el cursor antes de que el juego comience
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void Start()
    {
        // Inicializar con el arma biológica
        _currentWeapon = bioGun;
        Debug.Log("Arma inicial: BioGun");

        // 🔹 Ocultar y bloquear el cursor al iniciar el juego
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        // 🔹 Obtener referencia al Input System
        _playerInput = GetComponent<PlayerInput>();

        // 🔹 Vincular la acción ToggleCursor con el método que desbloquea el cursor
        _playerInput.actions["ToggleCursor"].performed += ctx => ToggleCursor();

        UpdateAmmoUI(); // 🔹 Se actualiza la UI al inicio

        // 🔹 obtener animator del hijo
        anim = GetComponentInChildren<Animator>();
    }

    private void Update()
    {
        HandleMovement();
        HandleRotation();

        // 🔹 Asegurar que el cursor siga oculto al inicio
        if (Time.timeSinceLevelLoad < 0.1f) // Solo en los primeros 0.1s del juego
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }


    private void HandleMovement()
    {
        float speed ;
        if (_isSprinting){
            anim.SetFloat("MovementA", 1f); //Animacion estar
            speed =sprintSpeed;
        }else{

            speed = walkSpeed;
        }

        Vector3 moveDirection = (transform.forward * _moveInput.y + transform.right * _moveInput.x) * speed;

        if (characterController.isGrounded)
        {
            _velocity.y = -2f; // Pequeña fuerza hacia abajo para mantener al jugador pegado al suelo
            if (_isJumping)
            {
                _velocity.y = Mathf.Sqrt(jumpForce * -2f * gravity); // Aplicar la fuerza del salto
            }
        }
        else
        {
            // Aplicar aceleración de la gravedad
            _velocity.y += gravity * gravityAcceleration * Time.deltaTime;
        }

        characterController.Move((moveDirection + _velocity) * Time.deltaTime);
    }

    private void ToggleCursor()
    {
        if (Cursor.lockState == CursorLockMode.Locked)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
        else
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }

    private void UpdateAmmoUI()
    {
        if (bioAmmoText != null && bioGun != null)
        {
            bioAmmoText.text = bioGun.ammo + " / " + bioGun.maxAmmo;
        }

        if (organAmmoText != null && organThrow != null)
        {
            organAmmoText.text = organThrow.ammo + " / " + organThrow.maxAmmo;
        }
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

        if (_moveInput == Vector2.zero)
        {
            anim.SetFloat("MovementA", 0f);
        }else{
            anim.SetFloat("MovementA", 0.4f);
        }
    }

    public void OnLook(InputAction.CallbackContext context)
    {
        _lookInput = context.ReadValue<Vector2>();
    }

    public void OnJump(InputAction.CallbackContext context)
    {

        if (context.performed && characterController.isGrounded && !_isJumping)
        {
            anim.SetTrigger("Jump");
            _isJumping = true;
            
        }
   

        if(!characterController.isGrounded)
        {
            anim.SetTrigger("Landen");
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
            if (_moveInput != Vector2.zero) // Si se está moviendo pero no corriendo
            {
                anim.SetFloat("MovementA", 0.3f); // Animación de caminar
            }
            else
            {
                anim.SetFloat("MovementA", 0f); // Animación de estar en reposo
            }
        }
    }

    public void OnShoot(InputAction.CallbackContext context)
    {
        if (context.performed && _currentWeapon != null)
        {
            Transform firePoint = null;

            if (_currentWeapon is BioGun)
            {
                firePoint = ((_currentWeapon as BioGun).firePoint);
            }
            else if (_currentWeapon is OrganThrow)
            {
                firePoint = ((_currentWeapon as OrganThrow).firePoint);
            }

            if (firePoint == null) return;

            Ray ray = new Ray(cameraTransform.position, cameraTransform.forward);
            RaycastHit hit;
            Vector3 shootDirection;

            if (Physics.Raycast(ray, out hit, 100f))
            {
                shootDirection = (hit.point - firePoint.position).normalized;
            }
            else
            {
                shootDirection = cameraTransform.forward;
            }

            _currentWeapon.Shoot(shootDirection);
            UpdateAmmoUI(); // 🔹 Se actualiza la UI después de disparar
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
            UpdateAmmoUI(); // 🔹 Se actualiza la UI después de recargar
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


    public void Win()
    {
        Debug.Log("Jugador ha ganado");

        // 🔹 Notificar a `GameManager`
        GameManager.Instance.ShowWinScreen();
    }

}