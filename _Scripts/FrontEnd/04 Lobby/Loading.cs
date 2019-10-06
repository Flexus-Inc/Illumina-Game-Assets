﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Loading : MonoBehaviour
{
    public GameObject LoadingScreen;
    public Text loadingtext;
    public void LoadScene(int sceneIndex){
        StartCoroutine(LoadAsynchronously(sceneIndex));
    }
    IEnumerator LoadAsynchronously(int sceneIndex){
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneIndex);
        LoadingScreen.SetActive(true);
    while(!operation.isDone)
        {
            float progress = Mathf.Clamp01(operation.progress/.9f);
            loadingtext.text = progress * 100f +"%";
            yield return null;
        }    
    }
}
