using System.Collections;
using System.Collections.Generic;
using Illumina;
using UnityEngine;
using UnityEngine.Tilemaps;

public class OutlineManager : MonoBehaviour {

    public Tile Blanktile;
    public static Dictionary<Vector3Int, GameObject> Outlines = new Dictionary<Vector3Int, GameObject>();
    public static bool BaseOutlinesShown = false;
    public static bool NavigatorOutlinesShown = false;
    static Outline OutlineCreator;
    static Tile BlankTile;

    void Start() {
        Outlines = new Dictionary<Vector3Int, GameObject>();
        var map = this.gameObject.GetComponent<GameAssetsCollection>().Layers[1];
        var outline = (Tile) this.gameObject.GetComponent<GameAssetsCollection>().Outline;
        OutlineCreator = new Outline(map, outline);
        BlankTile = Blanktile;
    }

    // Update is called once per frame
    public static void ClearNavigatorOutlines() {
        OutlineCreator.Tile = BlankTile;
        List<Vector3Int> forRemovals = new List<Vector3Int>();
        foreach (var item in Outlines) {
            var ent = item.Value.GetComponent<NavigatorEntityManager>();
            if (ent != null) {
                OutlineCreator.SetTile(item.Key);
                forRemovals.Add(item.Key);
            }
        }

        foreach (var item in forRemovals) {
            Outlines.Remove(item);
        }
        forRemovals.Clear();
        OutlineCreator.Tile = (Tile) GameObject.FindGameObjectWithTag("ScriptsContainer").GetComponent<GameAssetsCollection>().Outline;
        NavigatorOutlinesShown = false;
    }

    public static void ClearOtherNavigatorOutlines(string key) {
        OutlineCreator.Tile = BlankTile;
        List<Vector3Int> forRemovals = new List<Vector3Int>();
        foreach (var item in Outlines) {
            var ent = item.Value.GetComponent<NavigatorEntityManager>();
            if (ent != null) {
                if (ent.key != key) {
                    OutlineCreator.SetTile(item.Key);
                    forRemovals.Add(item.Key);
                }
            }
        }

        foreach (var item in forRemovals) {
            Outlines.Remove(item);
        }
        forRemovals.Clear();
        OutlineCreator.Tile = (Tile) GameObject.FindGameObjectWithTag("ScriptsContainer").GetComponent<GameAssetsCollection>().Outline;
        NavigatorOutlinesShown = false;
    }

    public static void ClearBaseOutlines() {
        OutlineCreator.Tile = BlankTile;
        List<Vector3Int> forRemovals = new List<Vector3Int>();
        foreach (var item in Outlines) {
            var ent = item.Value.GetComponent<BaseEntityManager>();
            if (ent != null) {
                OutlineCreator.SetTile(item.Key);
                forRemovals.Add(item.Key);
            }
        }
        foreach (var item in forRemovals) {
            Outlines.Remove(item);
        }
        forRemovals.Clear();
        OutlineCreator.Tile = (Tile) GameObject.FindGameObjectWithTag("ScriptsContainer").GetComponent<GameAssetsCollection>().Outline;
        BaseOutlinesShown = false;
    }

    public static void ShowOutlines(NavigatorEntityManager entity) {
        foreach (var item in Outlines) {
            var ent = item.Value.GetComponent<NavigatorEntityManager>();
            if (entity.key == ent.key) {
                OutlineCreator.SetTile(item.Key);
            }
        }
        NavigatorOutlinesShown = true;
    }

    public static void ShowOutlines(BaseEntityManager entity) {
        foreach (var item in Outlines) {
            var ent = item.Value.GetComponent<BaseEntityManager>();
            if (ent != null) {
                if (entity.key == ent.key) {
                    OutlineCreator.SetTile(item.Key);
                }
            }
        }
        BaseOutlinesShown = true;
    }

    private void Update() {

        if (Input.GetMouseButtonDown(0)) {
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            var mousePosY = mousePos.y - 11.7f;
            mousePos = new Vector3(mousePos.x, mousePosY, mousePos.z);
            Vector2 mousePos2d = new Vector2(mousePos.x, mousePos.y);
            RaycastHit2D hit = Physics2D.Raycast(mousePos2d, Vector2.zero);
            var cell = GameObject.FindGameObjectWithTag("ScriptsContainer").GetComponent<GameAssetsCollection>().Layers[0].WorldToCell(mousePos);
            if (hit.collider == null && !Outlines.ContainsKey(cell)) {
                ClearBaseOutlines();
                ClearNavigatorOutlines();
            } else if (hit.collider != null && !Outlines.ContainsKey(cell)) {
                if (hit.collider.tag == "Base") {
                    ClearNavigatorOutlines();
                }
                if (hit.collider.tag == "Navigator") {
                    ClearBaseOutlines();
                }
            } else {
                Debug.Log("Outline Clicked");
                var obj = Outlines[cell];

                if (BaseOutlinesShown) {
                    var gridpos = obj.GetComponent<BaseEntityManager>().GridPosition;
                    var pos = IlluminaConverter.ToCoordInt(gridpos);
                    pos = IlluminaConverter.FlapTopSwitch(pos);
                    var clickedBase = GameObject.FindGameObjectWithTag("ScriptsContainer").GetComponent<GamePlayManager>().world.Map.Maps.BasesMap[pos];
                    GameObject.FindGameObjectWithTag("ScriptsContainer").GetComponent<GamePlayManager>().world.AddNavigator(cell, clickedBase.owner, gridpos);
                    ClearBaseOutlines();
                }
                if (NavigatorOutlinesShown) {
                    var gridpos = obj.GetComponent<NavigatorEntityManager>().GridPosition;
                    var pos = IlluminaConverter.ToCoordInt(gridpos);
                    //pos = IlluminaConverter.FlapTopSwitch(pos);
                    var clickedNavigator = GameObject.FindGameObjectWithTag("ScriptsContainer").GetComponent<GamePlayManager>().world.Map.Maps.NavigatorsMap[pos];
                    GameObject.FindGameObjectWithTag("ScriptsContainer").GetComponent<GamePlayManager>().world.UpdateNavigatorPosition(gridpos, cell, obj);
                    ClearNavigatorOutlines();
                }
            }
        }
    }
}