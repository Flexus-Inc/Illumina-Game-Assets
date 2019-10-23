using System;
using System.Collections;
using System.Collections.Generic;
using Illumina;
using Illumina.Controller;
using Illumina.Models;
using UnityEngine;
using UnityEngine.UI;

public class LobbyView : MonoBehaviour {

    public GameObject[] UserContainers;
    public Button ReadyButton;
    public static LobbyRoom stagingLobby;
    public static LobbyRoom lobby;
    public static List<Player> players = new List<Player>();
    public static bool registrationDone = false;
    public static bool playersUpdating = false;
    public static bool playersChecking = false;
    public static bool decreasedCount = false;
    public static bool creatingPlayRoom = false;
    void Start() {
        LobbyController.Register(GameData.User);
        StartCoroutine(DisplayPlayers());
        StartCoroutine(StatusCheck());
        StartCoroutine(UpdatePlayers());
        //TODO: remove the comment above in release
    }

    public void Ready() {
        ReadyButton.interactable = false;
        ReadyButton.gameObject.SetActive(false);
        LobbyController.Ready(lobby, OnReadyRequestSuccess, OnReadyRequestFailed);
    }

    public void OnReadyRequestSuccess(object source) {
        var _lobby = (LobbyRoom) source;
        stagingLobby = _lobby;
        if (_lobby.readyplayers == 4 && !creatingPlayRoom) {
            var ishost = lobby.host == GameData.User.username;
            UIManager.DisplayLoading();
            creatingPlayRoom = true;
            if (ishost) {
                var room = CreatePlayRoom();
                LobbyController.CreatePlayRoom(room);
            } else {
                LobbyController.WaitPlayRoom(_lobby);
            }

        }
    }

    PlayRoom CreatePlayRoom() {
        var room = new PlayRoom();
        var players = new List<Player>();
        foreach (var item in lobby.users) {
            var ishost = lobby.host == item.username;
            if (ishost) {
                room.host = item.username;
            }
            var player = new Player(item, ishost);
            players.Add(player);
        }
        room.players = players.ToArray();
        return room;
    }

    public void LeaveRoom() {
        StartCoroutine(Leave());
    }

    IEnumerator Leave() {
        while (playersUpdating) {
            yield return null;
        }
        UIManager.DisplayLoading();
        LobbyController.LeaveRoom(lobby, GameData.User);
    }

    public void OnReadyRequestFailed(Exception err) {
        Debug.Log(err);
        UIManager.Danger("Problem occured. you will be redirected to Main Menu.");
        while (UIManager.popup_open) {
            //do nothing
        }
    }

    IEnumerator DisplayPlayers() {
        while (!registrationDone) {
            yield return null;
        }
        for (int i = 0; i < lobby.users.Length; i++) {
            UserContainers[i].transform.GetChild(1).GetComponent<Text>().text = lobby.users[i].name;
            UserContainers[i].transform.GetChild(2).GetComponent<Text>().text = "USER: " + lobby.users[i].username;
            UserContainers[i].transform.GetChild(3).GetComponent<Image>().sprite = GameDataManager.GetProfileAvatar(lobby.users[i].profile);
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
            var kickout = true;
            for (int i = 0; i < stagingLobby.users.Length; i++) {
                if (stagingLobby.users[i].username == GameData.User.username) {
                    kickout = false;
                }
            }
            if (kickout) {
                ScenesManager.GoToScene(3);
            }
            for (int i = 0; i < lobby.users.Length; i++) {
                if (stagingLobby.users.Length < lobby.users.Length && i == (lobby.users.Length - 1)) {
                    UserContainers[i].GetComponent<Animator>().SetBool("Active", false);
                    break;
                }
                if (UserContainers[i].transform.GetChild(1).GetComponent<Text>().text != stagingLobby.users[i].name) {
                    UserContainers[i].GetComponent<Animator>().SetBool("Active", false);
                    yield return new WaitForSeconds(0.75f);
                }
                UserContainers[i].transform.GetChild(1).GetComponent<Text>().text = stagingLobby.users[i].name;
                UserContainers[i].transform.GetChild(2).GetComponent<Text>().text = "USER: " + lobby.users[i].username;
                UserContainers[i].transform.GetChild(3).GetComponent<Image>().sprite = GameDataManager.GetProfileAvatar(lobby.users[i].profile);
                UserContainers[i].GetComponent<Animator>().SetBool("Active", true);
                yield return new WaitForSeconds(0.75f);
            }
            lobby = stagingLobby;

            if (lobby.users.Length < 4) {
                ReadyButton.gameObject.SetActive(false);
                ReadyButton.interactable = false;
            }
            if (lobby.readyplayers == 0 && stagingLobby.users.Length == 4) {
                ReadyButton.gameObject.SetActive(true);
                ReadyButton.interactable = true;
            }
            if (lobby.readyplayers == 4) {
                OnReadyRequestSuccess(lobby);
                break;
            }
            Debug.Log(lobby.readyplayers);
            yield return new WaitForSeconds(0.25f);
        }
    }

    IEnumerator StatusCheck() {
        while (true) {
            while (!registrationDone) {
                yield return null;
            }

            playersChecking = true;
            LobbyController.StatusCheck(GameData.User, lobby);
            while (playersChecking) {
                yield return null;
            }
            yield return new WaitForSeconds(1);
        }
    }

}