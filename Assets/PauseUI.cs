using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseUI : MonoBehaviour
{
    [Header("Buttons")]
    public GameObject pauseCanvas;
    public GameObject resumeButton;
    public GameObject backToMenuButton;
    
    [Header("Keybinds")]
    public KeyCode pauseKey = KeyCode.Escape;
    
    [Header("Scene names")]
    public string mainMenuScene = "MainMenu";
    
    private void Start()
    {
        pauseCanvas.SetActive(false);
    }
    
    private void Update()
    {
        if (Input.GetKeyDown(pauseKey))
        {
            if (Time.timeScale == 0)
            {
                Time.timeScale = 1;
                pauseCanvas.SetActive(false);
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
            }
            else
            {
                Time.timeScale = 0;
                pauseCanvas.SetActive(true);
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
            }
        }
    }
    
    public void ResumeButton()
    {
        Time.timeScale = 1;
        pauseCanvas.SetActive(false);
    }
    
    public void BackToMenuButton()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(mainMenuScene);
    }
}
