using System.Collections;
using System.Collections.Generic;
using Illumina.Controller;
using Illumina.Serialization;
using UnityEngine;

public class GameManager : MonoBehaviour {


    void Awake() {
        CreateSerializers();
    }

    void CreateSerializers() {
        Serializer.SavePathDirectory = Application.persistentDataPath + "/saves/";
        SettingsController.CreateSerializers();
    }

    // Update is called once per frame
    void Update() {

    }
}