using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Glosarium", menuName = "ScriptableObjects/Glosarium", order = 2)]
public class GlosariumScriptable : ScriptableObject
{
    [TextArea] public string ChemicalName;
    public Sprite ChemicalPicture;
    [TextArea] public string IUPACName;
    [TextArea] public string OtherNames;
    [TextArea] public string Characteristics;
    [TextArea] public string[] ActualUses;
    [TextArea] public string WrongUse;
    public string ShortenedChemicalName;
    [TextArea] public string[] Effects;
    public int AssociatedLevelNumber;
    [TextArea] public string FoodSigns;
}
