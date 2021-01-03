using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System;

public enum ObjectiveType
{
    SUBONLY, MAINONLY
};

[Serializable]
public struct SubObjective
{
    public string _dontEditThis;
    public string SubObjectiveName;
    [TextArea] public string SubObjectiveDescription;
    public int CorrespondingFlagIndex;
}

[Serializable]
public struct Objective
{
    public string _dontEditThis;
    public string ObjectiveName;
    [TextArea] public string ObjectiveDescription;
    public ObjectiveType type;
    public int CorrespondingFlagIndex;
    public UnityEvent AfterObjectiveAchieved;
    public SubObjective[] SubObjectives;
}

public class ObjectiveManager : MonoBehaviour
{
    [SerializeField] private FlagManager _flagManager;
    [SerializeField] private Text _objectiveText;
    [SerializeField] private Objective[] _objectives;
    private Objective _currentObjective;
    private int _objectiveIndex = 0;

    void Awake()
    {
        SetCurrentObjective(0);
    }

    public void SetCurrentObjective(int index)
    {
        _objectiveIndex = index;
        _currentObjective = _objectives[_objectiveIndex];
        CheckCurrentObjective();
    }

    public void CheckCurrentObjective()
    {
        if(_currentObjective.type == ObjectiveType.MAINONLY)
        {
            if (_flagManager.GetFlagBool(_currentObjective.CorrespondingFlagIndex))
            {
                _currentObjective.AfterObjectiveAchieved.Invoke();
            }
            else
            {
                SetObjectiveText(_currentObjective);
            }
        }
        else if(_currentObjective.type == ObjectiveType.SUBONLY)
        {
            bool allSubObjectivesAchieved = true;
            foreach(SubObjective sub in _currentObjective.SubObjectives)
            {
                if (_flagManager.GetFlagBool(sub.CorrespondingFlagIndex))
                    allSubObjectivesAchieved = false;
            }
            if (allSubObjectivesAchieved)
            {
                _currentObjective.AfterObjectiveAchieved.Invoke();
            }
            else
            {
                SetObjectiveText(_currentObjective);
            }
        }
    }

    public void SetObjectiveText(Objective objective)
    {
        string temporaryObjectiveText = "";
        temporaryObjectiveText += objective.ObjectiveDescription + "\n";
        foreach(SubObjective sub in objective.SubObjectives)
        {
            if (_flagManager.GetFlagBool(sub.CorrespondingFlagIndex))
            {
                temporaryObjectiveText += "<color=red>- " + sub.SubObjectiveDescription + "</color>" + "\n";
            }
            else
            {
                temporaryObjectiveText += "- " + sub.SubObjectiveDescription + "\n";
            }
        }
        _objectiveText.text = temporaryObjectiveText;
    }

    public int GetObjectiveIndex()
    {
        return _objectiveIndex;
    }

}
