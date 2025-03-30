using System;
using UnityEngine;

public class ColliderWaveCombat : MonoBehaviour
{
    [Header("UI Settings")]
    public GameObject waveUI;
    public GameObject previousUI;
    
    [Header("GameObject Settings")]
    public GameObject colliderToEnable;
    public WaveManager waveManager;

    public void Start()
    {
        waveManager =GetComponent<WaveManager>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            waveUI.SetActive(true);
            previousUI.SetActive(false);
            colliderToEnable.SetActive(true);
            waveManager.StartCombat();
        }
    }
    
}
