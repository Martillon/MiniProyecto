using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class PlayFootstep : MonoBehaviour
{
    [Header("Audio")]
    public AudioClip[] footSteps;
    AudioSource audioSource;

    public void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void PlayFootStep()
    {
        audioSource.clip = footSteps[Random.Range(0, footSteps.Length)];
        audioSource.pitch = Random.Range(0.8f, 1.2f);
        audioSource.volume = Random.Range(0.1f, 0.4f);
        audioSource.PlayOneShot(audioSource.clip);
    }
}
