using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleportPoint : MonoBehaviour
{
    [SerializeField] private BoxCollider2D _associatedBound;
    [SerializeField] private Direction _afterTeleportDirection;

    public BoxCollider2D GetBound()
    {
        return _associatedBound;
    }

    public Direction GetDirection()
    {
        return _afterTeleportDirection;
    }

    public void SetBound(BoxCollider2D newBound)
    {
        _associatedBound = newBound;
    }
}
