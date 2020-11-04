using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{
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
}
