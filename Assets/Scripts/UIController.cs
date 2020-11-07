using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private GameObject ExamineDialog;
    [SerializeField] private Text ExamineName;
    [SerializeField] private Text ExamineDescription;
    void Start()
    {
        
    }

    public void OpenExamineDialog(string newExamineName, string newExamineDescription)
    {
        ExamineName.text = newExamineName;
        ExamineDescription.text = newExamineDescription;
        ExamineDialog.SetActive(true);
    }
}
