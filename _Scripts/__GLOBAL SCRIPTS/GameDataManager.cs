using System.Collections;
using System.Collections.Generic;
using Illumina.Controller;
using Illumina.Serialization;
using UnityEngine;

public class GameDataManager : MonoBehaviour {
    // Start is called before the first frame update
    public Sprite[] ProfileAvatars;
    void Awake() {
        CreateSerializers();
    }

    void OnEnable() {
        SettingsController.LoadSettingsData();
    }

    void CreateSerializers() {
        Serializer.SavePathDirectory = Application.persistentDataPath + "/saves/";
        SettingsController.CreateSerializer();
        PlayDataController.CreateSerializer();
    }

    public Sprite GetAvatar(int profile) {
        return ProfileAvatars[profile];
    }

    public static Sprite GetProfileAvatar() {
        return GameObject.Find("__GameDataManager").GetComponent<GameDataManager>().GetAvatar(GameData.User.profile);
    }
}

//-63