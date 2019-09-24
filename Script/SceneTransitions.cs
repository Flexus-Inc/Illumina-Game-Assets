using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneTransitions : MonoBehaviour
{
    public int index;
    public Image black;
    public Animator animator;

    public void buttonclicked(){
        StartCoroutine(fade());
    }

    IEnumerator fade(){
        animator.SetBool("fade",true);
        yield return new WaitUntil(()=>black.color.a==1);
        SceneManager.LoadScene(index);
    }
}
