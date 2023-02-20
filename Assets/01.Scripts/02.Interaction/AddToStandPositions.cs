using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.InputSystem;

/// <summary>
/// The script for adding where to stand during the locomotion system at game time
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

    private XRRayInteractor camRayInteractor;
    private XRInteractorLineVisual lineVisual;

    // to store where the ray and object intersect
    private Vector3 hitPosition = Vector3.zero;

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
        camRayInteractor = camRay.GetComponent<XRRayInteractor>();
        lineVisual = camRay.GetComponent<XRInteractorLineVisual>();
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
    /// 
    /// </summary>
    /// <param name="position"></param>
    private void SpawnTeleportAnchor(Vector3 position)
    {
        Instantiate(teleportAnchorPrefab, position, Quaternion.identity);
    }
}
