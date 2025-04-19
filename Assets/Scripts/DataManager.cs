using System;
using UnityEditor.Overlays;
using UnityEngine;

public class DataManager : MonoBehaviour
{
    public static DataManager Instance;

    private int highScore;
    private string playerName;

    private string enteredName;
    
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    [Serializable]
    private class Data
    {
        public int highScore;
        public string playerName;
    }

    public void SaveData()
    {
        
    }
}
