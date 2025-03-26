using System;
using UnityEngine;

public class TutorialUI : MonoBehaviour
{
    [Header("UI")]
    public GameObject tutorialUI;
    public bool hasBeenActivated;
    
    public void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;
        if (hasBeenActivated) return;
        tutorialUI.SetActive(true);
        hasBeenActivated = true;
    }
    
    public void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            tutorialUI.SetActive(false);
        }
    }
}
