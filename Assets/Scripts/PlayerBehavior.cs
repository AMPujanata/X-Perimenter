 using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBehavior : MonoBehaviour
{
    private bool _canMove;
    [SerializeField] private float _moveSpeed;
    private bool _isMoving;
    private Coroutine _currentMoveCoroutine;
    private BoxCollider2D _2DCollider;
    [SerializeField] private FixedJoystick _joystick;
    void Awake()
    {
        _canMove = true;
        _isMoving = false;
        _2DCollider = GetComponent<BoxCollider2D>();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
/*#if UNITY_EDITOR // mouse/editor controls
        if(_canMove && Input.GetMouseButtonDown(0))
        {
            if (_isMoving)
                StopCoroutine(_currentMoveCoroutine);

            Vector3 convertedMousePosition = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, Camera.main.nearClipPlane));
            _currentMoveCoroutine = StartCoroutine(MoveToLocation(convertedMousePosition.x, convertedMousePosition.y));
        }
#endif
#if !UNITY_EDITOR && UNITY_ANDROID //touch controls
        Debug.Log("Unity Android");
        if (_canMove && Input.touchCount > 0)
        {
            if(Input.GetTouch(0).phase == TouchPhase.Began || Input.GetTouch(0).phase == TouchPhase.Moved)
            {
                if(_isMoving)
                    StopCoroutine(_currentMoveCoroutine);

                Vector2 touchPosition = Input.GetTouch(0).position;
                Vector3 convertedTouchPosition = Camera.main.ScreenToWorldPoint(new Vector3(touchPosition.x, touchPosition.y, Camera.main.nearClipPlane));
               _currentMoveCoroutine = StartCoroutine(MoveToLocation(convertedTouchPosition.x, convertedTouchPosition.y));
            }
        }
#endif*/
        if(_canMove)
        {
            transform.position += Vector3.right * _joystick.Horizontal * _moveSpeed * Time.deltaTime;
            transform.position += Vector3.up * _joystick.Vertical * _moveSpeed * Time.deltaTime;
        }
    }

    /*IEnumerator MoveToLocation(float xCoordinate, float yCoordinate)
    {
        _isMoving = true;
        Vector2 endPosition = new Vector2(xCoordinate, yCoordinate);
        while(Vector2.Distance(transform.position, endPosition) > 0.01f) //slowly move to endPosition while not close enough
        {
            Vector2 currentPosition = new Vector2(transform.position.x, transform.position.y);
            transform.position = Vector2.MoveTowards(currentPosition, endPosition, _moveSpeed * Time.deltaTime);
            yield return null;
        }

        CheckForInteraction();
        _isMoving = false;
        yield break;
    }*/

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

    public void FinishExamine()
    {
        FlagManager flagManager = FindObjectOfType<FlagManager>();
        if(flagManager.CheckLevelClearFlags()) //DO NOT let this stay here move it to a better trigger once prototype is over
            return;
        ToggleMovement(true);
    }
    public void ToggleMovement(bool shouldMove)
    {
        _canMove = shouldMove;
    }
}
