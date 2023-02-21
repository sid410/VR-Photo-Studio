using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

/// <summary>
/// Cycle through the pre-defined photo areas where the player can take pictures of the idol.
/// Areas are defined either during the main menu by the player, or in the ScriptableObject by the designer.
/// </summary>
public class CycleStandPositions : MonoBehaviour
{
    /// <remarks>
    /// The shoot button for placing and specifying the position where to stand.
    /// </remarks>
    [SerializeField] private InputActionProperty placeButton;

    // the origin of the XR user which has the main camera as a child
    [SerializeField] private GameObject playerOrigin;

    // the idol the user needs to face towards to take photos
    [SerializeField] private GameObject idolTarget;

    // to transition back to main menu after completing one cycle
    [SerializeField] private SceneTransitionManager sceneTransition;

    // the flash FX used when taking photos
    [SerializeField] private FlashScreen flashScreen;

    // the script for saving photos
    [SerializeField] private PhotoSaver photoSaver;

    // the ScriptableObject to pass all the data to regarding number of photos and where to stand
    [SerializeField] private CameraSettings cameraSettingsValues;

    private Vector3[] cyclePositions;
    private int numPicsToTake, picsTaken;
    private bool sessionFinished = false;
    private bool mainMenuLoading = false;

    /// <remarks>
    /// Only take a photo when both grip and activate button are pressed.
    /// Do not forget to trigger this in the event of XR Grab Interactable attached in camera model
    /// </remarks>
    private bool camModelSelected = false;
    public bool CamModelSelected
    {
        set { camModelSelected = value; }
    }

    private void Start()
    {
        /// <remarks>
        /// When there are no stand positions defined, immediately go back to main scene
        /// This is without doing the fading animation
        /// </remarks>
        if (cameraSettingsValues.standPositions.Count == 0)
        {
            SceneManager.LoadScene(0);
            enabled = false;
            return;
        }

        // choose whichever is lower from the max photos set vs the number of anchors set
        // in other words, clip how much the player can take in one session
        if (cameraSettingsValues.maxPhotos <= cameraSettingsValues.standPositions.Count)
        {
            numPicsToTake = cameraSettingsValues.maxPhotos;
        }
        else
        {
            numPicsToTake = cameraSettingsValues.standPositions.Count;
        }
        // initialize the array to cycle all the positions
        cyclePositions = new Vector3[numPicsToTake];

        // pass the set positions from the main menu to the cycle array
        for (int i = 0; i < numPicsToTake; i++)
        {
            cyclePositions[i] = cameraSettingsValues.standPositions[i];
        }

        // initialize the counter for photos taken to zero
        picsTaken = 0;
        // then start taking photos from the first set point
        MoveOriginToNewPhotoArea();
    }

    private void Update()
    {
        /// <remarks>
        /// When session is finished, wait for the flashing to end before going to main menu
        /// Because scene transition is an async function, make sure this is only called once
        /// </remarks>
        if (!flashScreen.IsFlashing && sessionFinished && !mainMenuLoading)
        {
            sceneTransition.GoToSceneAsync(0);
            mainMenuLoading = true;
        }

        /// <remarks>
        /// trigger only when holding the camera, releasing the button, and the previous flashing animation is finished
        /// and also the condition when it is in picture taking mode and not in gallery mode.
        /// </remarks>
        if (placeButton.action.WasReleasedThisFrame() && camModelSelected && !flashScreen.IsFlashing && photoSaver.isActiveAndEnabled)
        {
            // save a copy of the photo first then flash
            photoSaver.SavePhoto();
            flashScreen.Flash();
            
            // go back to main menu after looping once the predefined photo areas
            if (picsTaken >= numPicsToTake)
            {
                // finish the session, but wait for flashing to end before exit
                sessionFinished = true;
                return;
            }

            // move to new photo area after saving the photo
            MoveOriginToNewPhotoArea();
        }
    }

    /// <summary>
    /// Update the transform to new photo area, while looking at the idol,
    /// then increment to move to next photo area.
    /// </summary>
    private void MoveOriginToNewPhotoArea()
    {
        playerOrigin.transform.position = cyclePositions[picsTaken];
        playerOrigin.transform.LookAt(idolTarget.transform, Vector3.up);

        // if the 3D camera is more than 1 meter away, move it between the player and idol
        if (Vector3.Distance(gameObject.transform.position, playerOrigin.transform.position) > 1)
        {
            gameObject.transform.position = (playerOrigin.transform.position + idolTarget.transform.position) / 2;
        }

        picsTaken++;
    }
}
