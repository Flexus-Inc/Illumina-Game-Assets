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
            // RestClient.Put("https://illumina-6a2f2.firebaseio.com/playdatas/" + data.play_key + ".json", data);
        }

        public static void OnVerifyRequestSuccess(Exception e) {
            Debug.Log(e.Message);
        }

        public static PlayData LoadPlayData() {
            playDataSerializer.Initialize();
            Data = (PlayData) playDataSerializer.LoadData(Data);
            GameData.PlayData = Data;
            GameData.PlayDataLoaded = true;

            // RestClient.Get<GamePlayData>("https://illumina-6a2f2.firebaseio.com/playdatas/bwbGnZe.json").Then(res => {
            //     Data = res.ToPlayData();
            //     Data.old = true;
            //     GameData.PlayData = Data;
            //     GameData.PlayDataLoaded = true;
            //     Debug.Log("data loaded");
            //     Debug.Log(res.players_username[0]);

            // }).Catch(err => { Data.old = false; Debug.Log(err); });
            return Data;
            //TODO: Include loading data from firebase
        }
    }
}