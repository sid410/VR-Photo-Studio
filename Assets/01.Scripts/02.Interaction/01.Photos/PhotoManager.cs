using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PhotoManager : MonoBehaviour
{
    [SerializeField] private PhotoSaver picturingMode;
    [SerializeField] private PhotoGallery galleryMode;

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
            Debug.Log("taking picture mode");
        }
        if (galleryModeButton.action.triggered)
        {
            Debug.Log("viewing gallery mode");
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
        if (browseButton.action.ReadValue<Vector2>().x > 0.8f)
        {
            Debug.Log("browsing right");
            IgnoreInputActionForSeconds(browseButton.action, 1);
        }
        else if (browseButton.action.ReadValue<Vector2>().x < -0.8f)
        {
            Debug.Log("browsing left");
            IgnoreInputActionForSeconds(browseButton.action, 1);
        }
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
