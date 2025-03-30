using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChanger : MonoBehaviour
{
    public void ChangeScene(string scene)
    {
        UIAudio.Instance.PlayClick();
        SceneManager.LoadScene(scene);
    }
}
