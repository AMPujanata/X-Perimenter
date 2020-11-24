using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public struct Flag
{
    public string FlagName;
    public string FlagObjective;
}

[CreateAssetMenu(fileName = "New Flag Collection", menuName = "ScriptableObjects/Booth", order = 1)]
public class FlagCollection : ScriptableObject
{
    public string FlagCollectionName;
    public Flag[] Flags;

}
