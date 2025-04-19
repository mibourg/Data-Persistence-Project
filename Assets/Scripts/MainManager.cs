using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class MainManager : MonoBehaviour
{
    public static MainManager Instance;
    
    public Brick BrickPrefab;
    public int LineCount = 6;
    public Rigidbody Ball;

    public Text ScoreText;
    public GameObject GameOverText;
    
    private bool m_Started = false;
    private int m_Points;
    
    private bool m_GameOver = false;
    
    public int highScore;
    public string highScorePlayerName;
    public Text highScoreText;

    public string enteredName = "";
    
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            LoadData();
        }
        else
        {
            Destroy(gameObject);
        }
    }
    
    // Start is called before the first frame update
    private void Start()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.buildIndex == 1)
        {
            StartGame();
        }
    }

    private void Update()
    {
        if (!m_Started)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                m_Started = true;
                float randomDirection = Random.Range(-1.0f, 1.0f);
                Vector3 forceDir = new Vector3(randomDirection, 1, 0);
                forceDir.Normalize();

                Ball.transform.SetParent(null);
                Ball.AddForce(forceDir * 2.0f, ForceMode.VelocityChange);
            }
        }
        else if (m_GameOver)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            }
        }
    }

    void AddPoint(int point)
    {
        m_Points += point;
        ScoreText.text = $"Score: {m_Points}";
        if (m_Points > highScore)
        {
            highScore = m_Points;
            highScorePlayerName = enteredName;
            UpdateHighScoreText();
        } 
    }

    public void GameOver()
    {
        m_GameOver = true;
        GameOverText.SetActive(true);
    }
    
    public void StartGame()
    {
        m_GameOver = false;
        m_Started = false;
        m_Points = 0;

        ScoreText = GameObject.Find("ScoreText").GetComponent<Text>();
        GameOverText = GameObject.Find("GameoverText");
        GameOverText.SetActive(false);
        Ball = GameObject.Find("Ball").GetComponent<Rigidbody>();
        highScoreText = GameObject.Find("BestScoreText").GetComponent<Text>();

        if (highScore != 0)
        {
            UpdateHighScoreText();
        }
        
        const float step = 0.6f;
        int perLine = Mathf.FloorToInt(4.0f / step);
        
        int[] pointCountArray = new [] {1,1,2,2,5,5};
        for (int i = 0; i < LineCount; ++i)
        {
            for (int x = 0; x < perLine; ++x)
            {
                Vector3 position = new Vector3(-1.5f + step * x, 2.5f + i * 0.3f, 0);
                var brick = Instantiate(BrickPrefab, position, Quaternion.identity);
                brick.PointValue = pointCountArray[i];
                brick.onDestroyed.AddListener(AddPoint);
            }
        }
    }

    private void UpdateHighScoreText()
    {
        highScoreText.text = $"Best Score: {highScore} by {highScorePlayerName}";
    }
    
    [Serializable]
    private class Data
    {
        public int highScore;
        public string highScorePlayerName;
    }

    public void SaveData()
    {
        Data data = new Data();
        data.highScore = highScore;
        data.highScorePlayerName = highScorePlayerName;
        string json = JsonUtility.ToJson(data);
        File.WriteAllText(Application.persistentDataPath + "/savefile.json", json);
    }

    public void LoadData()
    {
        string path = Application.persistentDataPath + "/savefile.json";
        if (File.Exists(path))
        {
            string json = File.ReadAllText(Application.persistentDataPath + "/savefile.json");
            Data data = JsonUtility.FromJson<Data>(json);
            highScore = data.highScore;
            highScorePlayerName = data.highScorePlayerName;
        }
    }
}
