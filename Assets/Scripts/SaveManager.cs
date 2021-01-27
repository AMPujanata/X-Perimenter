using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

[System.Serializable]
public class SaveData
{
    public int levelNumber;
    public float playerPositionX;
    public float playerPositionY;
    public float playerPositionZ;
    public bool[] flags;
    public int currentObjectiveIndex;
    public float boundUp;
    public float boundDown;
    public float boundLeft;
    public float boundRight;
}

public class SaveManager : MonoBehaviour
{
    private SaveData _currentSaveData;
    private string _savePath;
    [SerializeField] private PlayerBehavior _playerBehavior;
    [SerializeField] private CameraController _cameraController;
    [SerializeField] private FlagManager _flagManager;
    [SerializeField] private ObjectiveManager _objectiveManager;

    void Awake()
    {
        _currentSaveData = new SaveData();
        _savePath = Application.persistentDataPath + "/saveData.dat";
    }

    public void SaveGame()
    {
        _currentSaveData.levelNumber = SceneManager.GetActiveScene().buildIndex;
        _currentSaveData.playerPositionX = _playerBehavior.transform.position.x;
        _currentSaveData.playerPositionY = _playerBehavior.transform.position.y;
        _currentSaveData.playerPositionZ = _playerBehavior.transform.position.z;
        Flag[] _flags = _flagManager.GetFlags();
        _currentSaveData.flags = new bool[_flags.Length];
        for(int i = 0; i < _flags.Length; i++)
        {
            _currentSaveData.flags[i] = _flags[i].IsFlagAchieved;
        }
        _currentSaveData.currentObjectiveIndex = _objectiveManager.GetObjectiveIndex();
        _currentSaveData.boundUp = _playerBehavior.GetBound("up");
        _currentSaveData.boundDown = _playerBehavior.GetBound("down");
        _currentSaveData.boundLeft = _playerBehavior.GetBound("left");
        _currentSaveData.boundRight = _playerBehavior.GetBound("right");
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(_savePath);
        bf.Serialize(file, _currentSaveData);
        file.Close();
    }

    public void LoadGame()
    {
        if (File.Exists(_savePath))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(_savePath, FileMode.Open);
            _currentSaveData = (SaveData)bf.Deserialize(file);
            file.Close();
            Vector3 tempPlayerPosition = new Vector3(_currentSaveData.playerPositionX, _currentSaveData.playerPositionY, _currentSaveData.playerPositionZ);
            _playerBehavior.transform.position = tempPlayerPosition;
            _objectiveManager.SetCurrentObjective(_currentSaveData.currentObjectiveIndex);
            for (int i = 0; i < _currentSaveData.flags.Length; i++)
            {
                if (_currentSaveData.flags[i])
                    _flagManager.ActivateFlag(i);
            }
            _playerBehavior.SetBounds(_currentSaveData.boundLeft, _currentSaveData.boundRight, _currentSaveData.boundUp, _currentSaveData.boundDown);
            _cameraController.SetBounds(_currentSaveData.boundLeft, _currentSaveData.boundRight, _currentSaveData.boundUp, _currentSaveData.boundDown);
        }
    }
}
