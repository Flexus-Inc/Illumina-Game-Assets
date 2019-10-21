using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LobbyButtonController : MonoBehaviour {
    public static int ChosenTribe = -1;
    public GameObject[] tribepanel;
    int tribeindex;

    public void OnMouseOver(int tribeindex) {
        tribepanel[tribeindex].SetActive(true);
    }
    public void OnMouseExit(int tribeindex) {
        tribepanel[tribeindex].SetActive(false);
        if (ChosenTribe >= 0) {
            tribepanel[ChosenTribe].SetActive(true);
        }
    }
}