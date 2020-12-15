using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class ObjectiveBar : MonoBehaviour
{
    [SerializeField] private Text _objectiveText;

    public void SetObjective(string newObjective)
    {
        _objectiveText.text = "Objective: " + newObjective;
    }
}
