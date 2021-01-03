using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileType : MonoBehaviour
{
    [SerializeField] private bool isWalkable;

    public bool IsTileWalkable()
    {
        return isWalkable;
    }
}
