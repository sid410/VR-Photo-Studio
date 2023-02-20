using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Smooth fade effect to render in front of main camera.
/// </summary>
/// <remarks>
/// Used for scene transition.
/// </remarks>
public class FadeScreen : MonoBehaviour
{
    [SerializeField] private bool fadeOnStart = true;
    [SerializeField] private Color fadeColor;
    [SerializeField] private float fadeDuration = 2.0f; // fading duration in seconds
    public float FadeDuration
    {
        get { return fadeDuration; }
    }

    private Renderer rend;

    private void Start()
    {
        rend = GetComponent<Renderer>();
        if (fadeOnStart)
        {
            FadeIn();
        }
    }

    /// <summary>
    /// Fade from opaque to transparent
    /// </summary>
    public void FadeIn()
    {
        Fade(1, 0);
    }

    /// <summary>
    /// Fade from transparent to opaque
    /// </summary>
    public void FadeOut()
    {
        Fade(0, 1);
    }

    /// <summary>
    /// Function to control the alpha transition of fade screen
    /// </summary>
    /// <param name="alphaIn">Starting alpha value from 0->1 </param>
    /// <param name="alphaOut">Ending alpha value from 0->1 </param>
    public void Fade(float alphaIn, float alphaOut)
    {
        StopAllCoroutines();
        StartCoroutine(FadeRoutine(alphaIn, alphaOut));
    }

    /// <summary>
    /// Coroutine of the Fade function
    /// </summary>
    private IEnumerator FadeRoutine(float alphaIn, float alphaOut)
    {
        float timer = 0;
        while (timer <= fadeDuration)
        {
            Color newColor = fadeColor;
            newColor.a = Mathf.Lerp(alphaIn, alphaOut, timer / fadeDuration);

            rend.material.SetColor("_BaseColor", newColor);

            timer += Time.deltaTime;
            yield return null;
        }

        Color newColorFinal = fadeColor;
        newColorFinal.a = alphaOut;
        rend.material.SetColor("_BaseColor", newColorFinal);
    }
}
