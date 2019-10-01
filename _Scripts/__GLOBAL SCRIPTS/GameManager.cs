using System.Collections;
using System.Collections.Generic;
using Illumina;
using UnityEngine;
using UnityEngine.Tilemaps;

public class GameManager : MonoBehaviour {
    public Tilemap map;
    public TileBase tile;
    void Start() {
        var outline = new Outline(map, tile);
        outline.GetSurroundingPos(new Vector3Int(5, 15, 0));
        outline.FillHexPos();

    }
    // Update is called once per frame
    void Update() {

    }
}