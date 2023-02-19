using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Timeline;

public class VerticalMovement : MonoBehaviour
{
    [SerializeField] private float minSpeed = 0.1f;
    [SerializeField] private float maxSpeed = 0.3f;
    [SerializeField] private float minOffset = 2;
    [SerializeField] private float maxOffset = 5;
    
    private float _speed;
    private float _offset;
    private bool _isGoingUp;
    private Vector3 _positionTop;
    private Vector3 _positionBottom;

    // Start is called before the first frame update
    void Start()
    {
        _speed = Random.Range(minSpeed, maxSpeed);
        _offset = Random.Range(minOffset, maxOffset);
        _isGoingUp = (Random.Range(0.0f, 1.0f) < 0.5f) ? true : false;
        
        _positionTop = transform.position + _offset * transform.forward;
        _positionBottom = transform.position - _offset * transform.forward;
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 movement;
        
        if (_isGoingUp)
        {
            movement = transform.forward;
        }

        else
        {
            movement = transform.forward * -1;
        }

        transform.position += movement * (_speed * Time.deltaTime);
        
        if (transform.position.y - _positionTop.y > 0f)
        {
            _isGoingUp = false;
        }
        
        else if (transform.position.y - _positionBottom.y < 0f)
        {
            _isGoingUp = true;
        }
    }
    
}
