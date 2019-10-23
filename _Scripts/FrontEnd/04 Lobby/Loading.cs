using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Loading : MonoBehaviour {
    public GameObject LoadingPanel;
    public string[] LoadingTexts;
    public Text LoadingTextContainer;

    public void Start() {
        StartCoroutine(DisplayTexts());
    }

    IEnumerator DisplayTexts() {
        foreach (var text in LoadingTexts) {
            LoadingTextContainer.text = text;
            yield return new WaitForSeconds(1.5f);
        }
    }
}