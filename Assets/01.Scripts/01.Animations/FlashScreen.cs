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

    private bool isFlashing = false;
    public bool IsFlashing
    {
        get { return isFlashing; }
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
        /// <remarks>
        /// Only on the off chance that this function is called while still doing the flashing Flash animation,
        /// we force stop the current flash animation.
        /// </remarks>
        StopAllCoroutines();

        isFlashing = true;
        StartCoroutine(FlashRoutine());
    }

    /// <summary>
    /// Coroutine of the Flash function
    /// </summary>
    private IEnumerator FlashRoutine()
    {
        yield return ChangeAlphaRoutine(0, 1, flashDurationIn);
        yield return ChangeAlphaRoutine(1, 0, flashDurationOut);
        yield return FinishFlashing();
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

    // add here later other stuff to do when finished flashing
    private IEnumerator FinishFlashing()
    {
        isFlashing = false;
        yield return null;
    }
}
