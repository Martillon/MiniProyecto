using UnityEngine;
using UnityEngine.SceneManagement;

public class FinishGoalScript : MonoBehaviour
{
    [Header("UI")]
    public GameObject winUI;
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            winUI.SetActive(true);
            Time.timeScale = 0;
        }
    }
}
