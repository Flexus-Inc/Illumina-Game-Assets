using Illumina.Models;
using Illumina.Serialization;

namespace Illumina.Controller {
    public class SettingsController {
        static Serializer settingsSerializer;
        public static SettingsData Data;

        public static void CreateSerializers() {
            Data = new SettingsData();
            settingsSerializer = new Serializer() {
                saveFileName = "settings_data.save",
                dataObject = Data
            };
        }

        public static void SaveSettingsData(SettingsData data) {
            settingsSerializer.Initialize();
            Data = data;
            settingsSerializer.SaveData(Data);
        }

        public static SettingsData LoadSettingsData() {
            settingsSerializer.Initialize();
            Data = (SettingsData) settingsSerializer.LoadData(Data);
            return Data;
        }
    }
}