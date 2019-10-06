using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Lobby : MonoBehaviour
{
    public GameObject[] tribepanel;
    int tribeindex;

    public void OnMouseOver(int tribeindex){
        tribepanel[tribeindex].SetActive(true);
    }
    public void OnMouseExit(int tribeindex){
        tribepanel[tribeindex].SetActive(false);
    }
}
