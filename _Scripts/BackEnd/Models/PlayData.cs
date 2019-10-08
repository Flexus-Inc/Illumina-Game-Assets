using System.Collections.Generic;

namespace Illumina.Models {

    [System.Serializable]
    public class PlayData : IlluminaModel {
        public bool old = false;
        public WorldMap worldMap;
        //public List<Player> Players;
    }
}