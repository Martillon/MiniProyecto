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
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    
    public void MainMenu()
    {
        SceneManager.LoadScene(mainMenuScene);
    }
}
