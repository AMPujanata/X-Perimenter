using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
public class MainMenuController : MonoBehaviour
{
    [SerializeField] private Button[] LevelSelectButtons;
    [SerializeField] private AudioSource _audioSource;
    [SerializeField] private Button _loadGameButton;
    [SerializeField] private GameObject _loadGameLockedObject;
    void Start()
    {
        Screen.orientation = ScreenOrientation.LandscapeLeft;
        PlayerPrefs.SetInt("IsLevel1Unlocked", 1);
        if (File.Exists(Application.persistentDataPath + "/saveData.dat"))
        {
            _loadGameButton.interactable = true;
            _loadGameLockedObject.SetActive(false);
        }
        else
        {
            _loadGameButton.interactable = false;
            _loadGameLockedObject.SetActive(true);
        }
    }

    public void CheckLevelValidity()
    {
        int levelToCheck = 0;
        for(int i = 0; i < LevelSelectButtons.Length; i++)
        {
            levelToCheck = i + 1;
            if(PlayerPrefs.GetInt("IsLevel" + levelToCheck.ToString() + "Unlocked", 0) == 0)
            {
                LevelSelectButtons[i].interactable = false;
                LevelSelectButtons[i].transform.GetChild(0).gameObject.SetActive(false);
                LevelSelectButtons[i].transform.GetChild(1).gameObject.SetActive(true);
            }
            else
            {
                LevelSelectButtons[i].interactable = true;
                LevelSelectButtons[i].transform.GetChild(0).gameObject.SetActive(true);
                LevelSelectButtons[i].transform.GetChild(1).gameObject.SetActive(false);
            }
        }
    }

    public void LoadLevel(string levelName) //used when passing only the scene's name into the method
    {
        //add any extra setup before load scene
        SceneManager.LoadScene(levelName);
    }

    public void LoadLevel(Scene level) //used when passing a whole Scene into the method
    {
        SceneManager.LoadScene(level.name);
    }

    public void QuitGame()
    {
        //add any extra cleanup here
        Application.Quit();
    }

    public void PrepareLoadGame()
    {
        if(File.Exists(Application.persistentDataPath + "/saveData.dat"))
        {
            SaveData tempSaveData = new SaveData();
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + "/saveData.dat", FileMode.Open);
            tempSaveData = (SaveData)bf.Deserialize(file);
            file.Close();

            PlayerPrefs.SetInt("ShouldLoadData", 1);
            SceneManager.LoadScene(tempSaveData.levelNumber);
        }
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

}
