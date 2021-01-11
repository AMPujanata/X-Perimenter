using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private Transform _playerTransform;
    [SerializeField] private Vector3 _offset;
    [SerializeField] private BoxCollider2D _startingBound;
    private bool _cameraTooBigLR;
    private bool _cameraTooBigUD;
    private float _aspect;
    private float _orthographicSize;
    private float _boundLeft;
    private float _boundRight;
    private float _boundUp;
    private float _boundDown;

    void Awake()
    {
        _aspect = Camera.main.aspect;
        _orthographicSize = Camera.main.orthographicSize;
        SetBounds(_startingBound);
    }

    void LateUpdate()
    {
        transform.position = new Vector3(_playerTransform.position.x + _offset.x, _playerTransform.position.y + _offset.y, _offset.z);
        Vector3 position = transform.position;
        //centers the camera between the two bounds if it's too big
        if (_cameraTooBigLR)
            position.x = (_boundLeft + _boundRight) / 2;
        else
        {
            if (position.x < _boundLeft + _orthographicSize * _aspect)
                position.x = _boundLeft + _orthographicSize * _aspect;
            else if (position.x > _boundRight - _orthographicSize * _aspect)
                position.x = _boundRight - _orthographicSize * _aspect;
        }
        if (_cameraTooBigUD)
            position.y = (_boundUp + _boundDown) / 2;
        else
        {
            if (position.y < _boundDown + _orthographicSize)
                position.y = _boundDown + _orthographicSize;
            else if (position.y > _boundUp - _orthographicSize)
                position.y = _boundUp - _orthographicSize;
        }
        transform.position = position;
    }

    public void SetBounds(BoxCollider2D bound)
    {
        _boundLeft = bound.bounds.min.x;
        _boundRight = bound.bounds.max.x;
        _boundUp = bound.bounds.max.y;
        _boundDown = bound.bounds.min.y;
        float _boundSizeLR = _boundRight - _boundLeft;
        float _boundSizeUD = _boundUp - _boundDown;
        if (_orthographicSize * _aspect * 2 >= _boundSizeLR)
        {
            _cameraTooBigLR = true;
        }
        else
        {
            _cameraTooBigLR = false;
        }
        if (_orthographicSize * 2 >= _boundSizeUD)
        {
            _cameraTooBigUD = true;
        }
        else
        {
            _cameraTooBigUD = false;
        }
    }
    public void SetBounds(float boundLeft, float boundRight, float boundUp, float boundDown)
    {
        _boundLeft = boundLeft;
        _boundRight = boundRight;
        _boundUp = boundUp;
        _boundDown = boundDown;
        float _boundSizeLR = _boundRight - _boundLeft;
        float _boundSizeUD = _boundUp - _boundDown;
        if(_orthographicSize * _aspect * 2 >= _boundSizeLR)
        {
            _cameraTooBigLR = true;
        }
        else
        {
            _cameraTooBigLR = false;
        }
        if(_orthographicSize * 2 >= _boundSizeUD)
        {
            _cameraTooBigUD = true;
        }
        else
        {
            _cameraTooBigUD = false;
        }
    }
}
