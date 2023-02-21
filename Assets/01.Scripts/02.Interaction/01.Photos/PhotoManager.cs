using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PhotoManager : MonoBehaviour
{
    /// <remarks>
    /// Set this to the Right hand secondary button
    /// </remarks>
    [SerializeField] private InputActionProperty picturingModeButton;

    /// <remarks>
    /// Set this to the Right hand primary button
    /// </remarks>
    [SerializeField] private InputActionProperty galleryModeButton;

    /// <remarks>
    /// Set this to the Left hand secondary button
    /// </remarks>
    [SerializeField] private InputActionProperty loadButton;

    /// <remarks>
    /// Set this to the Left hand primary button
    /// </remarks>
    [SerializeField] private InputActionProperty deleteButton;

    /// <remarks>
    /// Set this to the Left hand Move button
    /// </remarks>
    [SerializeField] private InputActionProperty browseButton;

    [SerializeField] private GameObject camDisplayLive;
    [SerializeField] private GameObject camDisplayGallery;

    [SerializeField] private PhotoGallery gallery;

    private void Update()
    {
        UpdateInputActions();
    }

    /// <summary>
    /// This is where to map the logic of what functions to do for each input events.
    /// </summary>
    private void UpdateInputActions()
    {
        // buttons that are triggered once
        if (picturingModeButton.action.triggered)
        {
            SwitchCameraModeToLive(true);
        }
        if (galleryModeButton.action.triggered)
        {
            SwitchCameraModeToLive(false);
        }
        if (loadButton.action.triggered)
        {
            Debug.Log("refreshing gallery");
        }
        if (deleteButton.action.triggered)
        {
            Debug.Log("deleting picture");
        }

        // joystick actions which are delayed for scrolling left/right
        if (camDisplayGallery.activeInHierarchy)
        {
            if (browseButton.action.ReadValue<Vector2>().x > 0.8f)
            {
                gallery.ViewNextOrPreviousPhoto(true);
                IgnoreInputActionForSeconds(browseButton.action, 1);
            }
            else if (browseButton.action.ReadValue<Vector2>().x < -0.8f)
            {
                gallery.ViewNextOrPreviousPhoto(false);
                IgnoreInputActionForSeconds(browseButton.action, 1);
            }
        }
    }

    /// <summary>
    /// switch between taking pictures mode and gallery mode
    /// </summary>
    /// <param name="isLiveMode">set to true when taking pictures, false to view gallery</param>
    private void SwitchCameraModeToLive(bool isLiveMode)
    {
        camDisplayLive.SetActive(isLiveMode);
        camDisplayGallery.SetActive(!isLiveMode);
    }

    /// <summary>
    /// For ignoring input action events for a certain interval.
    /// </summary>
    /// <param name="action">the input action</param>
    /// <param name="seconds">how much time to delay until start listening again, in seconds</param>
    private void IgnoreInputActionForSeconds(InputAction action, float seconds)
    {
        StartCoroutine(IgnoreInputActionCoroutine(action, seconds));
    }

    /// <summary>
    /// The Coroutine function for IgnoreInputActionForSeconds
    /// </summary>
    private IEnumerator IgnoreInputActionCoroutine(InputAction action, float seconds)
    {
        action.Disable();
        yield return new WaitForSeconds(seconds);
        action.Enable();
    }
}
