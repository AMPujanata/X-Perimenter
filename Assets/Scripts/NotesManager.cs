using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class NotesManager : MonoBehaviour
{
    [SerializeField] private GameObject[] _notePrefabs;
    [SerializeField] private GameObject _prevButton;
    [SerializeField] private GameObject _nextButton;
    [SerializeField] private Image _newNoteAvailableImage;
    [SerializeField] private List<string> _notes;
    private int _notesPerPage;
    private int _pageCount;

    void Awake()
    {
        _notesPerPage = _notePrefabs.Length;
    }

    public void SetupNotes()
    {
        _pageCount = 0;
        DisplayNotes();
    }

    private void DisplayNotes()
    {
        int startingIndex = _pageCount * _notesPerPage;
        if(_pageCount == 0)
        {
            _prevButton.SetActive(false);
        }
        else
        {
            _prevButton.SetActive(true);
        }
        if(startingIndex + _notesPerPage >= _notes.Count)
        {
            _nextButton.SetActive(false);
        }
        else
        {
            _nextButton.SetActive(true);
        }
        foreach (GameObject prefab in _notePrefabs)
        {
            prefab.SetActive(false);
        }
        if (_notes.Count == 0)
        {
            _notePrefabs[0].transform.GetChild(0).gameObject.GetComponent<Text>().text = "No clues yet...";
            _notePrefabs[0].SetActive(true);
        }
        else
        {
            for (int i = 0; i < _notesPerPage; i++)
            {
                if (startingIndex + i >= _notes.Count)
                {
                    return;
                }
                _notePrefabs[i].transform.GetChild(0).gameObject.GetComponent<Text>().text = _notes[startingIndex + i];
                _notePrefabs[i].SetActive(true);
            }
        }
    }

    public void PreviousPage()
    {
        _pageCount -= 1;
        DisplayNotes();
    }

    public void NextPage()
    {
        _pageCount += 1;
        DisplayNotes();
    }

    public void AddNote(string noteContent)
    {
        _notes.Add(noteContent);
        _newNoteAvailableImage.gameObject.SetActive(true);
    }

    public void SetNotes(List<string> tempNotes)
    {
        _notes = tempNotes;
    }

    public List<string> GetNotes()
    {
        return _notes;
    }
}
