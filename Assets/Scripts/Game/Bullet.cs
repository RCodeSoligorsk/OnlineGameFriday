using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private float _speed = 8.0f;
    private Rigidbody _rigidbody;
    private bool _isLaunched = false;

    private void Start()
    {
        Initialize();
    }

    private void Initialize()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _rigidbody.AddForce(transform.forward * _speed, ForceMode.Impulse);
        Destroy(gameObject, 8.0f / _speed);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out IDamageable damageable) && _isLaunched)
        {
            damageable.TakeDamage(8);
            PhotonNetwork.Destroy(gameObject);
        }

        if (_isLaunched == false)
        {
            _isLaunched = true; //Прикол
        }
    }
}
