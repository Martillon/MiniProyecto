using System;
using UnityEngine;
using UnityEngine.Serialization;

public class Level2EndScript : MonoBehaviour
{
    [Header("UI")]
    public GameObject endLabel;
    public GameObject previousLabel;
    public GameObject winPanel;
    public Collider thisCollider;
    
    public void Start()
    {
        WaveManager.OnAllWavesCompleted += OnLevelEnd;
        thisCollider = GetComponent<Collider>();
        thisCollider.enabled = false;
        winPanel.SetActive(false);
    }

    private void OnLevelEnd()
    {
        thisCollider.enabled = true;
        previousLabel.SetActive(false);
        endLabel.SetActive(true);
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            winPanel.SetActive(true);
            Time.timeScale = 0;
        }
    }

    public void OnDestroy()
    {
        WaveManager.OnAllWavesCompleted -= OnLevelEnd;
    }
}
