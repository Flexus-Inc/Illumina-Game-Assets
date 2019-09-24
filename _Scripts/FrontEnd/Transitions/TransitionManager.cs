using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TransitionManager : MonoBehaviour {

    public Animator transitioner;
    public Animator loading_transitioner;

    void Awake() {
        transitioner.gameObject.SetActive(true);
        loading_transitioner.gameObject.SetActive(true);
        StartCoroutine(FadeOut());
    }

    IEnumerator FadeOut() {
        if (ScenesManager.loading_status == 1) {
            loading_transitioner.SetTrigger("Start");
            yield return new WaitForSeconds(0.5f);
            ScenesManager.loading_status = 0;
        }
        transitioner.SetTrigger("Start");
        yield return new WaitForSeconds(0.5f);
        transitioner.gameObject.SetActive(false);
        loading_transitioner.gameObject.SetActive(false);
        transitioner.gameObject.GetComponent<Image>().color = new Color(0, 0, 0, 0);
    }

    public void GoToScene(int sceneBuildIndex) {
        ScenesManager.transition_status = 1;
        transitioner.gameObject.SetActive(true);
        loading_transitioner.gameObject.SetActive(true);
        StartCoroutine(LoadAsyncScene(sceneBuildIndex));
        StartCoroutine(ShowLoading());
    }

    public static void ChangeSceneTo(int sceneBuildIndex) {
        GameObject.Find("__SceneTransitions").GetComponent<TransitionManager>().GoToScene(sceneBuildIndex);
    }

    IEnumerator LoadAsyncScene(int sceneBuildIndex) {

        transitioner.SetTrigger("End");
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneBuildIndex);
        asyncLoad.allowSceneActivation = false;
        yield return new WaitForSeconds(0.5f);
        asyncLoad.allowSceneActivation = true;
        while (!asyncLoad.isDone) {
            yield return null;
        }

        ScenesManager.transition_status = 0;
    }

    IEnumerator ShowLoading() {
        yield return new WaitForSeconds(0.75f);
        if (ScenesManager.transition_status == 1) {
            ScenesManager.loading_status = 1;
            loading_transitioner.SetTrigger("End");
        }
    }
}