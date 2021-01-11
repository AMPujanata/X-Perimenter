using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class GlosariumManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _nameText;
    [SerializeField] private Image _chemicalImage;
    [SerializeField] private TextMeshProUGUI _generalDescriptionText;
    [SerializeField] private TextMeshProUGUI[] _actualUsesText;
    [SerializeField] private TextMeshProUGUI _wrongUsesText;
    [SerializeField] private TextMeshProUGUI _effectsCategoryText;
    [SerializeField] private TextMeshProUGUI[] _effectsText;
	[SerializeField] private Button _glosariumButton;
	[SerializeField] private GameObject _lockedGlosarium;
    [SerializeField] private GameObject _glosariumPrevButton;
    [SerializeField] private GameObject _glosariumNextButton;
    [SerializeField] private GameObject _glosariumNextLockedObject;
    [SerializeField] private GlosariumScriptable[] _glosariumPages;
    private GlosariumScriptable[] _activeGlosariumPages;
    private GlosariumScriptable _currentGlosarium;
    private int _glosariumIndex;
    // Start is called before the first frame update
    void Start()
    {
        int _activeGlosariumIndex = 0;
        for(int i = 0; i < _glosariumPages.Length; i++)
        {
            if(PlayerPrefs.GetInt("IsLevel" + _glosariumPages[i].AssociatedLevelNumber + "Cleared") == 1)
            {
                _activeGlosariumIndex++;
            }
        }
        _activeGlosariumPages = new GlosariumScriptable[_activeGlosariumIndex];
        _activeGlosariumIndex = 0;
        for (int i = 0; i < _glosariumPages.Length; i++)
        {
            if (PlayerPrefs.GetInt("IsLevel" + _glosariumPages[i].AssociatedLevelNumber + "Cleared") == 1)
            {
                _activeGlosariumPages[_activeGlosariumIndex] = _glosariumPages[i];
                _activeGlosariumIndex++;
            }
        }
        if (_activeGlosariumPages.Length > 0)
		{
			_lockedGlosarium.SetActive(false);
			_glosariumButton.interactable = true;
		}
		else
		{
			_lockedGlosarium.SetActive(true);
			_glosariumButton.interactable = false;
		}
        _glosariumIndex = 0;
        _currentGlosarium = _activeGlosariumPages[_glosariumIndex];
    }


    public void SetupGlosarium()
    {
        _glosariumIndex = 0;
        DisplayGlosarium();
    }

    public void DisplayGlosarium()
    {
        _currentGlosarium = _activeGlosariumPages[_glosariumIndex];

        _nameText.text = _currentGlosarium.ChemicalName;
        _chemicalImage.sprite = _currentGlosarium.ChemicalPicture;

        _generalDescriptionText.text = "<color=blue>Nama IUPAC:</color>\n";
        _generalDescriptionText.text += _currentGlosarium.IUPACName + "\n\n";
        _generalDescriptionText.text += "<color=blue>Nama Lain:</color>\n";
        _generalDescriptionText.text += _currentGlosarium.OtherNames + "\n\n";
        _generalDescriptionText.text += "<color=blue>Ciri-Ciri:</color>\n";
        _generalDescriptionText.text += _currentGlosarium.Characteristics;

        for(int i = 0; i < _currentGlosarium.ActualUses.Length; i++)
        {
            if (i >= _actualUsesText.Length)
                break;
            _actualUsesText[i].text = "- " + _currentGlosarium.ActualUses[i];
        }

        _wrongUsesText.text = _currentGlosarium.WrongUse;
        _effectsCategoryText.text = "<color=blue>Dampak " + _currentGlosarium.ShortenedChemicalName + " Pada Kesehatan</color>";

        for(int i = 0; i < _currentGlosarium.Effects.Length; i++)
        {
            if (i >= _effectsText.Length)
                break;
            _effectsText[i].text = "- " + _currentGlosarium.Effects[i];
        }

        int levelToUnlock = _currentGlosarium.AssociatedLevelNumber + 1;
        PlayerPrefs.SetInt("IsLevel" + levelToUnlock + "Unlocked", 1);

        if (_glosariumIndex == 0)
            _glosariumPrevButton.SetActive(false);
        else
            _glosariumPrevButton.SetActive(true);
        if (_glosariumIndex == (_glosariumPages.Length - 1))
            _glosariumNextButton.SetActive(false);
        else
            _glosariumNextButton.SetActive(true);
        if (_glosariumIndex == (_activeGlosariumPages.Length - 1))
        {
            _glosariumNextLockedObject.SetActive(true);
            _glosariumNextButton.GetComponent<Button>().interactable = false; 
        }
        else
        {
            _glosariumNextLockedObject.SetActive(false);
            _glosariumNextButton.GetComponent<Button>().interactable = true;
        }
    }

    public void NextGlosariumPage()
    {
        _glosariumIndex += 1;
        DisplayGlosarium();
    }

    public void PrevGlosariumPage()
    {
        _glosariumIndex -= 1;
        DisplayGlosarium();
    }
}
