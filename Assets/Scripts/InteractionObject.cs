using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System;
[Serializable]
public struct InteractionState
{
    public string _stateName;
    public string _howToReachState;
    public UnityEvent _interactionEvent;
}
public class InteractionObject : MonoBehaviour
{
    [SerializeField] private InteractionState[] _states;
    private int _stateIndex = 0;
    void Start()
    {

    }

    public void Interact()
    {
        _states[_stateIndex]._interactionEvent.Invoke();
    }
}
