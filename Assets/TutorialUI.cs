using System;
using UnityEngine;

public class TutorialUI : MonoBehaviour
{
    [Header("UI")]
    public GameObject tutorialUI;

    private bool hasBeenActivated;
    
    public void OnTriggerEnter(Collider other)
    {
        if (CompareTag("Player"))
        {
            if (!hasBeenActivated)
            {
                tutorialUI.SetActive(true);
                hasBeenActivated = true;
            }
        }
    }
    
    public void OnTriggerExit(Collider other)
    {
        if (CompareTag("Player"))
        {
            tutorialUI.SetActive(false);
        }
    }
}
