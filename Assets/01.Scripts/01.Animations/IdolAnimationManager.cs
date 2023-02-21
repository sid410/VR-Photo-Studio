using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;
using TMPro;
using UnityEngine.Animations.Rigging;

/// <summary>
/// The Manager for handling all the idol animations logic, controlled by the dropdown list from menu
/// </summary>
public class IdolAnimationManager : MonoBehaviour
{
    [SerializeField] private Rig rig;
    [SerializeField] private GameObject idolModel;
    [SerializeField] private PlayableDirector director;
    [SerializeField] private TMP_Dropdown dropList;
    [SerializeField] private List<TimelineAsset> animList = new List<TimelineAsset>();

    private List<string> animNames = new List<string>();

    private Animator idolAnimator;
    private int animLoopCounter;
    private bool isLooping;

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

        // finally, add the loop and idle to options
        animNames.Add("Cycle All");
        animNames.Add("Idle");
        dropList.AddOptions(animNames);
        
        // event listener when value of dropdown change
        dropList.onValueChanged.AddListener(delegate
        {
            SetIdolAnimation(dropList.value); 
        });

        // and listen to the stopped events
        ListenStoppedEvents();

        // play the idle animation, as it is the last
        dropList.value = animNames.Count - 1;
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
            if (!isLooping)
            {
                // set to idle
                dropList.value = animNames.Count - 1;
                return;
            }

            // loop through the animation list
            LoopAllAnimations();
        }
    }

    /// <summary>
    /// Set the animation based from the input from the dropdown list
    /// </summary>
    /// <param name="index">take the index of the value of the dropdown list</param>
    private void SetIdolAnimation(int index)
    {
        // play the chosen animation from dropdown exactly once
        if (0 <= index && index < animNames.Count - 2)
        {
            isLooping = false;

            StartWavingAnimation(false);

            PlayAnimationOfIndex(index);
        }
        // loop through all the animations defined in the list
        else if (index == animNames.Count - 2)
        {
            isLooping = true;

            StartWavingAnimation(false);

            LoopAllAnimations();
        }
        // go to idle mode
        else if (index == animNames.Count - 1)
        {
            isLooping = false;
            director.Stop();

            // only wave when in idle mode
            StartWavingAnimation(true);
        }
    }

    /// <summary>
    /// Method for cycling all the inputted animations in the list.
    /// </summary>
    private void LoopAllAnimations()
    {
        if (animLoopCounter < animNames.Count - 2)
        {
            PlayAnimationOfIndex(animLoopCounter);
            animLoopCounter++;
        }
        else
        {
            animLoopCounter = 0;
            PlayAnimationOfIndex(animLoopCounter);
        }
    }

    /// <summary>
    /// Play the animation based from the dropdown index
    /// </summary>
    private void PlayAnimationOfIndex(int index)
    {
        director.playableAsset = animList[index];
        director.RebuildGraph();
        director.time = 0.0;
        director.Play();
    }

    /// <summary>
    /// Turn On/Off the waving animation by changing the rig weight
    /// </summary>
    /// <param name="wavingStart">set to true to start waving animation</param>
    private void StartWavingAnimation(bool wavingStart)
    {
        if (wavingStart)
        {
            rig.weight = 1.0f;
        }
        else
        {
            rig.weight = 0.0f;
        }
    }
}
