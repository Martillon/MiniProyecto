using UnityEngine;

public class TutorialWinCollider : MonoBehaviour
{
    [Header("UI")]
    public GameObject winPanel;

    public void OnTriggerEnter(Collider other)
    {
        winPanel.SetActive(true);
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        Time.timeScale = 0f; // Pause the game
    }
}
