using System.Collections;
using System.Collections.Generic;
using Illumina.Controller;
using Illumina.Models;
using UnityEngine;
using UnityEngine.UI;

public class LobbyView : MonoBehaviour {

    public GameObject[] UserContainers;
    public static LobbyRoom lobby;
    public static bool registrationDone = false;
    public static bool playersUpdating = false;
    void Start() {
        LobbyController.Register(GameData.User);
        StartCoroutine(DisplayPlayers());
        StartCoroutine(UpdatePlayers());
    }

    IEnumerator DisplayPlayers() {
        while (!registrationDone) {
            yield return null;
        }
        for (int i = 0; i < lobby.users.Length; i++) {
            UserContainers[i].transform.GetChild(1).GetComponent<Text>().text = lobby.users[i].username;
            UserContainers[i].transform.GetChild(2).GetComponent<Text>().text = lobby.users[i].email;
            UserContainers[i].GetComponent<Animator>().SetBool("Active", true);
            yield return new WaitForSeconds(0.5f);
        }
    }

    IEnumerator UpdatePlayers() {
        while (true) {
            while (!registrationDone) {
                yield return null;
            }
            playersUpdating = true;
            LobbyController.Update(lobby);
            while (playersUpdating) {
                yield return null;
            }

            for (int i = 0; i < lobby.users.Length; i++) {
                UserContainers[i].transform.GetChild(1).GetComponent<Text>().text = lobby.users[i].username;
                UserContainers[i].transform.GetChild(2).GetComponent<Text>().text = lobby.users[i].email;
                UserContainers[i].GetComponent<Animator>().SetBool("Active", true);
                yield return new WaitForSeconds(0.25f);
            }

            if (lobby.users.Length == 4) {
                break;
            }
        }
    }

}