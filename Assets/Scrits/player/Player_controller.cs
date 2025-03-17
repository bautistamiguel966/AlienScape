using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player_Controller : MonoBehaviour
{
    private CharacterController _controller;
    private GameObject _cam;
    
    [Header("Estadísticas Normales")]
    [SerializeField] private float _velocity;
    [SerializeField] private float _heightJump;
    [SerializeField] private float _timeRotation;
  
    [Header("Datos sobre el piso")]
    [SerializeField] private Transform _floorDetect;
    [SerializeField] private float _distanceFromFloor;
    [SerializeField] private LayerMask _maskFloor;

    float VelocityRotation;
    float gravityForce = -9.81f;
    Vector3 vectorVelocity;
    bool isGrounded;

    private void Start()
    {
        _controller = GetComponent<CharacterController>();
        _cam = GameObject.FindGameObjectWithTag("MainCamera");
        
    }

    private void Update()
    {
        isGrounded = Physics.CheckSphere(_floorDetect.position, _distanceFromFloor, _maskFloor);
        Debug.Log(isGrounded);

        // Resetea la velocidad en Y si está en el suelo
        if (isGrounded && vectorVelocity.y < 0)
        {
            vectorVelocity.y = -2f;
        }

        // Salto 
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            vectorVelocity.y = Mathf.Sqrt(_heightJump * -2 * gravityForce);
        }

        // Aplicar gravedad (negativa para que caiga)
        vectorVelocity.y += gravityForce * Time.deltaTime;
        _controller.Move(vectorVelocity * Time.deltaTime);

        _controller.Move(vectorVelocity * Time.deltaTime);
    }
}
