using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class FlagManager : MonoBehaviour
{
    private Dictionary<string, bool> _levelClearFlags;
    [SerializeField] private GameObject _finishInvestigationButton;
    private void Awake()
    {
        _levelClearFlags = new Dictionary<string, bool>();
    }


    public void AddLevelClearFlag(string key, bool value)
    {
        _levelClearFlags.Add(key, value);
    }

    public void ActivateLevelClearFlag(string key)
    {
        _levelClearFlags[key] = true;
    }

    public bool CheckLevelClearFlags()
    {
        foreach(var KeyValuePair in _levelClearFlags)
        {
            if (!KeyValuePair.Value)
            {
                Debug.Log("One flag wasn't set. Returning...");
                return false;
            }
        }
        Debug.Log("All flags cleared!");
        _finishInvestigationButton.SetActive(true); //might change this to a unity event in the future
        return true;
    }

}
