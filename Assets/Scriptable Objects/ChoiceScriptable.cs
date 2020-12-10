using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public struct Choice
{
    public string ChoiceText;
    public DialogueScriptable NextDialogue;
    public bool ShouldTriggerAfterDialogueEvent;
}

[CreateAssetMenu(fileName = "New Choice", menuName = "ScriptableObjects/Choice", order = 1)]
public class ChoiceScriptable : ScriptableObject
{
    [TextArea] public string ChoiceQuestion;
    public Choice[] Choices;
}
