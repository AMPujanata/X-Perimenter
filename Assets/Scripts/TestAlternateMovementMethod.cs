using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestAlternateMovementMethod : MonoBehaviour
{
    [SerializeField] private PersonalDPad _dPad;
    [SerializeField] private float _tileSize;
    [SerializeField] private float _moveSpeed;
    private bool _isPlayerMoving;

    void Start()
    {
        _isPlayerMoving = false;
    }

    void Update()
    {
        
        if (!_isPlayerMoving)
        {
            if(_dPad.GetHorizontalInput() != 0)
            {
                StartCoroutine(MoveToTile(_dPad.GetHorizontalInput(), 0));
            }
            else if(_dPad.GetVerticalInput() != 0)
            {
                StartCoroutine(MoveToTile(0, _dPad.GetVerticalInput()));
            }
        }
    }

    public IEnumerator MoveToTile(int horizontalTile, int verticalTile)
    {
        _isPlayerMoving = true;

        Vector2 startPos = new Vector2(transform.position.x, transform.position.y);
        Vector2 endPos = startPos + new Vector2(horizontalTile * _tileSize, verticalTile * _tileSize);
        Vector2 dir = (endPos - startPos).normalized;
        float time = 0;

        Debug.DrawLine(startPos, endPos);
        //rudimentary check to see if endPos is on a passable tile or not
        RaycastHit2D hit = Physics2D.Raycast(transform.position, dir);

        if (hit.collider != null)
        {
            if(hit.distance > _tileSize)
            {
                while (Vector2.Distance(transform.position, endPos) > 0.001f)
                {
                    time += Time.deltaTime;
                    transform.position = Vector2.MoveTowards(transform.position, endPos, _moveSpeed * Time.deltaTime);
                    yield return null;
                }
            }
        }
        else
        {
            while (Vector2.Distance(transform.position, endPos) > 0.001f)
            {
                time += Time.deltaTime;
                transform.position = Vector2.MoveTowards(transform.position, endPos, _moveSpeed * Time.deltaTime);
                yield return null;
            }
        }
        _isPlayerMoving = false;
        yield break;
    }
}
