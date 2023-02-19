using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;
using TMPro;

/// <summary>
/// The Manager for handling all the idol animations logic, controlled by the dropdown list from menu
/// </summary>
public class IdolAnimationManager : MonoBehaviour
{
    [SerializeField] private GameObject idolModel;
    [SerializeField] private PlayableDirector director;
    [SerializeField] private TMP_Dropdown dropList;
    [SerializeField] private List<TimelineAsset> animList = new List<TimelineAsset>();

    private List<string> animNames = new List<string>();

    private Animator idolAnimator;
    private int animLoopCounter;

    private void Start()
    {
        // get the animator component of the idol model for binding
        idolAnimator = idolModel.GetComponent<Animator>();

        // initialize the playable director with these settings
        director.playOnAwake = false;
        director.extrapolationMode = DirectorWrapMode.None;

        // when animations list is empty, there is no need to generate bindings and listeners
        if (animList == null)
        {
            return;
        }

        // generate the bindings of track to animator for all in the animation list
        foreach (TimelineAsset anim in animList)
        {
            director.SetGenericBinding(anim.GetOutputTrack(0), idolAnimator);
            animNames.Add(anim.name);
        }

        // finally, add the loop to all option
        animNames.Add("Cycle All");
        dropList.AddOptions(animNames);

        // event listener when value of dropdown change
        dropList.onValueChanged.AddListener(delegate
        {
            SetIdolAnimation(dropList.value); 
        });
        // play the first animation on the list
        SetIdolAnimation(0);
    }

    // start listening to animation finished events
    private void ListenStoppedEvents()
    {
        director.stopped += OnPlayableDirectorStopped;
    }

    // stop listening to animation finished events
    private void IgnoreStoppedEvents()
    {
        director.stopped -= OnPlayableDirectorStopped;
    }

    /// <summary>
    /// Raises and event after finishing an animation.
    /// Used for looping through all the animations in the list.
    /// </summary>
    /// <param name="stoppedDirector">for checking the stopped animation</param>
    private void OnPlayableDirectorStopped(PlayableDirector stoppedDirector)
    {
        // director detects stopped event
        if (director == stoppedDirector)
        {
            // loop through the animation list
            if (animLoopCounter < animList.Count - 1)
            {
                animLoopCounter++;
            }
            else 
            {
                animLoopCounter = 0;
            }

            LoopAllAnimations(animLoopCounter);
        }
    }

    /// <summary>
    /// Set the animation based from the input from the dropdown list
    /// </summary>
    /// <param name="index">take the index of the value of the dropdown list</param>
    private void SetIdolAnimation(int index)
    {
        // play the chosen animation from dropdown exactly once
        if (0 <= index && index < animList.Count)
        {
            IgnoreStoppedEvents();

            director.playableAsset = animList[index];
            director.RebuildGraph();
            director.time = 0.0;
            director.Play();
        }
        // loop through all the animations defined in the list
        else if (index == animList.Count)
        {
            ListenStoppedEvents();

            animLoopCounter = 0;
            LoopAllAnimations(animLoopCounter);
        }
    }

    /// <summary>
    /// Method for cycling all the inputted animations in the list.
    /// </summary>
    /// <param name="count">the counter for what animation to play next</param>
    private void LoopAllAnimations(int count)
    {
        director.playableAsset = animList[count];
        director.RebuildGraph();
        director.time = 0.0;
        director.Play();
    }

}
