using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System;

[Serializable]
public struct DialogueGiverState
{
    public string _stateName;
    public string _howToReachState;
    public DialogueScriptable _dialogue;
    public UnityEvent _afterDialogueEvent;
    public bool _hasEventTriggeredAlready;
}

public class DialogueGiver : InteractionObject
{
    [SerializeField] private DialogueGiverState[] _dialogueGiverStates;
    private int _currentDialogueGiverStateIndex;
    private DialogueManager _dialogueManager;
    public bool ShouldTriggerAfterDialogueEvent;

    private void Start()
    {
        _dialogueManager = GameObject.Find("GameManager").GetComponent<DialogueManager>();
        _currentDialogueGiverStateIndex = 0;
    }

    public override void Interact()
    {
        _dialogueManager.SetupDialogue(_dialogueGiverStates[_currentDialogueGiverStateIndex]._dialogue, this);
    }

    public void ActivateAfterDialogueEvent()
    {
        if (!_dialogueGiverStates[_currentDialogueGiverStateIndex]._hasEventTriggeredAlready && ShouldTriggerAfterDialogueEvent)
        {
            _dialogueGiverStates[_currentDialogueGiverStateIndex]._afterDialogueEvent.Invoke();
            _dialogueGiverStates[_currentDialogueGiverStateIndex]._hasEventTriggeredAlready = true;
            ShouldTriggerAfterDialogueEvent = false;
        }
        else
        {
            ShouldTriggerAfterDialogueEvent = false;
        }
    }

    public void SwitchState(int stateIndex)
    {
        _currentDialogueGiverStateIndex = stateIndex;
    }
}
