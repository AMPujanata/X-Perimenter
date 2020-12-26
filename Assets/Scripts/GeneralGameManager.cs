using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GeneralGameManager : MonoBehaviour
{
    [SerializeField] private GameObject _pauseCanvas;
    [SerializeField] private GameObject _gameUICanvas;
    [SerializeField] private GameObject _dialogueCanvas;
    [SerializeField] private GameObject _gameOverCanvas;
    [SerializeField] private GameObject _levelClearCanvas;
    private bool _gameUICanvasWasActive;
    private bool _dialogueCanvasWasActive;
    private float _previousTimeScale;
	private float _previousDeltaScale;
    private PlayerBehavior _player;
    private CameraController _camera;
    // Start is called before the first frame update
    void Start()
    {
        _gameUICanvasWasActive = true;
        _player = FindObjectOfType<PlayerBehavior>();
        _camera = FindObjectOfType<CameraController>();
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
		_previousDeltaScale = Time.fixedDeltaTime;
        Time.timeScale = 0f;
        Time.fixedDeltaTime = 0f;
    }

    public void Unpause()
    {
        _gameUICanvas.SetActive(_gameUICanvasWasActive);
        _dialogueCanvas.SetActive(_dialogueCanvasWasActive);
        _pauseCanvas.SetActive(false);
        Time.timeScale = _previousTimeScale;
        Time.fixedDeltaTime = _previousDeltaScale;
    }

    public void BackToMainMenu()
    {
		Time.timeScale = 1f;
        SceneManager.LoadScene(0);
    }

    public void RestartLevel()
    {
		Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void GameOver()
    {
        _gameOverCanvas.SetActive(true);
        Time.timeScale = 0f;
    }

    public void LevelCleared()
    {
        PlayerPrefs.SetInt("IsLevel" + SceneManager.GetActiveScene().buildIndex + "Cleared", 1);
        _levelClearCanvas.SetActive(true);
        Time.timeScale = 0f;
    }

    public void TeleportPlayerHere(Transform teleportPointTransform)
    {
        TeleportPoint teleportPoint = teleportPointTransform.gameObject.GetComponent<TeleportPoint>();
        //also put fade here
        _player.transform.position = teleportPointTransform.position;
        _player.SetBounds(teleportPoint.GetBound());
        _camera.SetBounds(teleportPoint.GetBound());
		_player.ToggleMovement(true);
    }
}
