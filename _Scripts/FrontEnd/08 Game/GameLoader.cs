using System.Collections;
using System.Collections.Generic;
using Illumina;
using Illumina.Controller;
using UnityEngine;

public class GameLoader : MonoBehaviour {
    // Start is called before the first frame update
    void OnEnable() {
        GameData.PlayDataLoaded = false;
    }

    // Update is called once per frame
    void Awake() {
        var ishost = GameData.User.username == GameData.PlayRoom.host;
        if (ishost) {
            CreateGameWorld();
        } else {
            LoadGameWorld();
        }
    }

    void CreateGameWorld() {
        var collections = this.gameObject.GetComponent<GameAssetsCollection>();
        var world = new World(collections.ToWorldCollection());
        world.CreateNew();

        GameData.PlayData.worldMap = world.Map;
        GameData.PlayData.key = GameData.PlayRoom.data.play_key;
        PlayDataController.Data = GameData.PlayData;
        PlayDataController.SavePlayData();
    }

    void LoadGameWorld() {
        PlayDataController.LoadPlayData();
    }
}