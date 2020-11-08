using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExamineObject : InteractionObject
{

    [TextArea][SerializeField] private string _objectDescription;
    /*[SerializeField] private string[] _objectDescriptionState;
    [SerializeField] private int _currentState;*/ //these two are for later when i have time to make objects with multiple switchable descriptions
    private PlayerBehavior _playerBehavior;
    private UIController _UIController;
    // Start is called before the first frame update
    void Start()
    {
        _playerBehavior = FindObjectOfType<PlayerBehavior>();
        _UIController = FindObjectOfType<UIController>();
        if (_flagBearer)
        {
            FlagManager flagManager = FindObjectOfType<FlagManager>();
            flagManager.AddLevelClearFlag(name, false);
        }
    }

    public override void Interact()
    {
        _playerBehavior.ToggleMovement(false);
        _UIController.OpenExamineDialog(name, _objectDescription); //this will eventually get changed to be able to call a specific description
        if (_flagBearer)
        {
            FlagManager flagManager = FindObjectOfType<FlagManager>();
            flagManager.ActivateLevelClearFlag(name);
        }
    }
}
