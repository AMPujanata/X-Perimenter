 using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum MovementType
{
    FREE, GRID
};

public enum Direction
{
    LEFT, RIGHT, UP, DOWN
};

[System.Serializable]
public class PlayerSpriteSet
{
    public Sprite idleSprite;
    public Sprite leftFootSprite;
    public Sprite rightFootSprite;
}

public class PlayerBehavior : MonoBehaviour
{
    private bool _canMove;
    [SerializeField] private float _freeMoveSpeed;
    private BoxCollider2D _2DCollider;
    [SerializeField] private FixedJoystick _joystick;
    [SerializeField] private BoxCollider2D _startingBound;
    [SerializeField] private MovementType _movementType;
    [SerializeField] private PersonalDPad _dPad;
    [SerializeField] private float _tileSize;
    [SerializeField] private float _gridMoveSpeed;
    [SerializeField] private float _timePerTile;
    [SerializeField] private Direction _startingDirection;
    [SerializeField] private PlayerSpriteSet _leftSprite;
    [SerializeField] private PlayerSpriteSet _rightSprite;
    [SerializeField] private PlayerSpriteSet _upSprite;
    [SerializeField] private PlayerSpriteSet _downSprite;
    private Direction _currentFoot;
    private PlayerSpriteSet _currentSpriteSet;
    private SpriteRenderer _spriteRenderer;
    private MovementTrigger _lastMovementTrigger;
    private bool _isPlayerMoving;
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
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _spriteRenderer.sprite = DirectionToSpriteConverter(_startingDirection).idleSprite;
        _currentFoot = Direction.LEFT;
        _canMove = true;
        _isPlayerMoving = false;
        _2DCollider = GetComponent<BoxCollider2D>();
        SetBounds(_startingBound);
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
            if(_movementType == MovementType.FREE)
            {
                if (!_isPlayerMoving)
                {
                    if (_joystick.Vertical > 0)
                    {
                        StartCoroutine(MoveToTile(Direction.UP));
                    }
                    else if (_joystick.Horizontal > 0)
                    {
                        StartCoroutine(MoveToTile(Direction.RIGHT));
                    }
                    else if (_joystick.Horizontal < 0)
                    {
                        StartCoroutine(MoveToTile(Direction.LEFT));
                    }
                    else if (_joystick.Vertical < 0)
                    {
                        StartCoroutine(MoveToTile(Direction.DOWN));
                    }
                }
                /*transform.position += Vector3.right * _joystick.Horizontal * _freeMoveSpeed * Time.deltaTime;
                transform.position += Vector3.up * _joystick.Vertical * _freeMoveSpeed * Time.deltaTime;
                Vector3 position = transform.position;
                //code for not moving out of bounds here
                if (position.x < _boundLeft - _2DColliderBoundLeft)
                    position.x = _boundLeft - _2DColliderBoundLeft;
                else if (position.x > _boundRight - _2DColliderBoundRight)
                    position.x = _boundRight - _2DColliderBoundRight;
                if (position.y < _boundDown - _2DColliderBoundUp) // ???? why doesnt it work with bound down? i hate this holy shit
                    position.y = _boundDown - _2DColliderBoundUp;
                else if (position.y > _boundUp + _2DColliderBoundUp)
                    position.y = _boundUp + _2DColliderBoundUp;
                transform.position = position;*/
            }
            else if(_movementType == MovementType.GRID)
            {
                if (!_isPlayerMoving)
                {
                    if (_dPad.GetVerticalInput() > 0)
                    {
                        StartCoroutine(MoveToTile(Direction.UP));
                    }
                    else if (_dPad.GetHorizontalInput() > 0)
                    {
                        StartCoroutine(MoveToTile(Direction.RIGHT));
                    }
                    else if (_dPad.GetHorizontalInput() < 0)
                    {
                        StartCoroutine(MoveToTile(Direction.LEFT));
                    }
                    else if (_dPad.GetVerticalInput() < 0)
                    {
                        StartCoroutine(MoveToTile(Direction.DOWN));
                    }
                }
            }
        }
    }

    public void CheckForInteraction() //currently only interacts with the first found object in the array
    {
        if (!_canMove)
            return;
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
                break;
            }
        }
    }

    public IEnumerator MoveToTile(Direction _direction)
    {
        _isPlayerMoving = true;

        Vector2 dir = Vector2.up;
        if(_direction == Direction.LEFT)
        {
            dir = Vector2.left;
        }
        else if (_direction == Direction.RIGHT)
        {
            dir = Vector2.right;
        }
        else if (_direction == Direction.UP)
        {
            dir = Vector2.up;
        }
        else if (_direction == Direction.DOWN)
        {
            dir = Vector2.down;
        }
        _currentSpriteSet = DirectionToSpriteConverter(_direction);

        Vector2 startPos = new Vector2(transform.position.x, transform.position.y);
        Vector2 endPos = startPos + (dir * _tileSize);
        float time = 0;

        if (endPos.x < _boundLeft)
        {
            _isPlayerMoving = false;
            _spriteRenderer.sprite = _currentSpriteSet.idleSprite;
            yield break;
        }
        else if (endPos.x > _boundRight)
        {
            _isPlayerMoving = false;
            _spriteRenderer.sprite = _currentSpriteSet.idleSprite;
            yield break;
        }
        else if (endPos.y < _boundDown) // ???? why doesnt it work with bound down? i hate this holy shit
        {
            _isPlayerMoving = false;
            _spriteRenderer.sprite = _currentSpriteSet.idleSprite;
            yield break;
        }
        else if (endPos.y > _boundUp)
        {
            _isPlayerMoving = false;
            _spriteRenderer.sprite = _currentSpriteSet.idleSprite;
            yield break;
        }
        int layerMask = 1 << 8;
        layerMask = ~layerMask; //doesn't collide against layer 8, aka IgnoreRaycast (ignore the fact there are 2 ignoreraycast layers, i didnt check)

        //rudimentary check to see if endPos is on a passable tile or not
        RaycastHit2D hit = Physics2D.Raycast(transform.position, dir, Mathf.Infinity, layerMask);

        if (hit.collider != null)
        {
            if (hit.distance > _tileSize)
            {
                if (_currentFoot == Direction.LEFT)
                    _spriteRenderer.sprite = _currentSpriteSet.leftFootSprite;
                else
                    _spriteRenderer.sprite = _currentSpriteSet.rightFootSprite;
                while (Vector2.Distance(transform.position, endPos) > 0.001f)
                {
                    if (Vector2.Distance(transform.position, endPos) <= (_tileSize / 2))
                        _spriteRenderer.sprite = _currentSpriteSet.idleSprite;
                    time += Time.deltaTime;
                    transform.position = Vector2.MoveTowards(transform.position, endPos, _gridMoveSpeed * Time.deltaTime);
                    yield return null;
                }
                if (_currentFoot == Direction.LEFT)
                    _currentFoot = Direction.RIGHT;
                else
                    _currentFoot = Direction.LEFT;
            }
        }
        else
        {
            if (_currentFoot == Direction.LEFT)
                _spriteRenderer.sprite = _currentSpriteSet.leftFootSprite;
            else
                _spriteRenderer.sprite = _currentSpriteSet.rightFootSprite;
            while (Vector2.Distance(transform.position, endPos) > 0.001f)
            {
                if (Vector2.Distance(transform.position, endPos) <= (_tileSize / 2))
                    _spriteRenderer.sprite = _currentSpriteSet.idleSprite;
                time += Time.deltaTime;
                transform.position = Vector2.MoveTowards(transform.position, endPos, _gridMoveSpeed * Time.deltaTime);
                yield return null;
            }
            if (_currentFoot == Direction.LEFT)
                _currentFoot = Direction.RIGHT;
            else
                _currentFoot = Direction.LEFT;
        }
        _isPlayerMoving = false;
        _spriteRenderer.sprite = _currentSpriteSet.idleSprite; 
        //following code snippet is checking for movement trigger interactions. may be laggy
        Collider2D[] overlap = Physics2D.OverlapAreaAll(_2DCollider.bounds.min, _2DCollider.bounds.max);
        MovementTrigger movementTrigger = null;
        for (int i = 0; i < overlap.Length; i++)
        {
            if (overlap[i].gameObject == gameObject)
                continue;

            movementTrigger = overlap[i].gameObject.GetComponent<MovementTrigger>();
            if (movementTrigger != null)
            {
                if(movementTrigger != _lastMovementTrigger)
                    movementTrigger.Interact();
                _lastMovementTrigger = movementTrigger;
                break;
            }
        }
        _lastMovementTrigger = movementTrigger;
        yield break;
    }

    public void ToggleMovement(bool shouldMove)
    {
        _canMove = shouldMove;
        _joystick.ResetInput();
        _dPad.ResetInput();
    }

    public void SetBounds(BoxCollider2D bound)
    {
        _boundLeft = bound.bounds.min.x;
        _boundRight = bound.bounds.max.x;
        _boundUp = bound.bounds.max.y;
        _boundDown = bound.bounds.min.y;
    }

    public void SetBounds (float boundLeft, float boundRight, float boundUp, float boundDown)
    {

        _boundLeft = boundLeft;
        _boundRight = boundRight;
        _boundUp = boundUp;
        _boundDown = boundDown;
    }

    public float GetBound(string direction)
    {
        if (direction == "left")
            return _boundLeft;
        else if (direction == "right")
            return _boundRight;
        else if (direction == "up")
            return _boundUp;
        else if (direction == "down")
            return _boundDown;
        else
        {
            Debug.Log("Error reading bound string (wrong identifier?)");
            return 0;
        }
    }

    public void SetDirection(Direction _direction)
    {
        _currentSpriteSet = DirectionToSpriteConverter(_direction);
        _spriteRenderer.sprite = _currentSpriteSet.idleSprite;
    }
    public PlayerSpriteSet DirectionToSpriteConverter(Direction _direction)
    {
        if (_direction == Direction.LEFT)
        {
            return _leftSprite;
        }
        else if (_direction == Direction.RIGHT)
        {
            return _rightSprite;
        }
        else if (_direction == Direction.UP)
        {
            return _upSprite;
        }
        else if (_direction == Direction.DOWN)
        {
            return _downSprite;
        }
        else
        {
            return _upSprite;
        }
    }
}
