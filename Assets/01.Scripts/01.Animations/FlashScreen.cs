using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Flash effect to render in front of main camera.
/// </summary>
/// <remarks>
/// Called when taking photos.
/// Also called when specifying the teleport anchors.
/// </remarks>
public class FlashScreen : MonoBehaviour
{
    [SerializeField] private Color flashColor;
    [SerializeField] private float flashDurationIn = 0.05f; // flashing duration in seconds from transparent to opaque
    [SerializeField] private float flashDurationOut = 0.45f; // flashing duration in seconds from opaque to transparent

    public float FlashDuration
    {
        get { return flashDurationIn + flashDurationOut; }
    }

    private Renderer rend;

    private void Start()
    {
        rend = GetComponent<Renderer>();
    }

    /// <summary>
    /// Function to call when want to flash after taking picture
    /// </summary>
    public void Flash()
    {
        StopAllCoroutines();
        StartCoroutine(FlashRoutine());
    }

    /// <summary>
    /// Coroutine of the Flash function
    /// </summary>
    private IEnumerator FlashRoutine()
    {
        yield return ChangeAlphaRoutine(0, 1, flashDurationIn);
        yield return ChangeAlphaRoutine(1, 0, flashDurationOut);
    }

    /// <summary>
    /// Coroutine for changing the alpha
    /// </summary>
    /// <param name="alphaIn">starting alpha</param>
    /// <param name="alphaOut">ending alpha</param>
    /// <param name="duration">how long to lerp from start to end of alpha</param>
    /// <returns></returns>
    private IEnumerator ChangeAlphaRoutine(float alphaIn, float alphaOut, float duration)
    {
        float timer = 0;
        while (timer <= duration)
        {
            Color newColor = flashColor;
            newColor.a = Mathf.Lerp(alphaIn, alphaOut, timer / duration);

            rend.material.SetColor("_BaseColor", newColor);

            timer += Time.deltaTime;
            yield return null;
        }

        Color newColorFinal = flashColor;
        newColorFinal.a = alphaOut;
        rend.material.SetColor("_BaseColor", newColorFinal);
    }
}
