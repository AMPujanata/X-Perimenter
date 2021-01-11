using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{
    [SerializeField] private GameObject _parentDialogueContainer;
    [SerializeField] private GameObject _dialogueContainer;
    [SerializeField] private Image _speakerSprite;
    [SerializeField] private Text _speakerNameText;
    [SerializeField] private Text _dialogueText;
    [SerializeField] private GameObject _choiceDialogueContainer;
    [SerializeField] private Button[] _choiceTextButtons;
    [SerializeField] private AudioClip _defaultDialogueAdvanceSound;
    private List<int> _flagsToTriggerAfter;
    private DialogueScriptable _currentDialogue;
    private int currentIndex;
    private GeneralGameManager _generalGameManager;
    private PlayerBehavior _playerBehavior;
    private FixedJoystick _fixedJoystick;
	void Awake()
	{
        _generalGameManager = FindObjectOfType<GeneralGameManager>();
        _playerBehavior = FindObjectOfType<PlayerBehavior>();
        _fixedJoystick = FindObjectOfType<FixedJoystick>();
		_flagsToTriggerAfter = new List<int>();
	}
	
    public void SetupDialogue(DialogueScriptable dialogueFormat)
    {
        _generalGameManager.ToggleGameUI(false);
        _playerBehavior.ToggleMovement(false);
        _currentDialogue = dialogueFormat;
        currentIndex = 0;
        if (_currentDialogue.Lines[currentIndex].lineSFX != null)
        {
            _generalGameManager.PlayAudioClip(_currentDialogue.Lines[currentIndex].lineSFX);
        }
        else
        {
            _generalGameManager.PlayAudioClip(_defaultDialogueAdvanceSound);
        }
        //_speakerSprite.sprite = _currentDialogue.Lines[currentIndex].SpeakerSprite;
        _speakerNameText.text = _currentDialogue.Lines[currentIndex].SpeakerName;
        _dialogueText.text = _currentDialogue.Lines[currentIndex].SpeakerLine;
        _parentDialogueContainer.SetActive(true);
        _dialogueContainer.SetActive(true);
    }

    public void ShowNextLine()
    {
        if (currentIndex  < _currentDialogue.Lines.Length - 1)
        {
            currentIndex++;
            if (_currentDialogue.Lines[currentIndex].lineSFX != null)
            {
                _generalGameManager.PlayAudioClip(_currentDialogue.Lines[currentIndex].lineSFX);
            }
            else
            {
                _generalGameManager.PlayAudioClip(_defaultDialogueAdvanceSound);
            }
            //_speakerSprite.sprite = _currentDialogue.Lines[currentIndex].SpeakerSprite;
            _speakerNameText.text = _currentDialogue.Lines[currentIndex].SpeakerName;
            _dialogueText.text = _currentDialogue.Lines[currentIndex].SpeakerLine;
        }
        else
        {
            if (_choiceDialogueContainer.activeSelf)
                return;
            if(_currentDialogue._flagType == FlagType.IMMEDIATE)
            {
                FlagManager flagManager = GetComponent<FlagManager>();
                flagManager.ActivateFlag(_currentDialogue._flagIndex);
            }
            else if(_currentDialogue._flagType == FlagType.AFTER)
            {
                _flagsToTriggerAfter.Add(_currentDialogue._flagIndex);
            }
            if (_currentDialogue._nextChoice)
            {
                SetupChoiceDialogue(_currentDialogue._nextChoice);
            }
            else if (_currentDialogue._nextDialogue)
            {
                SetupDialogue(_currentDialogue._nextDialogue);
            }
            else
            {
                FlagManager flagManager = GetComponent<FlagManager>();
				if(_flagsToTriggerAfter.Count != 0)
				{
					foreach(int flagToAdd in _flagsToTriggerAfter)
					{
						flagManager.ActivateFlag(_currentDialogue._flagIndex);
					}
					_flagsToTriggerAfter.Clear();
				}
                _dialogueContainer.SetActive(false);
                _parentDialogueContainer.SetActive(false);
                _generalGameManager.ToggleGameUI(true);
                _playerBehavior.ToggleMovement(true);
            }
        }
    }

    //
    public void SetupChoiceDialogue(ChoiceScriptable choiceFormat)
    {
        int index = 0;
        _generalGameManager.PlayAudioClip(_defaultDialogueAdvanceSound);
        foreach (Choice choice in choiceFormat.Choices)
        {
            _choiceTextButtons[index].gameObject.SetActive(true);
            _choiceTextButtons[index].transform.GetChild(0).GetComponent<Text>().text = choice.ChoiceText;
            _choiceTextButtons[index].onClick.AddListener(delegate { SetupNextDialog(choice.NextDialogue, choice.ShouldTriggerAfterDialogueEvent); });
            index++;
            if (index >= _choiceTextButtons.Length)
            {
                break;
            }
        }
        for (int i = index; i < _choiceTextButtons.Length; i++)
        {
            _choiceTextButtons[i].gameObject.SetActive(false);
        }
        _choiceDialogueContainer.SetActive(true);
    }

    void SetupNextDialog(DialogueScriptable dialogue, bool shouldTriggerAfterDialogueEvent)
    {
        foreach (Button button in _choiceTextButtons)
        {
            button.onClick.RemoveAllListeners();
            button.gameObject.SetActive(false);
        }
        SetupDialogue(dialogue);
        _choiceDialogueContainer.SetActive(false);
    }
}
