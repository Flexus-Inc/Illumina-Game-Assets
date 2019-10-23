using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UserTimer : MonoBehaviour
{
    public Image loading;
    public float sec;

    void Start ()
        {
            StartCoroutine (StartTimer());
        }

    IEnumerator StartTimer(){
        float timeStartedLerping = Time.time;
        float timeSinceStarted = Time.time - timeStartedLerping;
        float percentageComplete = timeSinceStarted / sec;

        while (true) {
            timeSinceStarted = Time.time - timeStartedLerping;
            percentageComplete = timeSinceStarted / sec;
            loading.fillAmount = 1 - percentageComplete;
            if (percentageComplete >= 1) break;
            yield return new WaitForEndOfFrame();
        }
    }

}
