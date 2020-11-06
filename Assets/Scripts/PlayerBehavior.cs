 using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBehavior : MonoBehaviour
{
    private bool _canMove;
    [SerializeField] private float _moveSpeed;
    private bool _isMoving;
    private Coroutine _currentMoveCoroutine;
    void Awake()
    {
        _canMove = true;
        _isMoving = false;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
#if UNITY_EDITOR
        if(_canMove && Input.GetMouseButtonDown(0))
        {
            if (_isMoving)
                StopCoroutine(_currentMoveCoroutine);

            Vector3 convertedMousePosition = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, Camera.main.nearClipPlane));
            _currentMoveCoroutine = StartCoroutine(MoveToLocation(convertedMousePosition.x, convertedMousePosition.y));
        }
#endif
#if !UNITY_EDITOR && UNITY_ANDROID
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
#endif
    }

    IEnumerator MoveToLocation(float xCoordinate, float yCoordinate)
    {
        _isMoving = true;
        Vector2 endPosition = new Vector2(xCoordinate, yCoordinate);
        Debug.Log(endPosition);
        while(Vector2.Distance(transform.position, endPosition) > 0.01f)
        {
            Vector2 currentPosition = new Vector2(transform.position.x, transform.position.y);
            transform.position = Vector2.MoveTowards(currentPosition, endPosition, _moveSpeed * Time.deltaTime);
            yield return null;
        }
        _isMoving = false;
        yield break;
    }
}
