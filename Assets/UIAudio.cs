using UnityEngine;

public class UIAudio : MonoBehaviour
{
    public static UIAudio Instance;

    [SerializeField] private AudioSource source;
    [SerializeField] private AudioClip click;

    private void Awake()
    {
        Instance = this;
        DontDestroyOnLoad(this);
        source = GetComponent<AudioSource>();
    }

    public void PlayClick()
    {
        source.PlayOneShot(click);
    }
}
