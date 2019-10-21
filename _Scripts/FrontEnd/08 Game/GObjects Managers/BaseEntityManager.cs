using System;
using System.Collections;
using System.Collections.Generic;
using Illumina;
using UnityEngine;
using UnityEngine.Tilemaps;

public class BaseEntityManager : MonoBehaviour {

    public static Base clickedBase;
    public Tile Blank;
    public static Tile Blanktile;
    static Outline OutlineCreator;
    World world;
    public Vector3 BasePosition;
    public Vector3Int GridPosition;
    static List<Vector3Int> OutlinePositions;
    public Tribe TribeIdentity;
    public string key;
    void Awake() {
        var map = GameObject.FindGameObjectWithTag("ScriptsContainer").GetComponent<GameAssetsCollection>().Layers[1];
        var outline = (Tile) GameObject.FindGameObjectWithTag("ScriptsContainer").GetComponent<GameAssetsCollection>().Outline;
        OutlineCreator = new Outline(map, outline);
        OutlinePositions = new List<Vector3Int>();
        world = GameObject.FindGameObjectWithTag("ScriptsContainer").GetComponent<GamePlayManager>().world;
        clickedBase = null;
        Blanktile = Blank;
    }
    private void OnMouseDown() {
        BaseClick();
    }

    void BaseClick() {
        if (!GamePlayManager.GamePaused && GamePlayManager.MovementEnabled && (this.gameObject.GetComponent<BaseEntityManager>().TribeIdentity == (Tribe) GamePlayManager.PlayerTurn) && GamePlayManager.Navigators.Count < 3) {
            var selectedObj = this.gameObject;
            selectedObj.GetComponent<BaseEntityManager>().ShowOutlines(selectedObj.GetComponent<BaseEntityManager>().GridPosition);
            OutlineManager.ShowOutlines(selectedObj.GetComponent<BaseEntityManager>());
            Debug.Log("Base clicked");
        }
    }

    public void ShowOutlines(Vector3Int pos) {

        var outlines = OutlineCreator.GetSurroundingPos(IlluminaConverter.ToFlatTopPos(pos));
        OutlineManager.ClearNavigatorOutlines();
        for (int i = 0; i < outlines.Length; i++) {
            var _pos = IlluminaConverter.ToCoordInt(outlines[i]);

            // if (show) {
            //     available = clickedBase.owner.navigators.Count < 3;
            //     if (((int) clickedBase.owner.tribe) != GamePlayManager.PlayerTurn) {
            //         continue;
            //     }
            // }
            if ((!world.Map.Maps.GeneralsMap.ContainsKey(_pos) && !world.Map.Maps.NavigatorsMap.ContainsKey(_pos)) && i != 3 && i != 0) {
                if (!OutlineManager.Outlines.ContainsKey(outlines[i])) {
                    OutlineManager.Outlines.Add(outlines[i], this.gameObject);
                }
            }
        }
        OutlineCreator.Tile = (Tile) GameObject.FindGameObjectWithTag("ScriptsContainer").GetComponent<GameAssetsCollection>().Outline;
    }

    public void SummonNavigator() {

    }

}