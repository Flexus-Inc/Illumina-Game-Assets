using UnityEngine;
using UnityEngine.Tilemaps;

namespace Illumina {
    public class Outline {
        protected Tilemap OutlineMap;
        protected TileBase Tile;
        protected Vector3Int[] HexPos = new Vector3Int[7];

        public Vector3Int[] GetHexPos() {
            return HexPos;
        }

        public Outline(Tilemap map, TileBase tile) {
            HexPos = new Vector3Int[7];
            Tile = tile;
            OutlineMap = map;
        }

        protected Vector3Int[] FindHexPos(Vector3Int center) {
            var factor = (center.x % 2 == 0) ? 1 : -1;
            var start = (center.x % 2 == 0) ? 0 : HexPos.Length - 1;

            int[] x = new int[] {
                center.x,
                center.x - 1,
                center.x + 1,
                center.x,
                center.x - 1,
                center.x + 1,
                center.x
            };
            int[] y = new int[7];

            if (center.x % 2 == 0) {
                y[0] = center.y + 1;
                y[1] = center.y;
                y[2] = center.y;
                y[3] = center.y;
                y[4] = center.y - 1;
                y[5] = center.y - 1;
                y[6] = center.y - 1;
            } else {
                y[0] = center.y + 1;
                y[1] = center.y + 1;
                y[2] = center.y + 1;
                y[3] = center.y;
                y[4] = center.y;
                y[5] = center.y;
                y[6] = center.y - 1;
            }

            for (int i = 0; i < HexPos.Length; i++) {
                HexPos[i] = new Vector3Int(y[i], x[i], 0);
            }

            return HexPos;

        }
        public void FillHexPos() {
            for (int i = 0; i < HexPos.Length; i++) {
                if (i == 3) {
                    continue;
                }
                OutlineMap.SetTile(HexPos[i], Tile);
            }
        }

        public void FillHexPos(Tile tile) {
            var defaultTile = this.Tile;
            this.Tile = tile;
            FillHexPos();
            this.Tile = defaultTile;
        }

        public Vector3Int[] GetSurroundingPos(Vector3Int center) {
            FindHexPos(center);
            return HexPos;
        }
    }
}