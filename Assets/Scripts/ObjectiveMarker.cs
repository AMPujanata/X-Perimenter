using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectiveMarker : MonoBehaviour
{
    public void MoveObjectiveMarker(Transform destination)
    {
        transform.position = destination.position;
        gameObject.SetActive(true);
    }

    public void DisableObjectiveMarker()
    {
        gameObject.SetActive(false);
    }
}
