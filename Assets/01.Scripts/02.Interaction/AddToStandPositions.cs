using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.InputSystem;

/// <summary>
/// The script for adding where to stand during the locomotion system at game time.
/// </summary>
public class AddToStandPositions : MonoBehaviour
{
    /// <remarks>
    /// The shoot button for placing and specifying the position where to stand.
    /// </remarks>
    [SerializeField] private InputActionProperty placeButton;

    [SerializeField] private GameObject camRay;
    [SerializeField] private GameObject camShutter;
    [SerializeField] private GameObject teleportAnchorPrefab; // the prefab to spawn and where to teleport during real game time
    [SerializeField] private FlashScreen flashScreen; // the flash FX used when taking photos

    private XRRayInteractor camRayInteractor;
    private XRInteractorLineVisual lineVisual;

    // to store where the ray and object intersect
    private Vector3 hitPosition = Vector3.zero;

    // the ScriptableObject to pass all the data to regarding number of photos and where to stand
    [SerializeField] private CameraSettings cameraSettingsValues;

    /// <remarks>
    /// Only show the camera ray when both grip and activate button are pressed.
    /// Do not forget to trigger this in the event of XR Grab Interactable attached in camera model
    /// </remarks>
    private bool camModelSelected = false;
    public bool CamModelSelected
    {
        set { camModelSelected = value; }
    }

    private void Start()
    {
        // initialize the interactor and ray visualization
        camRayInteractor = camRay.GetComponent<XRRayInteractor>();
        lineVisual = camRay.GetComponent<XRInteractorLineVisual>();

        // load the existing teleport anchors stored at the CameraSettings ScriptableObject at start
        LoadTeleportAnchors();
    }

    private void Update()
    {
        /// <remarks>
        /// Spawn a teleport anchor once when the shoot button is released
        /// </remarks>
        if (placeButton.action.WasReleasedThisFrame() && hitPosition != Vector3.zero)
        {
            SpawnTeleportAnchor(hitPosition);
            hitPosition = Vector3.zero;
        }

        /// <remarks>
        /// Only when holding the camera plus holding the shoot button, will show the camera ray
        /// </remarks>
        if (camModelSelected && placeButton.action.IsPressed())
        {
            lineVisual.enabled = true;

            camRayInteractor.TryGetHitInfo(out hitPosition, out Vector3 hitNormal, out int hitInLine, out bool hitValid);
        }
        else
        {
            lineVisual.enabled = false;
        }
    }

    /// <summary>
    /// Spawn a Teleport anchor to the hit position of the ray to the ground on release.
    /// Also call the flash FX before spawning.
    /// </summary>
    /// <param name="position">the hit position between ray and ground</param>
    private void SpawnTeleportAnchor(Vector3 position)
    {
        flashScreen.Flash();
        cameraSettingsValues.standPositions.Add(position);
        Instantiate(teleportAnchorPrefab, position, Quaternion.identity);
    }

    private void LoadTeleportAnchors()
    {
        foreach (Vector3 pos in cameraSettingsValues.standPositions)
        {
            Instantiate(teleportAnchorPrefab, pos, Quaternion.identity);
        }
    }
}
