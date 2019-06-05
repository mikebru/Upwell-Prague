using System.Collections;
using UnityEngine;

public class UIController : MonoBehaviour
{
    [Header("Text References")]
    [SerializeField] private TextMesh titleMesh;
    [SerializeField] private TextMesh instructionMesh;
    [Header("Animation Settings")]
    [SerializeField] private float animTimer;

    public void CallAlphaAnimation(bool fadeIn)
    {
        StartCoroutine(TextAnimation(animTimer, fadeIn, titleMesh));
        StartCoroutine(TextAnimation(animTimer, fadeIn, instructionMesh));
    }

    private IEnumerator TextAnimation(float timer, bool fadeIn, TextMesh textMesh)
    {
        textMesh.gameObject.SetActive(true);

        Color currentColor = textMesh.color;
        Color endColor = (fadeIn) ? new Color(currentColor.r, currentColor.g, currentColor.b, 1.0f) : new Color(currentColor.r, currentColor.g, currentColor.b, 0.0f);

        for (float t = 0.0f; t < timer; t += Time.deltaTime)
        {
            textMesh.color = Color.Lerp(currentColor, endColor, t / timer);
            yield return null;
        }

        textMesh.gameObject.SetActive(fadeIn);
    }
}
