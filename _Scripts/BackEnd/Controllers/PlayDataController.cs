using System;
using System.IO;
using Illumina.Models;
using Illumina.Networking;
using Illumina.Serialization;
using Newtonsoft.Json;
using Proyecto26;
using UnityEngine;
using UnityEngine.Networking;

namespace Illumina.Controller {
    public class PlayDataController : Controller {
        static Serializer playDataSerializer;
        public static PlayData Data;

        public static void CreateSerializer() {
            Data = new PlayData();
            playDataSerializer = new Serializer() {
                saveFileName = "play_data.save",
                dataObject = Data
            };
        }

        public static void SavePlayData() {
            playDataSerializer.Initialize();
            Data.old = true;
            playDataSerializer.SaveData(Data);
            Debug.Log(GameData.PlayRoom.data.play_key);
            Request saveRequest = new Request {
                uri = NetworkManager.Laravel_Uri + "/play/savedata",
                body = GameData.PlayRoom
            };
            saveRequest.RequestSuccessEvents += OnSaveDataRequestSuccess;
            saveRequest.RequestFailedEvents += OnSaveDataRequestFailed;
            Store<PlayRoom>(saveRequest);
        }

        public static void OnSaveDataRequestSuccess(object source) {
            var room = (PlayRoom) source;
            GameData.PlayDataLoaded = true;
            GameData.PlayRoom = room;
            if (room.data == null) {
                Debug.Log("data is null");
            } else {
                GameData.PlayData = room.data.ToPlayData();
            }
            ScenesManager.GoToScene(9);
        }

        public static void OnSaveDataRequestFailed(Exception err) {
            Debug.Log(err);
            UIManager.Notify(Notification.Danger, "Problem occured. cannot save data to the server");
            ScenesManager.GoToScene(4);
        }

        public static PlayData LoadPlayData() {
            playDataSerializer.Initialize();
            Data = (PlayData) playDataSerializer.LoadData(Data);
            GameData.PlayData = Data;
            GameData.PlayDataLoaded = true;
            Request loadRequest = new Request {
                uri = NetworkManager.Laravel_Uri + "/play/loaddata",
                body = GameData.PlayRoom
            };
            loadRequest.RequestSuccessEvents += OnSaveDataRequestSuccess;
            loadRequest.RequestFailedEvents += OnLoadDataRequestFailed;
            Store<PlayRoom>(loadRequest);
            return Data;
        }

        public static void OnLoadDataRequestFailed(Exception err) {
            Debug.Log(err);
            UIManager.Notify(Notification.Danger, "Problem occured. cannot load data from the server");
        }
    }
}