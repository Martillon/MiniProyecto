using UnityEngine;

public class FinalCollider : MonoBehaviour
{
    [Header("WinCanvas")]
    public GameObject winCanvas;
    
    private Collider thisCollider;
    
    private void Start()
    {
        WaveManager.OnAllWavesCompleted += OnLevelEnd;
        thisCollider = GetComponent<Collider>();
        if (thisCollider == null)
        {
            Debug.LogError("Collider is not assigned in the inspector.");
        }
        thisCollider.isTrigger = true;
        thisCollider.enabled = false;
    }
    
    private void OnLevelEnd()
    {
        thisCollider.enabled = true;
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            winCanvas.SetActive(true);
            Time.timeScale = 0;
        }
    }
}
