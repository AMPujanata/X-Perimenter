using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


public class LevelClearController : MonoBehaviour
{
    public UnityEvent CorrectAnswer;
    public UnityEvent WrongAnswer;
    public void Activate() //this will be used to load up questions later
    {

    }
    
    public void Success()
    {
        CorrectAnswer.Invoke();
    }

    public void Failure()
    {
        WrongAnswer.Invoke();
    }

}
