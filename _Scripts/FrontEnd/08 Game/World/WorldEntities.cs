using Illumina.Security;
using UnityEngine;

namespace Illumina {

    public enum WorldEntityType {
        Floor = 0, Base = 1, General, Navigator, Trap
    }

    [System.Serializable]
    public class WorldEntity {
        public Player owner;
        public CoordInt GridPosition;
        public string key;
        public WorldEntity(Player owner, Vector3Int pos) {
            this.owner = owner;
            this.GridPosition = IlluminaConverter.ToCoordInt(pos);
            this.key = Keys.RandomKey(7);
        }
    }

    [System.Serializable]
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
        public bool flipX = false;
        public Navigator(Player owner, Vector3Int pos, bool flipX = false) : base(owner, pos) {
            owner.navigators.Add(this.key, this);
            this.flipX = flipX;
        }
    }

    [System.Serializable]
    public class General : WorldEntity {
        //

        public General(Player owner, Vector3Int pos) : base(owner, pos) {

        }
    }

    [System.Serializable]
    public class Trap : WorldEntity {
        //
        public Trap(Player owner, Vector3Int pos) : base(owner, pos) {
            owner.traps.Add(this.key, this);
        }
    }

}