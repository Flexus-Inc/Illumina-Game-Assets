using System;
using System.Collections;
using System.Collections.Generic;
using Illumina;
using UnityEngine;
using UnityEngine.Tilemaps;

public class BaseEntityManager : MonoBehaviour {

    public static Base clickedBase;
    public Tile Blanktile;
    Outline OutlineCreator;
    World world;
    public Vector3 BasePosition;
    public Vector3Int GridPosition;
    List<Vector3Int> OutlinePositions;
    public Tribe TribeIdentity;
    void Awake() {
        var map = GameObject.FindGameObjectWithTag("ScriptsContainer").GetComponent<GameAssetsCollection>().Layers[1];
        var outline = (Tile) GameObject.FindGameObjectWithTag("ScriptsContainer").GetComponent<GameAssetsCollection>().Outline;
        OutlineCreator = new Outline(map, outline);
        OutlinePositions = new List<Vector3Int>();
        world = GameObject.FindGameObjectWithTag("ScriptsContainer").GetComponent<GamePlayManager>().world;
        clickedBase = null;
    }

    void Update() {
        if (Input.GetMouseButtonDown(0) && !GamePlayManager.GamePaused && GamePlayManager.MovementEnabled && (this.gameObject.GetComponent<BaseEntityManager>().TribeIdentity == (Tribe) GamePlayManager.PlayerTurn)) {
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 mousePos2d = new Vector2(mousePos.x, mousePos.y);
            RaycastHit2D hit = Physics2D.Raycast(mousePos2d, Vector2.zero);
            GameObject selectedObj = null;
            if (hit.collider != null) {
                selectedObj = hit.collider.gameObject;
                selectedObj.GetComponent<BaseEntityManager>().ShowOutlines();
            } else {
                ShowOutlines(false);
                var gridpos = OutlineCreator.OutlineMap.WorldToCell(mousePos);
                var eq = true;
                if (clickedBase != null) {
                    var pos = IlluminaConverter.ToCoordInt(this.GridPosition);
                    pos = IlluminaConverter.FlapTopSwitch(pos);
                    eq = clickedBase.GridPosition == pos;
                }
                if (OutlinePositions.Contains(gridpos) && eq) {

                    world.AddNavigator(gridpos, clickedBase.owner, GridPosition);
                }
            }
        }
    }

    public void ShowOutlines(bool show = true) {
        if (!show) {
            OutlineCreator.Tile = Blanktile;
        } else {
            var pos = IlluminaConverter.ToCoordInt(GridPosition);
            pos = IlluminaConverter.FlapTopSwitch(pos);
            clickedBase = world.Map.Maps.BasesMap[pos];
        }
        var outlines = OutlineCreator.GetSurroundingPos(IlluminaConverter.ToFlatTopPos(this.GridPosition));

        for (int i = 0; i < outlines.Length; i++) {
            var _pos = IlluminaConverter.ToCoordInt(outlines[i]);
            var available = true;
            if (show) {
                available = clickedBase.owner.navigators.Count < 3;
                if (((int) clickedBase.owner.tribe) != GamePlayManager.PlayerTurn) {
                    continue;
                }
            }
            if ((!world.Map.Maps.GeneralsMap.ContainsKey(_pos) && !world.Map.Maps.NavigatorsMap.ContainsKey(_pos)) && i != 3 && i != 0 && available) {
                OutlinePositions.Add(outlines[i]);
                OutlineCreator.SetTile(outlines[i]);
            }
        }
        OutlineCreator.Tile = (Tile) GameObject.FindGameObjectWithTag("ScriptsContainer").GetComponent<GameAssetsCollection>().Outline;
    }

    public void SummonNavigator() {

    }

}