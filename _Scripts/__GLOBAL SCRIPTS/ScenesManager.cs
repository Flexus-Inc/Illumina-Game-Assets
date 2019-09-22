using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum Transition {
    Fade = 0
}
public class ScenesManager : MonoBehaviour {
    // Start is called before the first frame update

    public Animator[] transitions;
    public float[] TotalTransitionTime;
    public Animator[] LoadingTransitions;
    public float[] LoadingTransitionTime;
    public static Transition transition = Transition.Fade;
    public static int loadingtransition = 0;
    static Animator nextTransition;
    static Animator loadingTransition;
    public int transition_status = 0;
    public int loading_status = 0;

    void Awake() {
        foreach (var transition in LoadingTransitions) {
            transition.gameObject.SetActive(false);
        }
        foreach (var transition in transitions) {
            transition.gameObject.SetActive(false);
        }
    }

    void Start() {
        nextTransition = transitions[(int) transition];
        loadingTransition = LoadingTransitions[loadingtransition];
    }

    // Update is called once per frame
    void Update() {
        if (Input.GetKeyDown(KeyCode.Space)) {
            UIManager.EnableGUI();
            nextTransition.gameObject.SetActive(true);
            loadingTransition.gameObject.SetActive(true);
            nextTransition.SetTrigger("Start");
            transition_status = 1;
            StartCoroutine(LoadAsyncScene(1));
            StartCoroutine(ShowLoading());
        }
    }

    IEnumerator LoadAsyncScene(int sceneBuildIndex) {

        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneBuildIndex);

        while (!asyncLoad.isDone) {
            yield return null;
        }
        transition_status = 0;
        if (loading_status == 1) {
            yield return new WaitForSeconds(LoadingTransitionTime[loadingtransition]);
        }
        nextTransition.SetTrigger("End");
        yield return new WaitForSeconds(TotalTransitionTime[(int) transition] / 2);
        nextTransition.gameObject.SetActive(false);
        UIManager.DisableGUI();
    }

    IEnumerator ShowLoading() {
        yield return new WaitForSeconds(TotalTransitionTime[(int) transition]);
        loading_status = 1;
        if (transition_status == 1) {
            loadingTransition.SetTrigger("Start");
        }
        while (transition_status != 0) {
            yield return null;
        }
        loadingTransition.SetTrigger("End");
        loading_status = 0;
        yield return new WaitForSeconds(LoadingTransitionTime[loadingtransition]);
        loadingTransition.gameObject.SetActive(false);
    }
}