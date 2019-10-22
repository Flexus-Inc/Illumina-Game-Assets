using System;
using Illumina.Models;
using Illumina.Networking;
using Illumina.Serialization;
using UnityEngine;
using static Illumina.Networking.Request;

namespace Illumina.Controller {

    public class LobbyController : Controller {
        public static void Register(User user) {
            LobbyRegistrant _register = new LobbyRegistrant();
            _register.username = user.username;
            Request registerRequest = new Request {
                uri = NetworkManager.Laravel_Uri + "/lobby/register",
                body = _register
            };
            registerRequest.RequestSuccessEvents += OnRegisterRequestSuccess;
            registerRequest.RequestFailedEvents += OnRegisterRequestFailed;
            Store<LobbyRoom>(registerRequest);
        }

        public static void OnRegisterRequestSuccess(object source) {
            var lobby = (LobbyRoom) source;
            LobbyView.lobby = lobby;
            LobbyView.registrationDone = true;
            if (lobby.response_code == "0") {
                UIManager.Danger("Your username is not valid");
            }
            if (lobby.response_code == "2") {
                UIManager.Notify(Notification.Success, "You created a new lobby");
            }

        }

        public static void OnRegisterRequestFailed(Exception err) {
            Debug.Log(err);
            UIManager.Danger("Problem occured. you will be redirected to Main Menu.");
            while (UIManager.popup_open) {
                //do nothing
            }
            ScenesManager.GoToScene(3);
        }

        public static void Update(LobbyRoom lobby) {
            LobbyRoom room = lobby;
            Request updateRequest = new Request {
                uri = NetworkManager.Laravel_Uri + "/lobby/update",
                body = room
            };
            updateRequest.RequestSuccessEvents += OnUpdateRequestSuccess;
            updateRequest.RequestFailedEvents += OnUpdateRequestFailed;
            Store<LobbyRoom>(updateRequest);
        }

        public static void OnUpdateRequestSuccess(object source) {
            var lobby = (LobbyRoom) source;
            LobbyView.stagingLobby = lobby;
            LobbyView.playersUpdating = false;
            if (lobby.response_code == "4") {
                UIManager.Notify(Notification.Light, "Someone left the lobby", false);
            }
        }

        public static void OnUpdateRequestFailed(Exception err) {
            Debug.Log(err);
            UIManager.Danger("Problem occured. you will be redirected to Main Menu.");
            while (UIManager.popup_open) {
                //do nothing
            }
        }

        public static void StatusCheck(User user, LobbyRoom lobby) {
            LobbyRegistree registree = new LobbyRegistree {
                username = user.username,
                hostid = lobby.hostid
            };
            Request checkRequest = new Request {
                uri = NetworkManager.Laravel_Uri + "/lobby/statuscheck",
                body = registree
            };
            checkRequest.RequestSuccessEvents += OnCheckRequestSuccess;
            checkRequest.RequestFailedEvents += OnCheckRequestFailed;
            Update<LobbyRegistree>(checkRequest);
        }

        public static void OnCheckRequestSuccess(object source) {
            var lobby = (LobbyRegistree) source;
            Debug.Log("Checked status of " + lobby.username);
            LobbyView.playersChecking = false;
        }

        public static void OnCheckRequestFailed(Exception err) {
            Debug.Log(err);
            UIManager.Danger("Problem occured. you will be redirected to Main Menu.");
            while (UIManager.popup_open) {
                //do nothing
            }
        }

        public static void Ready(LobbyRoom lobby, RequestSuccessEventHandler e, RequestFailedEventHandler f) {
            LobbyRoom room = lobby;
            Request updateRequest = new Request {
                uri = NetworkManager.Laravel_Uri + "/lobby/ready",
                body = room
            };
            updateRequest.RequestSuccessEvents += e;
            updateRequest.RequestFailedEvents += f;
            Update<LobbyRoom>(updateRequest);
        }

        public static void CreatePlayRoom(PlayRoom room) {
            Request createRequest = new Request {
                uri = NetworkManager.Laravel_Uri + "/lobby/createplay",
                body = room
            };
            createRequest.RequestSuccessEvents += OnCreatePlayRoomRequestSuccess;
            createRequest.RequestFailedEvents += OnCreatePlayRoomRequestFailed;
            Store<PlayRoom>(createRequest);
        }
        public static void WaitPlayRoom(LobbyRoom lobby) {
            Request createRequest = new Request {
                uri = NetworkManager.Laravel_Uri + "/lobby/waitplay",
                body = lobby
            };
            createRequest.RequestSuccessEvents += OnCreatePlayRoomRequestSuccess;
            createRequest.RequestFailedEvents += OnCreatePlayRoomRequestFailed;
            Store<PlayRoom>(createRequest);
        }

        public static void OnCreatePlayRoomRequestSuccess(object source) {
            Debug.Log("created");
            var room = (PlayRoom) source;
            Debug.Log("Play room created ,hosting " + room.hostid);
            GameData.PlayRoom = room;
            GameData.PlayData = room.data.ToPlayData();
            UIManager.Notify(Notification.Info, "Battlefield is now ready");
            UIManager.HideLoading();
            ScenesManager.GoToScene(8);
            UIManager.popup_open = false;
        }

        public static void OnCreatePlayRoomRequestFailed(Exception err) {
            Debug.Log(err);
            UIManager.Danger("Problem occured. you will be redirected to Main Menu.");
            LobbyView.creatingPlayRoom = false;
            while (UIManager.popup_open) {
                //do nothing
            }
            ScenesManager.GoToScene(3);
        }
    }
}