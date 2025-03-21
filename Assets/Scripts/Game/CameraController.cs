using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{

    public static CameraController instance;

    private Transform _target;

    [SerializeField] private float _smooth = 2.0f;
    private Vector3 _velocity;

    [SerializeField] private Vector3 _offset = new Vector3(0f, 5f, -4f);

    private void Awake()
    {
        instance = this;
    }

    private void Update()
    {
        Work();
    }

    public void SetTarget(Transform target)
    {
        _target = target;
    }

    private void Work()
    {
        if (_target == null)
            return;
        
        Vector3 newPosition = Vector3.SmoothDamp(transform.position, _target.position + _offset, ref _velocity, _smooth);
        transform.position = newPosition;
    }
}
