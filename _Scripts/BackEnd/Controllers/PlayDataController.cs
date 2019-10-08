using Illumina.Models;
using Illumina.Serialization;
using UnityEngine;

namespace Illumina.Controller {
    public class PlayDataController {
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