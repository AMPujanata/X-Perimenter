 using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBehavior : MonoBehaviour
{
    private bool _canMove;
    [SerializeField] private float _moveSpeed;
    private BoxCollider2D _2DCollider;
    [SerializeField] private FixedJoystick _joystick;
    [SerializeField] private BoxCollider2D _currentBound;
    private float _boundLeft;
    private float _boundRight;
    private float _boundUp;
    private float _boundDown;
    private float _2DColliderBoundLeft;
    private float _2DColliderBoundRight;
    private float _2DColliderBoundUp;
    private float _2DColliderBoundDown;
    void Awake()
    {
        _canMove = true;
        _2DCollider = GetComponent<BoxCollider2D>();
        _boundLeft = _currentBound.bounds.min.x;
        _boundRight = _currentBound.bounds.max.x;
        _boundUp = _currentBound.bounds.max.y;
        _boundDown = _currentBound.bounds.min.y;
        _2DColliderBoundLeft = _2DCollider.bounds.min.x;
        _2DColliderBoundRight = _2DCollider.bounds.max.x;
        _2DColliderBoundUp = _2DCollider.bounds.max.y;
        _2DColliderBoundDown = _2DCollider.bounds.min.y;
    }

    // Update is called once per frame
    void Update()
    {
        if(_canMove)
        {
            transform.position += Vector3.right * _joystick.Horizontal * _moveSpeed * Time.deltaTime;
            transform.position += Vector3.up * _joystick.Vertical * _moveSpeed * Time.deltaTime;
            Vector3 position = transform.position;
            //code for not moving out of bounds here
            if (position.x < _boundLeft - _2DColliderBoundLeft)
                position.x = _boundLeft - _2DColliderBoundLeft;
            else if (position.x > _boundRight - _2DColliderBoundRight)
                position.x = _boundRight - _2DColliderBoundRight;
            if (position.y < _boundDown - _2DColliderBoundDown)
                position.y = _boundDown - _2DColliderBoundDown;
            else if (position.y > _boundUp - _2DColliderBoundUp)
                position.y = _boundUp - _2DColliderBoundUp;
            transform.position = position;
        }
    }

    public void CheckForInteraction() //currently only interacts with the first found object in the array
    {
        Collider2D[] overlap = Physics2D.OverlapAreaAll(_2DCollider.bounds.min, _2DCollider.bounds.max);
        for (int i = 0; i < overlap.Length; i++)
        {
            if (overlap[i].gameObject == gameObject)
                continue;

            InteractionObject overlapInteraction;
            overlapInteraction = overlap[i].gameObject.GetComponent<InteractionObject>();
            if (overlapInteraction != null)
            {
                overlapInteraction.Interact();
            }
        }
    }

    public void ToggleMovement(bool shouldMove)
    {
        _canMove = shouldMove;
    }

    public void SetBounds(BoxCollider2D bound)
    {
        _currentBound = bound;
        _boundLeft = _currentBound.bounds.min.x;
        _boundRight = _currentBound.bounds.max.x;
        _boundUp = _currentBound.bounds.max.y;
        _boundDown = _currentBound.bounds.min.y;
    }
}
