using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{
    [SerializeField] private GameObject _dialogueContainer;
    [SerializeField] private Image _speakerSprite;
    [SerializeField] private Text _speakerNameText;
    [SerializeField] private Text _dialogueText;
    [SerializeField] private ChoiceDialogueManager _choiceDialogueManager;
    private DialogueScriptable _currentDialogue;
    private DialogueGiver _currentDialogueGiver;
    private int currentIndex;
    public void SetupDialogue(DialogueScriptable dialogueFormat, DialogueGiver dialogueGiver)
    {
        _currentDialogue = dialogueFormat;
        _currentDialogueGiver = dialogueGiver;
        currentIndex = 0;
        _speakerSprite.sprite = _currentDialogue.Lines[currentIndex].SpeakerSprite;
        _speakerNameText.text = _currentDialogue.Lines[currentIndex].SpeakerName;
        _dialogueText.text = _currentDialogue.Lines[currentIndex].SpeakerLine;
        _dialogueContainer.SetActive(true);
    }

    public void ShowNextLine()
    {
        currentIndex++;
        if(currentIndex < _currentDialogue.Lines.Length)
        {
            _speakerSprite.sprite = _currentDialogue.Lines[currentIndex].SpeakerSprite;
            _speakerNameText.text = _currentDialogue.Lines[currentIndex].SpeakerName;
            _dialogueText.text = _currentDialogue.Lines[currentIndex].SpeakerLine;
        }
        else
        {
            if (_currentDialogue.ShouldTriggerAfterDialogueEvent)
                _currentDialogueGiver.ShouldTriggerAfterDialogueEvent = true;
            if (_currentDialogue._nextChoice)
            {
                _choiceDialogueManager.SetupChoiceDialogue(_currentDialogue._nextChoice, _currentDialogueGiver);
                _dialogueContainer.SetActive(false);
            }
            else if (_currentDialogue._nextDialogue)
            {
                SetupDialogue(_currentDialogue._nextDialogue, _currentDialogueGiver);
            }
            else
            {
                _dialogueContainer.SetActive(false);
                _currentDialogueGiver.ActivateAfterDialogueEvent();
            }
        }
    }
}
