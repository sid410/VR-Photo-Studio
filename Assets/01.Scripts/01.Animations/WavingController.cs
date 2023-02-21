using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// The Waving IK controller during Idle mode
/// </summary>
public class WavingController : MonoBehaviour
{
    // move the IK target between two transforms
    [SerializeField] private GameObject wavingRigTarget, waveOut, waveIn;

    // adjust as needed to get good animation
    [SerializeField] private float speed = 0.45f;
    [SerializeField] private float distanceTolerance = 0.01f;

    private Vector3 firstPos, secondPos, targetPos;
    private Quaternion firstRot, secondRot;

    private float totalDistance, remainingDistance;
    private bool wavingIn;

    private void Start()
    {
        InitializeWavingVariables();
    }

    private void Update()
    {
        UpdateWavingPosition();
        UpdateWavingRotation(remainingDistance/totalDistance);
    }

    /// <summary>
    /// Move back and forth between the two positions
    /// </summary>
    private void UpdateWavingPosition()
    {
        wavingRigTarget.transform.position = Vector3.MoveTowards(wavingRigTarget.transform.position, targetPos, Time.deltaTime * speed);

        // switch to waving out when waving in is completed, and vice versa
        if (Vector3.Distance(wavingRigTarget.transform.position, targetPos) <= distanceTolerance)
        {
            if (targetPos == secondPos)
            {
                targetPos = firstPos;
                wavingIn = false;
            }
            else
            {
                targetPos = secondPos;
                wavingIn = true;
            }
        }

        remainingDistance = Vector3.Distance(wavingRigTarget.transform.position, targetPos);
    }

    /// <summary>
    /// adjust the rotation based from the position results
    /// </summary>
    /// <param name="progress">the percentage completed from start to end position, so we can adjust rotation accordingly</param>
    private void UpdateWavingRotation(float progress)
    {
        if (wavingIn)
        {
            wavingRigTarget.transform.rotation = Quaternion.Lerp(firstRot, secondRot, 1.0f - progress);
        }
        else 
        {
            wavingRigTarget.transform.rotation = Quaternion.Lerp(secondRot, firstRot, 1.0f - progress);
        }
    }

    /// <summary>
    /// Initialize again the waving variables if the Idol Transform changes
    /// </summary>
    public void InitializeWavingVariables()
    {
        /// <remarks>
        /// Start first by waving in
        /// </remarks>
        wavingIn = true;

        firstPos = waveOut.transform.position;
        firstRot = waveOut.transform.rotation;

        secondPos = waveIn.transform.position;
        secondRot = waveIn.transform.rotation;

        wavingRigTarget.transform.position = firstPos;
        wavingRigTarget.transform.rotation = firstRot;

        targetPos = secondPos;

        totalDistance = Vector3.Distance(firstPos, secondPos);
    }

}
