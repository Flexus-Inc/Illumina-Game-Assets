using UnityEngine;

namespace Illumina {

    [System.Serializable]
    public class WorldEntity {
        public Player owner;
        public CoordInt GridPosition;
        public WorldEntity(Player owner, Vector3Int pos) {
            this.owner = owner;
            this.GridPosition = IlluminaConverter.ToCoordInt(pos);
        }
    }
    public class Floor : WorldEntity {
        //
        public Floor(Player owner, Vector3Int pos) : base(owner, pos) { }
    }

    [System.Serializable]
    public class Base : WorldEntity {
        public Base(Player owner, Vector3Int pos) : base(owner, pos) {
            //will not do anything;
        }

    }

    [System.Serializable]
    public class Navigator : WorldEntity {
        //
        public Navigator(Player owner, Vector3Int pos) : base(owner, pos) { }
    }

    [System.Serializable]
    public class General : WorldEntity {
        //
        public General(Player owner, Vector3Int pos) : base(owner, pos) { }
    }

    [System.Serializable]
    public class Trap : WorldEntity {
        //
        public Trap(Player owner, Vector3Int pos) : base(owner, pos) { }
    }

}