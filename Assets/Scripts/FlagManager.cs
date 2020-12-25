using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System;

[Serializable]
public struct Flag
{
    public string _dontChangeThis;
    public string FlagName;
    public string HowToActivateFlag;
    public bool IsFlagAchieved;
    public UnityEvent AfterFlagActivated;
}

public class FlagManager : MonoBehaviour
{
    [SerializeField] private ObjectiveManager _objectiveManager;
    [SerializeField] private Flag[] _flags;
    public void ActivateFlag(int index)
    {
        if (!_flags[index].IsFlagAchieved)
        {
            _flags[index].IsFlagAchieved = true;
            _flags[index].AfterFlagActivated.Invoke();
        }
        _objectiveManager.CheckCurrentObjective();
    }

    public bool GetFlagBool(int index)
    {
        return _flags[index].IsFlagAchieved;
    }
}
