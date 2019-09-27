using System.Collections;
using System.Collections.Generic;
using Illumina.Controller;
using Illumina.Serialization;
using UnityEngine;

public class GameData : MonoBehaviour {

    void Awake() {
        CreateSerializers();
        SettingsController.LoadSettingsData();
    }

    void CreateSerializers() {
        Serializer.SavePathDirectory = Application.persistentDataPath + "/saves/";
        SettingsController.CreateSerializers();
    }

    // Update is called once per frame
    void Update() {

    }
}