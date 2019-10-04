using System.Collections;
using System.Collections.Generic;
using Illumina;
using UnityEngine;

public class GamePlayManager : MonoBehaviour {

    World world;
    void Start() {
        var collections = this.gameObject.GetComponent<GameAssetsCollection>();
        world = new World(collections.ToWorldCollection());
        StartCoroutine(PlaceBase());
    }
    
    // Update is called once per frame
    IEnumerator PlaceBase() {
        world.Test();
        yield return null;
    }
}