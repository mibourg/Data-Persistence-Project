using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuUI : MonoBehaviour
{
    public TextMeshProUGUI bestScoreText;

    public TMP_InputField nameInput;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (MainManager.Instance.highScore != 0)
        {
            bestScoreText.text = "Best Score: " + MainManager.Instance.highScore + " by " +
                                 MainManager.Instance.highScorePlayerName;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnNameChanged()
    {
        MainManager.Instance.enteredName = nameInput.text;
    }
    
    public void QuitGame()
    {
        #if UNITY_EDITOR
        UnityEditor.EditorApplication.ExitPlaymode();
        #else 
        Application.Quit();
        #endif
        
        MainManager.Instance.SaveData();
    }

    public void StartGame()
    {
        SceneManager.LoadScene(1);
    }
}
