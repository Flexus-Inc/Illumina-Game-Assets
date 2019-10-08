using System.Collections;
using System.Collections.Generic;
using Illumina;
using UnityEngine;
using UnityEngine.Tilemaps;

public class GameAssetsCollection : MonoBehaviour {
    public Tilemap[] Layers;
    public TileBase[] GroundTiles;
    public TileBase Outline;
    public TileBase TrapMagic;
    public GameObject[] EntitiesParent;
    public GameObject[] GeneralPrefabs;
    public GameObject[] NavigatorPrefabs;
    public GameObject[] BasePrefabs;

    public WorldCollection ToWorldCollection() {
        var collection = new WorldCollection();
        collection.Layers = this.Layers;
        collection.GroundTiles = this.GroundTiles;
        collection.Outline = this.Outline;
        collection.TrapMagic = this.TrapMagic;
        collection.EntitiesParent = this.EntitiesParent;
        collection.GeneralPrefabs = this.GeneralPrefabs;
        collection.NavigatorPrefabs = this.NavigatorPrefabs;
        collection.BasePrefabs = this.BasePrefabs;
        return collection;
    }
}