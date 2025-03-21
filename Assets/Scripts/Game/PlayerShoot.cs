using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerShoot : MonoBehaviour
{
    [SerializeField] private GameObject _trailRenderer;
    [SerializeField] private GameObject _bulletPrefab;
    [SerializeField] private Transform _bulletOrigin;

    private Camera _camera;

    private PlayerInput _playerInput;
    private PhotonView _photonView;

    private bool _isAiming;

    private void Awake()
    {
        _playerInput = new PlayerInput();
        _photonView = GetComponent<PhotonView>();
    }

    private void OnEnable()
    {
        _playerInput.Enable();
    }

    private void OnDisable()
    {
        _playerInput.Disable();
    }

    private void Start()
    {
        _camera = Camera.main;

        if (_photonView.IsMine)
        {
            _playerInput.PlayerKeyboardInput.Aim.started += OnAimChanged;
            _playerInput.PlayerKeyboardInput.Aim.canceled += OnAimChanged;
            _playerInput.PlayerKeyboardInput.Fire.started += Shoot;
        }
    }

    private void Update()
    {
        Aim();
    }

    private void OnAimChanged(InputAction.CallbackContext context)
    {
        _isAiming = context.ReadValueAsButton();
        _trailRenderer.SetActive(_isAiming);
    }

    private void Aim()
    {
        if (_isAiming)
        {
            Ray ray = _camera.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out RaycastHit hitInfo))
            {
                Vector3 point = new Vector3(hitInfo.point.x, transform.position.y, hitInfo.point.z);
                Vector3 direction = point - transform.position;
                _trailRenderer.transform.forward = direction;
            }
        }
    }

    private void Shoot(InputAction.CallbackContext context)
    {
        if (_isAiming)
        {
            GameObject bullet = PhotonNetwork.Instantiate("Bullet", _bulletOrigin.position, _trailRenderer.transform.rotation);
            bullet.GetComponentInChildren<Collider>().enabled = true;
        }
    }
}
