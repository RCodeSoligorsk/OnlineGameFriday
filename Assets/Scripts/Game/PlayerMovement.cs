using Photon.Pun;
using System;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
[RequireComponent(typeof(Animator))]
public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float _runSpeed = 5.0f;
    [SerializeField] private float _walkSpeed = 2.0f;
    private float _speed;
    [SerializeField] private float _rotationSpeed = 6.0f;

    private CharacterController _characterController;
    private Animator _animator;
    private PhotonView _photonView;

    private PlayerInput _playerInput;
    private Vector2 _movementVector;
    private bool _isSprinting;

    private Vector3 _desiredVelocity;
    private Vector3 _velocity;

    private void Awake()
    {
        _characterController = GetComponent<CharacterController>();
        _animator = GetComponent<Animator>();
        _photonView = GetComponent<PhotonView>();

        _playerInput = new PlayerInput();

        _playerInput.PlayerKeyboardInput.Movement.started += OnPlayerMove;
        _playerInput.PlayerKeyboardInput.Movement.canceled += OnPlayerMove;
        _playerInput.PlayerKeyboardInput.Movement.performed += OnPlayerMove;

        _playerInput.PlayerKeyboardInput.Sprint.started += OnPlayerSprint;
        _playerInput.PlayerKeyboardInput.Sprint.canceled += OnPlayerSprint;
    }

    private void Start()
    {
        if (_photonView.IsMine)
        {
            CameraController.instance.SetTarget(transform);
        }
    }

    private void OnEnable()
    {
        _playerInput.Enable();
    }

    private void OnDisable()
    {
        _playerInput.Disable();
    }

    private void OnPlayerMove(InputAction.CallbackContext context)
    {
        _movementVector = context.ReadValue<Vector2>();
    }

    private void OnPlayerSprint(InputAction.CallbackContext context)
    {
        _isSprinting = context.ReadValueAsButton();
    }

    private void Update()
    {
        if (_photonView.IsMine)
        {
            Move();
            Rotate();
        }
    }

    private void Move()
    {
        _speed = _isSprinting ? _runSpeed : _walkSpeed;

        _desiredVelocity = new Vector3(_movementVector.x * _speed, 0, _movementVector.y * _speed) * Time.deltaTime;
        _velocity = Vector3.Lerp(_velocity, _desiredVelocity, Time.deltaTime * _speed);
        _characterController.Move(_velocity);
        _animator.SetFloat("Speed", _movementVector.magnitude * _speed);
    }

    private void Rotate()
    {
        if (_movementVector.magnitude > 0)
        {
            Quaternion targetRotation = Quaternion.LookRotation(_velocity, Vector3.up);
            Quaternion rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.deltaTime * _rotationSpeed);
            transform.rotation = rotation;
        }
    }
}
