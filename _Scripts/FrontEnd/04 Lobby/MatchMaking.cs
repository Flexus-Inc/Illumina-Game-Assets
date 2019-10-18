using System.Collections;
using System.Collections.Generic;
using Illumina.Controller;
using UnityEngine;

public class MatchMaking : MonoBehaviour {
    // Start is called before the first frame update
    void Start() {
        LobbyController.Register(GameData.User);
    }

    // Update is called once per frame
    void Update() {

    }
}
/*
waitingroom
rooms[]
 */