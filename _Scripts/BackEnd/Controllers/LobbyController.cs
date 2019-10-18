using System;
using Illumina.Models;
using Illumina.Networking;
using Illumina.Serialization;
using UnityEngine;

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
            UIManager.Danger(err.Message);
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
            LobbyView.lobby = lobby;
            LobbyView.playersUpdating = false;
        }

        public static void OnUpdateRequestFailed(Exception err) {
            Debug.Log(err);
            UIManager.Danger(err.Message);
        }
    }
}