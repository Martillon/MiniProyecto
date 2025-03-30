using UnityEngine;
using UnityEngine.SceneManagement;

public class DeadUIOptions : MonoBehaviour
{
    [Header("Buttons")]
    public GameObject restartButton;
    public GameObject mainMenuButton;
    public string mainMenuScene = "MainMenu";

    public void Restart()
    {
        UIAudio.Instance.PlayClick();
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    
    public void MainMenu()
    {
        UIAudio.Instance.PlayClick();
        SceneManager.LoadScene(mainMenuScene);
    }
}
