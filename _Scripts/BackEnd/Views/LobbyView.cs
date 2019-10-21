﻿using System;
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
        if (_lobby.readyplayers == 4) {
            var room = CreatePlayRoom();
            UIManager.DisplayLoading();
            LobbyController.CreatePlayRoom(room);
        }
    }

    PlayRoom CreatePlayRoom() {
        var room = new PlayRoom();
        var players = new List<Player>();
        foreach (var item in lobby.users) {
            var ishost = lobby.host == GameData.User.username;
            var player = new Player(item, ishost);
            players.Add(player);
        }
        room.players = players.ToArray();
        return room;
    }

    public void OnReadyRequestFailed(Exception err) {
        Debug.Log(err);
        UIManager.Danger(err.Message);
        ReadyButton.gameObject.SetActive(true);
        ReadyButton.interactable = true;
    }

    IEnumerator DisplayPlayers() {
        while (!registrationDone) {
            yield return null;
        }
        for (int i = 0; i < lobby.users.Length; i++) {
            UserContainers[i].transform.GetChild(1).GetComponent<Text>().text = lobby.users[i].name;
            UserContainers[i].transform.GetChild(2).GetComponent<Text>().text = "UN: " + lobby.users[i].username;
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
            if (stagingLobby.users.Length < 4) {
                ReadyButton.gameObject.SetActive(false);
                ReadyButton.interactable = false;
            }
            if (stagingLobby.readyplayers == 0 && stagingLobby.users.Length == 4) {
                ReadyButton.gameObject.SetActive(true);
                ReadyButton.interactable = true;
            }
            for (int i = 0; i < lobby.users.Length; i++) {
                if (stagingLobby.users.Length < lobby.users.Length && i == (lobby.users.Length - 1)) {
                    UserContainers[i].GetComponent<Animator>().SetBool("Active", false);
                    break;
                }
                if (UserContainers[i].transform.GetChild(1).GetComponent<Text>().text != stagingLobby.users[i].username) {
                    UserContainers[i].GetComponent<Animator>().SetBool("Active", false);
                    yield return new WaitForSeconds(0.75f);
                }
                UserContainers[i].transform.GetChild(1).GetComponent<Text>().text = stagingLobby.users[i].username;
                UserContainers[i].transform.GetChild(2).GetComponent<Text>().text = stagingLobby.users[i].email;
                UserContainers[i].GetComponent<Animator>().SetBool("Active", true);
                yield return new WaitForSeconds(0.25f);
            }
            lobby = stagingLobby;
            Debug.Log(lobby.readyplayers);
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