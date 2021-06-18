using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PilaView : MonoBehaviour
{
    [SerializeField] private Transform _startPosition;
    [SerializeField] private Transform _endPosition;
    [SerializeField] private float _speed = 5f;
    [SerializeField] private float _rotatingSpeed = 5f;
    [SerializeField] private bool _moveToEnd = true;

    public void Awake()
    {
        transform.position = _startPosition.position;

    }

    public void FixedUpdate()
    {
        transform.Rotate(0, 1f * _rotatingSpeed, 0);
        if (_moveToEnd)
        {
            transform.position = Vector3.MoveTowards(transform.position, _endPosition.position, Time.deltaTime * _speed);
            if ((transform.position - _endPosition.position).magnitude < 0.05f)
            {
                _moveToEnd = false;
            }
        }
        if (!_moveToEnd)
        {
            transform.position = Vector3.MoveTowards(transform.position, _startPosition.position, Time.deltaTime * _speed);
            if ((transform.position - _startPosition.position).magnitude < 0.05f)
            {
                _moveToEnd = true;
            }
        }
        
    }
}
