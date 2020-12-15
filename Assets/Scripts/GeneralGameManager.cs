using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GeneralGameManager : MonoBehaviour
{
    [SerializeField] private GameObject _pauseCanvas;
    [SerializeField] private GameObject _gameUICanvas;
    [SerializeField] private GameObject _dialogueCanvas;
    private bool _gameUICanvasWasActive;
    private bool _dialogueCanvasWasActive;
    private float _previousTimeScale;
    // Start is called before the first frame update
    void Start()
    {
        _gameUICanvasWasActive = true;
    }

    public void ToggleGameUI(bool validity)
    {
        _gameUICanvas.SetActive(validity);
    }

    public void Pause()
    {
        _gameUICanvasWasActive = _gameUICanvas.activeSelf;
        _dialogueCanvasWasActive = _dialogueCanvas.activeSelf;
        _gameUICanvas.SetActive(false);
        _dialogueCanvas.SetActive(false);
        _pauseCanvas.SetActive(true);
        _previousTimeScale = Time.timeScale;
        Time.timeScale = 0f;
        Time.fixedDeltaTime = 0f;
    }

    public void Unpause()
    {
        _gameUICanvas.SetActive(_gameUICanvasWasActive);
        _dialogueCanvas.SetActive(_dialogueCanvasWasActive);
        _pauseCanvas.SetActive(false);
        Time.timeScale = _previousTimeScale;
        Time.fixedDeltaTime = _previousTimeScale;
    }

    public void BackToMainMenu()
    {
        SceneManager.LoadScene(0);
    }

    public void RestartLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
