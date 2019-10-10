using System.Collections;
using System.Collections.Generic;
using Illumina;
using UnityEngine;
using UnityEngine.Tilemaps;

public class NavigatorEntityManager : MonoBehaviour {

    public static Navigator clickedNavigator;

    public Tile Blanktile;
    Outline OutlineCreator;
    World world;

    public Vector3 BasePosition;
    public Vector3Int GridPosition;
    List<Vector3Int> OutlinePositions;
    // Start is called before the first frame update
    void Awake() {
        var map = GameObject.FindGameObjectWithTag("ScriptsContainer").GetComponent<GameAssetsCollection>().Layers[1];
        var outline = (Tile) GameObject.FindGameObjectWithTag("ScriptsContainer").GetComponent<GameAssetsCollection>().Outline;
        OutlineCreator = new Outline(map, outline);
        OutlinePositions = new List<Vector3Int>();
        world = GameObject.FindGameObjectWithTag("ScriptsContainer").GetComponent<GamePlayManager>().world;
        clickedNavigator = null;
    }

    // Update is called once per frame
    void Update() {
        if (Input.GetMouseButtonDown(0) && !GamePlayManager.GamePaused && GamePlayManager.MovementEnabled) {
            Debug.Log("cliked navigator");
        }
    }
}