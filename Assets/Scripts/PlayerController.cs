using UnityEngine;
using TMPro;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [Header("Movement Settings")]
    public float walkSpeed = 8f;
    public float sprintSpeed = 10f;
    public float jumpForce = 1.5f;
    public float gravity = -25f;
    public float gravityAcceleration = 2f;
    public float lookSensitivity = 2f;

    [Header("Weapons")]
    public BioGun bioGun;
    public OrganThrow organThrow;
    private Weapon _currentWeapon;

    [Header("Weapon Models")]
    public GameObject bioGunModel;
    public GameObject organProjectileModel;

    [Header("References")]
    public Transform cameraTransform;
    public CharacterController characterController;

    [Header("UI Elements")]
    public TextMeshProUGUI bioAmmoText;
    public TextMeshProUGUI organAmmoText;


    [Header("Audio")]
    public AudioSource audioSource;
    public AudioClip[] walkSounds;
    public AudioClip[] sprintSounds;
    public AudioClip jumpSound;
    public AudioClip reloadSound;
    public AudioClip weaponSwitchSound;
    public float walkStepInterval = 0.6f;
    public float sprintStepInterval = 0.4f;

    private PlayerInput _playerInput;
    private Vector2 _moveInput;
    private Vector2 _lookInput;
    private bool _isJumping;
    private bool _isSprinting;
    private bool _isShooting;
    private Vector3 _velocity;
    private float _cameraPitch = 0f;
    private float nextFootstepTime = 0f;
    private float nextFireTime = 0f;

    private void Awake()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void Start()
    {
        _currentWeapon = bioGun;
        Debug.Log("Arma inicial: BioGun");

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        _playerInput = GetComponent<PlayerInput>();
        _playerInput.actions["ToggleCursor"].performed += ctx => ToggleCursor();

        bioGun.gameObject.SetActive(true);
        organThrow.gameObject.SetActive(false);

        if (organProjectileModel != null)
        {
            organProjectileModel.SetActive(false);
        }

        UpdateAmmoUI();
    }

    private void Update()
    {
        HandleMovement();
        HandleRotation();
        HandleFootsteps();
        HandleShooting();

        if (Time.timeSinceLevelLoad < 0.1f)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
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
            _velocity.y += gravity * gravityAcceleration * Time.deltaTime;
        }

        characterController.Move((moveDirection + _velocity) * Time.deltaTime);
    }

    private void HandleFootsteps()
    {
        if (!characterController.isGrounded || _moveInput.magnitude == 0) return;

        float stepInterval = _isSprinting ? sprintStepInterval : walkStepInterval;
        AudioClip[] selectedSounds = _isSprinting ? sprintSounds : walkSounds;

        if (Time.time >= nextFootstepTime)
        {
            nextFootstepTime = Time.time + stepInterval;

            if (selectedSounds.Length > 0 && audioSource != null)
            {
                AudioClip stepSound = selectedSounds[Random.Range(0, selectedSounds.Length)];
                audioSource.PlayOneShot(stepSound);
            }
        }
    }

    private void HandleShooting()
    {
        if (_isShooting && Time.time >= nextFireTime)
        {
            ShootWeapon();
            nextFireTime = Time.time + (1f / bioGun.fireRate);
        }
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

    private void ShootWeapon()
    {
        if (_currentWeapon == null) return;

        Transform firePoint = _currentWeapon.firePoint;
        if (firePoint == null) return;

        Ray ray = new Ray(cameraTransform.position, cameraTransform.forward);
        RaycastHit hit;
        Vector3 shootDirection = Physics.Raycast(ray, out hit, 100f)
            ? (hit.point - firePoint.position).normalized
            : cameraTransform.forward;

        _currentWeapon.Shoot(shootDirection);
        UpdateAmmoUI();
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

            if (audioSource != null && jumpSound != null)
            {
                audioSource.PlayOneShot(jumpSound);
            }
        }
        else if (context.canceled)
        {
            _isJumping = false;
        }
    }

    public void OnSprint(InputAction.CallbackContext context)
    {
        _isSprinting = context.performed;
    }

    public void OnShoot(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            _isShooting = true;
        }
        else if (context.canceled)
        {
            _isShooting = false;
        }
    }

    public void OnSwitchWeapon(InputAction.CallbackContext context)
    {
        if (context.performed && (context.control.displayName == "Scroll" || context.control.displayName == "Q"))
        {
            SwitchWeapon();
        }
    }

    public void OnReload(InputAction.CallbackContext context)
    {
        if (context.performed && _currentWeapon != null)
        {
            _currentWeapon.Reload();
            UpdateAmmoUI();

            // 🔹 Sonido de recarga
            if (audioSource != null && reloadSound != null)
            {
                audioSource.PlayOneShot(reloadSound);
            }
        }
    }

    private void SwitchWeapon()
    {
        bool isSwitchingToOrganThrow = _currentWeapon == bioGun;
        _currentWeapon = isSwitchingToOrganThrow ? organThrow : bioGun;
        Debug.Log($"Cambiado a {_currentWeapon.GetType().Name}");

        bioGunModel.SetActive(!isSwitchingToOrganThrow);
        organProjectileModel.SetActive(isSwitchingToOrganThrow);

        if (audioSource != null && weaponSwitchSound != null)
        {
            audioSource.PlayOneShot(weaponSwitchSound);
        }
    }

    public void Win()
    {
        Debug.Log("Jugador ha ganado");
        GameManager.Instance.ShowWinScreen();
    }
}
