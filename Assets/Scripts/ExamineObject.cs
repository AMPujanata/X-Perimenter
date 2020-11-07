using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExamineObject : InteractionObject
{
    [SerializeField] private string[] _stateName; //not actually used; just for labelling the state an object is in
    [SerializeField] private string[] _objectDescription; //the one actually displayed depends on the current state
    [SerializeField] private int _currentState;
    private PlayerBehavior _playerBehavior;
    private UIController _UIController;
    // Start is called before the first frame update
    void Start()
    {
        _playerBehavior = FindObjectOfType<PlayerBehavior>();
        _UIController = FindObjectOfType<UIController>();
    }

    public override void Interact()
    {
        _playerBehavior.ToggleMovement(false);
        _UIController.OpenExamineDialog(name, _objectDescription[0]);
    }
}
