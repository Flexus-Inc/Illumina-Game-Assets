using System.Collections;
using System.Collections.Generic;
using Illumina.Controller;
using UnityEngine;

public class GameLoader : MonoBehaviour {
    // Start is called before the first frame update
    void OnEnable() {
        GameData.PlayDataLoaded = false;
        PlayDataController.LoadPlayData();
    }

    // Update is called once per frame
    void Update() {

    }
}