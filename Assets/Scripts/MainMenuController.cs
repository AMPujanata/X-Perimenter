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
    void Start()
    {
        Screen.orientation = ScreenOrientation.LandscapeLeft;
        PlayerPrefs.SetInt("IsLevel0Cleared", 1);
    }

    public void CheckLevelValidity()
    {
        for(int i = 0; i < LevelSelectButtons.Length; i++)
        {
            if(PlayerPrefs.GetInt("IsLevel" + i.ToString() + "Cleared", 0) == 0)
            {
                LevelSelectButtons[i].enabled = false;
                LevelSelectButtons[i].transform.GetChild(0).gameObject.SetActive(false);
                LevelSelectButtons[i].transform.GetChild(1).gameObject.SetActive(true);
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
}
