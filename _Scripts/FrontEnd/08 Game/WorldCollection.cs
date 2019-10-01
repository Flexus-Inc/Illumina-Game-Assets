using UnityEngine;
using UnityEngine.Tilemaps;

namespace Illumina {

    [System.Serializable]
    public class WorldCollection {
        public Tilemap[] Layers;
        public TileBase[] GroundTiles;
        public TileBase Outline;
        public TileBase TrapMagic;
        public GameObject[] GeneralPrefabs;
        public GameObject[] NavigatorPrefabs;
        public GameObject[] BasePrefabs;

    }
}