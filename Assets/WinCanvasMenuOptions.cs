using UnityEngine;
using UnityEngine.SceneManagement;

public class WinCanvasMenuOptions : MonoBehaviour
{
    [Header("UI")]
    public string NextScene = "Level Two";
    
    public void NextLevel()
    {
        UIAudio.Instance.PlayClick();
        SceneManager.LoadScene(NextScene);
    }

    public void MainMenu()
    {
        UIAudio.Instance.PlayClick();
        SceneManager.LoadScene(0);
    }
}
