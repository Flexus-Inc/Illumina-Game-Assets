using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Threading;

public class Timer : MonoBehaviour
{
    public int timer;
    public Text countdownText;

    void Start(){
        StartCoroutine("LoseTime");
        Time.timeScale = 1;
    }

    void Update(){
        countdownText.text = ("" + timer);
    }

    IEnumerator LoseTime(){
        while(timer>0){
            yield return new WaitForSeconds(1);
            timer--;
        }
    }

}
