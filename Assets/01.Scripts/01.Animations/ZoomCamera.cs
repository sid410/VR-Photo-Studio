using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// For zooming in/out the display rendered in the camera 3D game object
/// </summary>
public class ZoomCamera : MonoBehaviour
{
    /// <remarks>
    /// Set this to the Right hand Move button
    /// </remarks>
    [SerializeField] private InputActionProperty zoomButton;

    /// <remarks>
    /// The GameObject which contains the 3D camera shutter
    /// </remarks>
    [SerializeField] private Camera camObject;

    /// <remarks>
    /// Zoom settings for clipping and zoom speed
    /// </remarks>
    [SerializeField] private float maxFOV = 60.0f;
    [SerializeField] private float minFOV = 20.0f;
    [SerializeField] private float zoomSteps = 1.0f;

    // the ScriptableObject to pass all the data to regarding zoom value
    [SerializeField] private CameraSettings cameraSettingsValues;

    /// <remarks>
    /// Only zoom if the 3D camera model is selected.
    /// Do not forget to trigger this in the event of XR Grab Interactable attached in camera model
    /// </remarks>
    private bool camModelSelected = false;
    public bool CamModelSelected
    {
        set { camModelSelected = value; }
    }

    /// <summary>
    /// load the saved zoom data from the CameraSettings scriptable object
    /// </summary>
    private void Start()
    {
        LoadZoom();
    }

    private void FixedUpdate()
    {
        // skip the zoom functions if 3D model of cam is not selected
        if (!camModelSelected)
        {
            return;
        }

        // zoom in or out depending if right controller is pressed forward or backward
        if (zoomButton.action.ReadValue<Vector2>().y > 0.8f)
        {
            ZoomFieldOfView(true);
        }
        else if (zoomButton.action.ReadValue<Vector2>().y < -0.8f)
        {
            ZoomFieldOfView(false);
        }
    }

    /// <summary>
    /// Function for changing the value of the Field of view to animate the zoom effect
    /// </summary>
    /// <param name="zoomIn">set to true if zooming in, set to false if zooming out</param>
    private void ZoomFieldOfView(bool zoomIn)
    {
        float currentView = camObject.fieldOfView;

        // clip the field of view
        if (currentView > maxFOV)
        {
            currentView = maxFOV - zoomSteps;
        }
        else if (currentView < minFOV)
        {
            currentView = minFOV + zoomSteps;
        }
        // increment or decrement the FoV to change zoom
        else
        {
            if (zoomIn)
            {
                currentView -= zoomSteps;
            }
            else
            {
                currentView += zoomSteps;
            }
        }

        // finally update the FoV for both the cam object and scriptable object cam settings
        camObject.fieldOfView = cameraSettingsValues.camZoom = currentView;
    }

    /// <summary>
    /// load the saved zoom data from camera settings to the working camera
    /// </summary>
    private void LoadZoom()
    {
        camObject.fieldOfView = cameraSettingsValues.camZoom;
    }

}
