using System.Collections;
using System.Collections.Generic;
using Illumina;
using UnityEngine;
using UnityEngine.Tilemaps;

public class NavigatorEntityManager : MonoBehaviour {

    public static Navigator clickedNavigator;

    public Tile Blank;
    public static Tile Blanktile;
    static Outline OutlineCreator;
    World world;
    public bool OutlineShown = false;
    public Vector3 BasePosition;
    public Vector3Int GridPosition;
    public static List<Vector3Int> OutlinePositions;
    public Tribe TribeIdentity;
    public string key;
    // Start is called before the first frame update
    void Awake() {
        var map = GameObject.FindGameObjectWithTag("ScriptsContainer").GetComponent<GameAssetsCollection>().Layers[1];
        var outline = (Tile) GameObject.FindGameObjectWithTag("ScriptsContainer").GetComponent<GameAssetsCollection>().Outline;
        OutlineCreator = new Outline(map, outline);
        OutlinePositions = new List<Vector3Int>();
        world = GameObject.FindGameObjectWithTag("ScriptsContainer").GetComponent<GamePlayManager>().world;
        clickedNavigator = null;
        Blanktile = Blank;

    }

    void OnMouseDown() {
        if (!GamePlayManager.GamePaused && GamePlayManager.MovementEnabled && (this.gameObject.GetComponent<NavigatorEntityManager>().TribeIdentity == (Tribe) GamePlayManager.PlayerTurn) && this.key == GamePlayManager.ActiveNavigatorKey) {
            var selectedObj = this.gameObject;
            selectedObj.GetComponent<NavigatorEntityManager>().ShowOutlines(selectedObj.GetComponent<NavigatorEntityManager>().GridPosition);
            OutlineManager.ShowOutlines(selectedObj.GetComponent<NavigatorEntityManager>());
        }
    }

    public void ShowOutlines(Vector3Int pos) {
        var outlines = OutlineCreator.GetSurroundingPos(IlluminaConverter.ToFlatTopPos(pos));
        OutlineManager.ClearBaseOutlines();
        OutlineManager.ClearOtherNavigatorOutlines(this.key);
        for (int i = 0; i < outlines.Length; i++) {
            var _pos = IlluminaConverter.ToCoordInt(outlines[i]);

            if (world.Map.Maps.GeneralsMap.ContainsKey(_pos)) {
                var general = world.Map.Maps.GeneralsMap[_pos];
                if (general.owner.tribe == this.gameObject.GetComponent<NavigatorEntityManager>().TribeIdentity) {
                    continue;
                }
            }
            if (world.Map.Maps.BasesMap.ContainsKey(IlluminaConverter.FlapTopSwitch(_pos))) {
                continue;
            }
            if (world.Map.Maps.NavigatorsMap.ContainsKey(_pos)) {
                var navigator = world.Map.Maps.NavigatorsMap[_pos];
                if (navigator.owner.tribe == this.gameObject.GetComponent<NavigatorEntityManager>().TribeIdentity) {
                    continue;
                }
            }

            if (i != 3) {
                if (!OutlineManager.Outlines.ContainsKey(outlines[i])) {
                    OutlineManager.Outlines.Add(outlines[i], this.gameObject);
                }
            }
        }

    }

    public void MoveNavigatorEntity(Vector3 oldPos, Vector3 newPos, GameObject navigator) {
        StartCoroutine(AnimateNavigatorMovement(oldPos, newPos, navigator));
    }

    IEnumerator AnimateNavigatorMovement(Vector3 oldpos, Vector3 newpos, GameObject navigator) {
        yield return null;
        var orb = navigator.transform.GetChild(1).gameObject;
        float timeStartedLerping = Time.time;
        float timeSinceStarted = Time.time - timeStartedLerping;
        float percentageComplete = timeSinceStarted / 0.75f;
        Debug.Log("runned");
        navigator.GetComponent<Animator>().SetTrigger("Run");
        while (true) {
            timeSinceStarted = Time.time - timeStartedLerping;
            percentageComplete = timeSinceStarted / 0.75f;
            float currentXValue = Mathf.Lerp(oldpos.x, newpos.x, percentageComplete);
            float currentYValue = Mathf.Lerp(oldpos.y, newpos.y, percentageComplete);
            float currentZValue = Mathf.Lerp(oldpos.z, newpos.z, percentageComplete);
            float orbcurrentXValue = Mathf.Lerp(oldpos.x, newpos.x, percentageComplete * 0.75f);
            Vector3 pos = new Vector3(currentXValue, currentYValue, currentZValue);     
            navigator.transform.position = pos;
            if (percentageComplete >= 1) break;
            yield return new WaitForEndOfFrame();
        }
        
        navigator.GetComponent<Animator>().SetTrigger("Idle");

    }

}