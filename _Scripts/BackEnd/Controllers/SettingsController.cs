using Illumina.Models;
using Illumina.Serialization;
using UnityEngine;

namespace Illumina.Controller {
    public class SettingsController {
        static Serializer settingsSerializer;
        public static SettingsData Data;

        public static void CreateSerializer() {
            Data = new SettingsData();
            settingsSerializer = new Serializer() {
                saveFileName = "settings_data.save",
                dataObject = Data
            };
        }

        public static void SaveSettingsData() {
            settingsSerializer.Initialize();
            settingsSerializer.SaveData(Data);
        }

        public static SettingsData LoadSettingsData() {
            settingsSerializer.Initialize();
            Data = (SettingsData) settingsSerializer.LoadData(Data);
            return Data;
        }

        public static void UpdateVolume() {
            GameObject.Find("__MusicManager").GetComponent<MusicManager>().source.volume = Data.MusicVolume * Data.MasterVolume;
            GameObject.Find("__SoundManager").GetComponent<SoundManager>().source.volume = Data.SoundFXVolume * Data.MasterVolume;
        }

        public static float GetMusicVolume() {
            return Data.MusicVolume * Data.MasterVolume;
        }
        public static float GetSoundFXVolume() {
            return Data.SoundFXVolume * Data.MasterVolume;
        }
    }
}