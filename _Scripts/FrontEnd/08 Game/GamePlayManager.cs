using System.Collections;
using System.Collections.Generic;
using Illumina;
using Illumina.Controller;
using Illumina.Models;
using UnityEngine;

public class GamePlayManager : MonoBehaviour {

    public World world;
    public static int PlayerTurn = 0;
    public static bool GamePaused = true;
    public static bool MovementEnabled = true;
    public static int TurnMoves = 0;
    public static int TurnMaxMoves = 0;

    void OnEnable() {
        PlayDataController.LoadSettingsData();
    }
    void Start() {
        CreateGameWorld();
        StartCoroutine(SetTimer(45));
    }

    void CreateGameWorld() {
        var collections = this.gameObject.GetComponent<GameAssetsCollection>();
        world = new World(collections.ToWorldCollection());
        if (GameData.PlayData.old) {
            world.Map = GameData.PlayData.worldMap;
        }
        if (!GameData.PlayData.old) {
            world.CreateNew();
            var data = new PlayData();
            data.worldMap = this.world.Map;
            data.players = this.world.players;
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

    public void PauseGame(bool paused = true) {
        GamePaused = paused;
    }

    IEnumerator SetTimer(float duration) {
        var time = duration;
        while (time > 0) {
            if (!GamePaused) {
                yield return new WaitForSeconds(1);
                time--;
                GameUIManager.TimerText = (int) time;
            } else {
                yield return null;
            }

        }
    }
}