using System.Collections;
using UnityEngine;

public class ExplossionEffect : MonoBehaviour
{
    [Header("Visual")]
    public float maxSize = 3f;
    public float duration = 0.3f;
    public AnimationCurve scaleCurve;

    [Header("Opcional")]
    public bool destroyAfter = true;

    private Vector3 initialScale;

    private void Start()
    {
        initialScale = transform.localScale;
        StartCoroutine(ExplodeRoutine());
    }

    private IEnumerator ExplodeRoutine()
    {
        float timer = 0f;

        while (timer < duration)
        {
            float scale = scaleCurve.Evaluate(timer / duration) * maxSize;
            transform.localScale = initialScale + Vector3.one * scale;
            timer += Time.deltaTime;
            yield return null;
        }

        if (destroyAfter)
            Destroy(gameObject);
    }
}
