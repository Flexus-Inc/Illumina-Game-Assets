using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Threading;
using UnityEngine.SceneManagement;

public class Timer : MonoBehaviour
{
    public Text timertext;
    public float timer = 0.0f;
    private  int time;
    public int sceneIndex;

    void Update(){
        timer -= Time.deltaTime;
        time = (int)(timer % 60);
        timertext.text = time.ToString();
        if(timer < 0){
            SceneManager.LoadScene(sceneIndex);
        }
    }
}
