using System.Collections;
using System.Collections.Generic;
using Illumina;
using Illumina.Controller;
using Illumina.Models;
using UnityEngine;

public class GamePlayManager : MonoBehaviour {

    public World world;
    public static int PlayerTurn = 0;
    public int TurnNumber = 1;
    public int PlayersTurnNumber = 1;

    void OnEnable() {
        PlayDataController.LoadSettingsData();
    }
    void Start() {
        CreateGameWorld();
    }

    void CreateGameWorld() {
        var collections = this.gameObject.GetComponent<GameAssetsCollection>();
        world = new World(collections.ToWorldCollection());
        // if (GameData.PlayData.old) {
        //     world.Map = GameData.PlayData.worldMap;
        // }
        if (GameData.PlayData.old) {
            world.CreateNew();
            var data = new PlayData();
            data.worldMap = this.world.Map;
            PlayDataController.Data = data;
            PlayDataController.SavePlayData();
        }
        world.Render();
    }

    // Update is called once per frame
    IEnumerator PlaceBase() {
        world.Test();
        yield return null;

    }
}