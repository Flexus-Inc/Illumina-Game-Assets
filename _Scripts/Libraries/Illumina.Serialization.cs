/*
    Author:Edrian Jose D.G. Ferrer
    Collaborators: None yet
    Version: 1.0
    Owned by: Flexus Group of Companies
 */

using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

namespace Illumina.Serialization {
    public class Serializer {
        BinaryFormatter binaryFormatter;
        public static string SavePathDirectory;
        string SaveFileName;
        string SavePath;

        object DataObject;

        public string saveFileName { get => SaveFileName; set => SaveFileName = value; }
        public object dataObject { get => DataObject; set => DataObject = value; }
        public string savePath { get => SavePath; }

        public Serializer() {
            binaryFormatter = new BinaryFormatter();
        }

        public Serializer Initialize() {
            if (!Directory.Exists(SavePathDirectory)) {
                Directory.CreateDirectory(SavePathDirectory);
            }
            SavePath = SavePathDirectory + SaveFileName;
            return this;
        }

        public void SaveData(object data) {
            using(var fileStream = File.Create(SavePath)) {
                binaryFormatter.Serialize(fileStream, data);
            }
        }

        public object LoadData(object NewDataObject) {
            if (File.Exists(SavePath)) {
                using(var fileStream = File.Open(SavePath, FileMode.Open)) {
                    NewDataObject = binaryFormatter.Deserialize(fileStream);
                }
            } else {
                SaveData(NewDataObject);
                Debug.Log("Created new " + savePath + " file");
            }

            return NewDataObject;
        }

    }
}