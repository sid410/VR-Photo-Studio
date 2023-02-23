using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// Toggle the Menu, spawn in front, and always face the user.
/// </summary>
public class ToggleMenu : MonoBehaviour
{
    [SerializeField] private Transform headCam; // main camera of the XR rig
    [SerializeField] private float spawnDistance = 1.5f; // spawn the menu 1.5m in front of user
    [SerializeField] private GameObject canvas, leftControllerCanvas, rightControllerCanvas;

    /// <remarks>
    /// Set this to the Menu button, in case of Meta Quest 2 in the left hand
    /// </remarks>
    public InputActionProperty showButton;

    private void Update()
    {
        // toggle the menu on/off
        if (showButton.action.WasPressedThisFrame())
        {
            ToggleCanvases();

            canvas.transform.position = headCam.position + (new Vector3(headCam.forward.x, 0, headCam.forward.z).normalized * spawnDistance);
        }

        // billboard functionality of menu to always face user
        canvas.transform.LookAt(new Vector3(headCam.position.x, canvas.transform.position.y, headCam.position.z));
        canvas.transform.forward *= -1;
    }

    /// <summary>
    /// Toggle the canvases On/Off if not null
    /// This way, we can just set the GameObject to none if we do not want to toggle
    /// </summary>
    private void ToggleCanvases()
    {
        if (leftControllerCanvas != null)
        {
            leftControllerCanvas.SetActive(!leftControllerCanvas.activeSelf);
        }
        if (rightControllerCanvas != null)
        {
            rightControllerCanvas.SetActive(!rightControllerCanvas.activeSelf);
        }
        if (canvas != null)
        {
            canvas.SetActive(!canvas.activeSelf);
        }
    }
}
