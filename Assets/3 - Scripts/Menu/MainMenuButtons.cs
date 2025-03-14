using UnityEngine;

public class MainMenuButtons : MonoBehaviour
{
    [Header("Groups")]
    public GameObject mainMenu;
    public GameObject optionsMenu;
    
    private void Start()
    {
        if (mainMenu != null && optionsMenu != null)
        {
            mainMenu.SetActive(true);
            optionsMenu.SetActive(false);
        }
        
        if (mainMenu == null) Debug.LogError("Main Menu is not assigned");
        if (optionsMenu == null) Debug.LogError("Options Menu is not assigned");
    }
    
    public void PlayButton()
    {
        //Change scene to level 1
    }
    
    public void OptionsButton()
    {
        mainMenu.SetActive(false);
        optionsMenu.SetActive(true);
    }
    
    public void BackButton()
    {
        mainMenu.SetActive(true);
        optionsMenu.SetActive(false);
    }
    
    public void ExitButton()
    {
        #if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;
        #else
            Application.Quit();
        #endif
    }
}
