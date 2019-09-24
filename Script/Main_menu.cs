using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Main_menu : MonoBehaviour
{
    public AudioSource buttonclick;
    public void Enclasp()
    {
        Debug.Log("Next scene");
        buttonclick.Play();
    }
    public void Enlighten()
    {
        Debug.Log("Next scene");
        buttonclick.Play();
    }
    public void Enchant()
    {
        buttonclick.Play();
        SceneManager.LoadScene(sceneBuildIndex: 2);
    }
    public void editaccount(){
        SceneManager.LoadScene(sceneBuildIndex: 3);
    }

    public GameObject Panel;

    public void OpenPanel(){
        if(Panel != null){
            bool isActive = Panel.activeSelf;
            Panel.SetActive(!isActive);
            Animator animator = Panel.GetComponent<Animator>();
            if(animator !=null){
                bool isOpen = animator.GetBool("open");

                animator.SetBool("open", !isOpen);
            }
        }
    }
}
