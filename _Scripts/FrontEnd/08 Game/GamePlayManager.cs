using System.Collections;
using System.Collections.Generic;
using Illumina;
using Illumina.Controller;
using Illumina.Models;
using UnityEngine;

public class GamePlayManager : MonoBehaviour {

    public World world;
    public GameObject[] NavigatorButtons = new GameObject[3];
    public static GameObject[] NavButtons;
    public static List<GameObject> Navigators = new List<GameObject>();
    public static List<string> NavigatorKeys = new List<string>();
    public static int PlayerTurn = 0;
    public static bool GamePaused = true;
    public static bool MovementEnabled = true;
    public static int TurnMoves = 0;
    public static int TurnMaxMoves = 0;

    void OnEnable() {
        GameData.PlayDataLoaded = false;
        PlayDataController.LoadPlayData();

    }
    void Awake() {
        foreach (var item in NavigatorButtons) {
            item.SetActive(false);
        }
        StartCoroutine(LoadGame());
    }

    IEnumerator LoadGame() {
        while (!GameData.PlayDataLoaded) {
            yield return null;
            Debug.Log("Loading...");
        }
        if (GameData.PlayDataLoaded) {
            CreateGameWorld();
            StartCoroutine(SetTimer(45));
        }

    }

    public void EnableNavigatorButton(int index) {
        NavigatorButtons[index].SetActive(true);
        Debug.Log("activated");
    }

    public static void EnableNavButton(int index) {
        GameObject.FindGameObjectWithTag("ScriptsContainer").
        GetComponent<GamePlayManager>().EnableNavigatorButton(index);
    }

    public void GoToNavigatorPosition(int index) {
        var pos = Navigators[index].GetComponent<Transform>().localPosition;
        var newpos = new Vector3(pos.x, pos.y, 0);
        Camera.main.transform.localPosition = newpos;
        //make it animated
    }

    void CreateGameWorld() {
        var collections = this.gameObject.GetComponent<GameAssetsCollection>();
        world = new World(collections.ToWorldCollection());
        if (GameData.PlayData.old) {
            world.players = GameData.PlayData.players;
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