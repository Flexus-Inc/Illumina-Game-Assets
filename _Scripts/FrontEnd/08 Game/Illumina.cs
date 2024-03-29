using UnityEngine;

namespace Illumina {

    //Illumina Datatypes
    public struct Limit {

        public float x, y;

        public Limit(float _xy) {
            this.x = _xy;
            this.y = _xy;
        }
        public Limit(float _x, float _y) {
            this.x = _x;
            this.y = _y;
        }

        public bool CompareTo(float other) {
            var isX = (other < this.x) || (other > this.y);
            if (isX) {
                return false;
            }
            return true;
        }
    }

    [System.Serializable]
    public class Coord {
        protected float x, y, z;

        public Coord(float x, float y, float z = 0) {
            this.x = x;
            this.y = y;
            this.z = z;
        }

        public virtual float X { get => x; }
        public virtual float Y { get => y; }
        public virtual float Z { get => z; }

        public override bool Equals(object obj) {
            return obj is Coord coord &&
                X == coord.X &&
                Y == coord.Y &&
                Z == coord.Z;
        }

        public override int GetHashCode() {
            var hashCode = 373119288;
            hashCode = hashCode * -1521134295 + X.GetHashCode();
            hashCode = hashCode * -1521134295 + Y.GetHashCode();
            hashCode = hashCode * -1521134295 + Z.GetHashCode();
            return hashCode;
        }

        //DELETE ME: might create a runtime error

        public static bool operator ==(Coord a, Coord b) => Equals(a, b);
        public static bool operator !=(Coord a, Coord b) => Equals(a, b);

        public Vector3 ToVector3() {
            return new Vector3(this.x, this.y, this.z);
        }
    }

    [System.Serializable]
    public class CoordInt : Coord {
        public CoordInt(int x, int y, int z = 0) : base((float) x, (float) y, (float) z) {
            //do nothing;
        }

        public override float X { get => (int) x; }
        public override float Y { get => (int) y; }
        public override float Z { get => (int) z; }

        public Vector3Int ToVector3Int() {
            return new Vector3Int((int) this.X, (int) this.Y, (int) this.Z);
        }
    }

    public class IlluminaConverter {
        public static CoordInt ToCoordInt(Vector3Int pos) {
            return new CoordInt(pos.x, pos.y, pos.z);
        }

        public static Vector3Int ToFlatTopPos(Vector3Int pos) {
            return new Vector3Int(pos.y, pos.x, pos.z);
        }

        public static CoordInt FlapTopSwitch(CoordInt pos) {
            return new CoordInt((int) pos.Y, (int) pos.X, (int) pos.Z);
        }
    }
}