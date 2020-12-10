﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


public enum Orientation
{
    LEFT, RIGHT
}

[Serializable]
public struct Line
{
    public string _dontEditThis;
    public string SpeakerName;
    public Sprite SpeakerSprite;
    public Orientation SpeakerOrientation;
    [TextArea] public string SpeakerLine;
}

[CreateAssetMenu(fileName = "New Dialogue", menuName = "ScriptableObjects/Dialogue", order = 1)]
public class DialogueScriptable : ScriptableObject
{
    public string DialogueName;
    public Line[] Lines;
    public ChoiceScriptable _nextChoice;
    public DialogueScriptable _nextDialogue;
    public bool ShouldTriggerAfterDialogueEvent;
}
