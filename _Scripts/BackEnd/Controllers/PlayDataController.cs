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
            //TODO : include saving to firebase

            var data = Data.ToServerData();
            Debug.Log("Before :" + JsonConvert.SerializeObject(data));
            PlayData _data = data.ToPlayData();
            data = _data.ToServerData();
            Debug.Log("After :" + JsonConvert.SerializeObject(data));
        }

        public static void OnVerifyRequestSuccess(Exception e) {
            Debug.Log(e.Message);
        }

        public static PlayData LoadSettingsData() {
            playDataSerializer.Initialize();
            Data = (PlayData) playDataSerializer.LoadData(Data);
            GameData.PlayData = Data;
            return Data;
            //TODO: Include loading data from firebase
        }
    }
}