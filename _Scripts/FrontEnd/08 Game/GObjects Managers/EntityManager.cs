using System;
using System.Collections;
using System.Collections.Generic;
using Illumina;
using UnityEngine;
using UnityEngine.Tilemaps;

public class EntityManager : MonoBehaviour {
    public WorldEntityType EntityType;
    public bool HasOutlineManager = false;
    Outline OutlineCreator;
    Vector3 BasePosition;
    Vector3Int GridPosition;
    void Awake() {

        if (EntityType == WorldEntityType.Base || EntityType == WorldEntityType.Navigator) {
            HasOutlineManager = true;
            var map = GameObject.FindGameObjectWithTag("ScriptsContainer").GetComponent<GameAssetsCollection>().Layers[1];
            var outline = (Tile) GameObject.FindGameObjectWithTag("ScriptsContainer").GetComponent<GameAssetsCollection>().Outline;
            OutlineCreator = new Outline(map, outline);
        }

    }

    void Update() {
        if (Input.GetMouseButtonDown(0) && HasOutlineManager) {
            //ShowOutlines();
        }
    }

    void ShowOutlines() {
        OutlineCreator.GetSurroundingPos(new Vector3Int(5, 5, 0));
        OutlineCreator.FillHexPos(false);
    }

}