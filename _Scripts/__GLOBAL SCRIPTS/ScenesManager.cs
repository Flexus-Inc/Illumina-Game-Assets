/*
    Author          : Ferrer, Edrian Jose D.G.
    Collaborators   : None yet
    Organization    : Flexus Group of Companies 
    Version         : 2.0

 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ScenesManager : MonoBehaviour {

    public Animator[] SceneTransitions;
    public Animator[] LoadingTransitions;
    public float[] SceneTransitionsTime;
    public float[] LoadingTransitionsTime;
    public static Animator SceneTransition;
    public static Animator LoadingTransition;
    public static Dictionary<int, List<NotificationObject>> SceneStartUpMessages = new Dictionary<int, List<NotificationObject>>();
    public static int ActiveTransitionIndex = 0;
    public static int ActiveLoadingTransitionIndex = 0;

    //Status variables are shared to TransitionManagers
    public static int transition_status = 0;
    public static int loading_status = 0;

    void Start() {
        if (SceneTransition == null) {
            SceneTransition = SceneTransitions[ScenesManager.ActiveTransitionIndex];
        }
        if (LoadingTransition == null) {
            LoadingTransition = LoadingTransitions[ScenesManager.ActiveLoadingTransitionIndex];
        }
    }

    public void Activate(bool activated = true) {
        SceneTransition.gameObject.SetActive(activated);
        LoadingTransition.gameObject.SetActive(activated);
    }

    IEnumerator ShowStartUpMessages() {
        var activeScene = SceneManager.GetActiveScene().buildIndex;
        if (SceneStartUpMessages.ContainsKey(activeScene)) {
            foreach (var notification in SceneStartUpMessages[activeScene]) {
                UIManager.Notify(notification.type, notification.message, notification.show_at_top);
                while (UIManager.notif_open) {
                    yield return null;
                }
                yield return new WaitForSeconds(0.5f);
            }
            SceneStartUpMessages.Remove(activeScene);
        }
    }

    public void ChangeScene(int buildIndex, bool hidden) {
        transition_status = 1;
        StartCoroutine(LoadAsyncScene(buildIndex, hidden));
        StartCoroutine(ShowLoading(hidden));
    }

    public static void GoToScene(int buildIndex, bool hidden = false) {
        GameObject.Find("__ScenesManager").GetComponent<ScenesManager>().ChangeScene(buildIndex, hidden);
    }

    //Coroutines
    IEnumerator LoadAsyncScene(int sceneBuildIndex, bool hidden) {
        Activate(!hidden);
        if (!hidden) {
            SceneTransition.SetTrigger("Start");
        }

        yield return new WaitForSeconds(SceneTransitionsTime[ActiveTransitionIndex] / 2);
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneBuildIndex);

        while (!asyncLoad.isDone) {
            yield return null;
        }

        transition_status = 0;
        if (loading_status == 1) {
            yield return new WaitForSeconds(LoadingTransitionsTime[ActiveLoadingTransitionIndex]);
        }
        if (!hidden) {
            SceneTransition.SetTrigger("End");
        }

        var waitTime = SceneTransitionsTime[ActiveTransitionIndex] / 2;
        waitTime += LoadingTransitionsTime[ActiveLoadingTransitionIndex];
        yield return new WaitForSeconds(waitTime);
        Activate(false);
        StartCoroutine(ShowStartUpMessages());
    }

    IEnumerator ShowLoading(bool hidden) {
        yield return new WaitForSeconds(SceneTransitionsTime[ActiveTransitionIndex]);
        loading_status = 1;
        if (transition_status == 1) {
            if (!hidden) {
                LoadingTransition.SetTrigger("Start");
            }

        }

        while (transition_status != 0) {
            yield return null;
        }
        if (!hidden) {
            LoadingTransition.SetTrigger("End");
        }

        loading_status = 0;
    }
}