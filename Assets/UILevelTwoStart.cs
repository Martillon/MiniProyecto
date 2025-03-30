using System;
using UnityEngine;

public class UILevelTwoStart : MonoBehaviour
{
    [Header("UI")]
    public GameObject firstTutorial;
    public GameObject firstObjective;
    public GameObject secondObjective;

    public void Start()
    {
        firstObjective.SetActive(true);
        secondObjective.SetActive(false);
        firstTutorial.SetActive(true);
    }

    public void OnTriggerExit(Collider other)
    {
        firstTutorial.SetActive(false);
    }
}
