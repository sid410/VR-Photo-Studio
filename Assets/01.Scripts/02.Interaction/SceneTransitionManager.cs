using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Handles the scene transition logic by passing the scene index
/// </summary>
public class SceneTransitionManager : MonoBehaviour
{
    // the GameObject in front of the main cam with the FadeScreen script
    public FadeScreen fadeScreen;

    /// <summary>
    /// Fade out current scene; Transition to the next scene (default to fade in at start)
    /// </summary>
    /// <param name="sceneIndex">Enter the scene index to which to transition to</param>
    public void GoToSceneAsync(int sceneIndex)
    {
        StartCoroutine(GoToSceneAsyncRoutine(sceneIndex));
    }

    /// <summary>
    /// Coroutine of the GoToSceneAsync function
    /// </summary>
    private IEnumerator GoToSceneAsyncRoutine(int sceneIndex)
    {
        fadeScreen.FadeOut();

        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneIndex);
        operation.allowSceneActivation = false;

        // wait for the fading animation to finish before loading next scene
        float timer = 0;
        while (timer <= fadeScreen.FadeDuration && !operation.isDone)
        {
            timer += Time.deltaTime;
            yield return null;
        }

        operation.allowSceneActivation = true;
    }
}
