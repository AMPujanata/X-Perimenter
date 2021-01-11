using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Events;
using UnityEngine.UI;
public class GeneralGameManager : MonoBehaviour
{
    [SerializeField] private GameObject _pauseCanvas;
    [SerializeField] private GameObject _gameUICanvas;
    [SerializeField] private GameObject _dialogueCanvas;
    [SerializeField] private GameObject _gameOverCanvas;
    [SerializeField] private GameObject _levelClearCanvas;
    [SerializeField] private Image _fadeImage;
    [SerializeField] private float _fadeTime;
    [SerializeField] private UnityEvent _startOfLevelEvent;
    [SerializeField] private AudioSource _audioSource;
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
        //code for loading game here from playerprefs
        if(PlayerPrefs.GetInt("ShouldLoadData", 0) == 1)
        {
            PlayerPrefs.SetInt("ShouldLoadData", 0);
            SaveManager saveManager = FindObjectOfType<SaveManager>();
            saveManager.LoadGame();
        }
        else
            _startOfLevelEvent.Invoke();
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

    public void FadeIn()
    {
        StartCoroutine(FadeInProcess());
    }

    public void FadeOut()
    {
        StartCoroutine(FadeOutProcess());
    }

    public void TeleportPlayerHere(Transform teleportPointTransform)
    {
        StartCoroutine(TeleportPlayerHereProcess(teleportPointTransform));
    }

    public IEnumerator FadeInProcess()
    {
        float timeElapsed = 0;
        Color c = _fadeImage.color;
        float alpha = 0;
        while(timeElapsed < _fadeTime)
        {
            alpha = Mathf.Lerp(0, 1, timeElapsed / _fadeTime);
            c.a = alpha;
            _fadeImage.color = c;
            timeElapsed += Time.deltaTime;
            yield return null;
        }
        c.a = 1;
        _fadeImage.color = c;
        yield break;
    }

    public IEnumerator FadeOutProcess()
    {
        float timeElapsed = 0;
        Color c = _fadeImage.color;
        float alpha = 1;
        while (timeElapsed < _fadeTime)
        {
            alpha = Mathf.Lerp(1, 0, timeElapsed / _fadeTime);
            c.a = alpha;
            _fadeImage.color = c;
            timeElapsed += Time.deltaTime;
            yield return null;
        }
        c.a = 0;
        _fadeImage.color = c;
        yield break;
    }

    public IEnumerator TeleportPlayerHereProcess(Transform teleportPointTransform)
    {
        _player.ToggleMovement(false);

        float timeElapsed = 0;
        Color c = _fadeImage.color;
        float alpha = 0;
        while (timeElapsed < _fadeTime)
        {
            alpha = Mathf.Lerp(0, 1, timeElapsed / _fadeTime);
            c.a = alpha;
            _fadeImage.color = c;
            timeElapsed += Time.deltaTime;
            yield return null;
        }
        c.a = 1;
        _fadeImage.color = c;

        TeleportPoint teleportPoint = teleportPointTransform.gameObject.GetComponent<TeleportPoint>();
        _player.transform.position = teleportPointTransform.position;
        _player.SetBounds(teleportPoint.GetBound());
        _camera.SetBounds(teleportPoint.GetBound());

        timeElapsed = 0;
        while (timeElapsed < _fadeTime)
        {
            alpha = Mathf.Lerp(1, 0, timeElapsed / _fadeTime);
            c.a = alpha;
            _fadeImage.color = c;
            timeElapsed += Time.deltaTime;
            yield return null;
        }
        c.a = 0;
        _fadeImage.color = c;

        _player.ToggleMovement(true);
        yield break;
    }

    public void PlayAudioClip(AudioClip clip)
    {
        _audioSource.PlayOneShot(clip);
    }

    public void SwitchBGM(AudioClip BGM)
    {
        _audioSource.Stop();
        _audioSource.clip = BGM;
        _audioSource.Play();
    }

    public void StopBGM()
    {
        _audioSource.Stop();
    }

    public void StartBGM()
    {
        _audioSource.Play();
    }
}
