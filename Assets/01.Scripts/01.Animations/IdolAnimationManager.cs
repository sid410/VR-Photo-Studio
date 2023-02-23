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
    [SerializeField] private TwoBoneIKConstraint wavingRig;
    [SerializeField] private MultiAimConstraint hipRig, bodyRig, headRig;
    [SerializeField] private GameObject idolModel;
    [SerializeField] private PlayableDirector director;
    [SerializeField] private TMP_Dropdown dropList;
    [SerializeField] private List<TimelineAsset> animList = new List<TimelineAsset>();

    [SerializeField] private string danceAnimationName;
    [SerializeField] private AudioSource danceAudio;

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
            // always set loop to false, as this value will be changed when choosing Cycle All option
            isLooping = false;
            // set to the chosen animation
            SetIdolAnimation(dropList.value);
            // and enable the dance music if they choose the option of dancing
            SetDanceMusic(dropList.options[dropList.value].text);
        });
        
        // and listen to the stopped events
        ListenStoppedEvents();

        // set the default to Idle animation
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
        // director detects stopped event and we choose Cycle All animations from dropdown
        if (director == stoppedDirector)
        {
            if (isLooping)
            {
                /// <remarks>
                /// Play the next animation
                /// This will automatically loop back to first animation if current animation is the last one
                /// </remarks>
                SetIdolAnimation(animLoopCounter++);
            }
            else // go back to Idle mode
            {
                // this fires an event, so the SetIdolAnimation is called there
                dropList.value = animNames.Count - 1;
            }
        }
    }

    /// <summary>
    /// Set the animation based from the input from the dropdown list
    /// </summary>
    /// <param name="index">take the index of the value of the dropdown list</param>
    private void SetIdolAnimation(int index)
    {
        // play the corresponding animation
        if (0 <= index && index < animNames.Count - 2)
        {
            PlayAnimationOfIndex(index);
            StartWavingAnimation(false);
        }
        // cycle through all the animations starting from the very top
        else if (index == animNames.Count - 2)
        {
            isLooping = true;

            // because we already start playing the animation 0, set counter to next
            animLoopCounter = 1;
            PlayAnimationOfIndex(0);
            StartWavingAnimation(false);
        }
        else // if idle 
        {
            // force stop the current animation and start waving again
            director.Stop();
            StartWavingAnimation(true);
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
            wavingRig.weight = 1.0f;
            hipRig.weight = 1.0f;
            bodyRig.weight = 1.0f;
            headRig.weight = 1.0f;
        }
        else
        {
            wavingRig.weight = 0.0f;
            hipRig.weight = 0.0f;
            bodyRig.weight = 0.0f;
            headRig.weight = 0.0f;
        }
    }

    /// <summary>
    /// Plays the dance music when the dance option is chosen
    /// </summary>
    /// <param name="dropListName">the filename of the dance animation (without the extension)</param>
    private void SetDanceMusic(string dropListName)
    {
        if (dropListName == danceAnimationName)
        {
            danceAudio.Play();
        }
        else 
        {
            danceAudio.Stop();
        }
    }
}
