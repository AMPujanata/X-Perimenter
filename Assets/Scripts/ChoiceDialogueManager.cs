using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChoiceDialogueManager : MonoBehaviour
{
    [SerializeField] private GameObject _choiceDialogueContainer;
    [SerializeField] private Text _questionText;
    [SerializeField] private Button[] _choiceTextButtons;
    [SerializeField] private DialogueManager _dialogueManager;
    private DialogueGiver _currentDialogueGiver;
    public void SetupChoiceDialogue(ChoiceScriptable choiceFormat, DialogueGiver dialogueGiver)
    {
        _currentDialogueGiver = dialogueGiver;
        _questionText.text = choiceFormat.ChoiceQuestion;
        int index = 0;
        foreach(Choice choice in choiceFormat.Choices)
        {
            _choiceTextButtons[index].gameObject.SetActive(true);
            _choiceTextButtons[index].transform.GetChild(0).GetComponent<Text>().text = choice.ChoiceText;
            _choiceTextButtons[index].onClick.AddListener(delegate { SetupNextDialog(choice.NextDialogue, choice.ShouldTriggerAfterDialogueEvent); });
            index++;
            if(index >= _choiceTextButtons.Length)
            {
                break;
            }
        }
        for(int i = index;  i < _choiceTextButtons.Length; i++)
        {
            _choiceTextButtons[i].gameObject.SetActive(false);
        }
        _choiceDialogueContainer.SetActive(true);
    }

    void SetupNextDialog(DialogueScriptable dialogue, bool shouldTriggerAfterDialogueEvent)
    {
        if (shouldTriggerAfterDialogueEvent)
            _currentDialogueGiver.ShouldTriggerAfterDialogueEvent = true;
        foreach(Button button in _choiceTextButtons)
        {
            button.onClick.RemoveAllListeners();
            button.gameObject.SetActive(false);
        }
        _dialogueManager.SetupDialogue(dialogue, _currentDialogueGiver);
        _choiceDialogueContainer.SetActive(false);
    }
}
