using System.Collections.Generic;
using UnityEngine;

namespace Illumina {
    public class BasesCreator {

        World world;

        public BasesCreator(World world) {
            this.world = world;
        }
        Vector3Int GetRandomPos(int[] p) {
            while (true) {
                int x = Random.Range(p[0], p[1]);
                int y = Random.Range(p[2], p[3]);
                Vector3Int pos = new Vector3Int(x, y, 0);
                return pos;
            }
        }

        public List<Vector3Int> GetBasesPosition() {
            var bases = 4;

            List<Vector3Int> base_pos = new List<Vector3Int>();
            int num = bases;

            while (num > 0) {
                var pos = GetRandomPos(new int[] { 1, 23, 1, 23 });

                if (num < bases) {
                    int iterations = bases - num;
                    bool IsNearby = true;
                    var req = 8;

                    //Gets the random coordinates until it is not nearby the previous base
                    do {
                        pos = GetRandomPos(new int[] { 1, 23, 1, 23 });
                        IsNearby = false;
                        for (int i = 0; i < iterations; i++) {
                            var test = base_pos[i];
                            var xdist = Mathf.Pow((pos.x - test.x), 2);
                            var ydist = Mathf.Pow((pos.y - test.y), 2);
                            var dist = (int) Mathf.Sqrt(xdist + ydist);
                            if (dist < req) {
                                IsNearby = true;
                            }
                        }
                    } while (IsNearby);
                }

                base_pos.Add(pos);
                num--;
            }
            return base_pos;
        }

        public void PlaceBases(List<Player> players) {
            var BasesPos = GetBasesPosition();
            for (int i = 0; i < players.Count; i++) {
                Base player_base = new Base(players[i], BasesPos[i]);
                world.Map.PlaceBase(BasesPos[i], player_base);
            }
            GenerateBasesFloor();
        }

        public void GenerateBasesFloor() {
            foreach (var player_base in world.Map.Maps.BasesMap) {
                GenerateRandomFloorPosition(player_base.Value, player_base.Key);
            }
        }

        public CoordInt ReturnSafePos(int[] p, CoordInt pos) {
            int x = (int) pos.X, y = (int) pos.Y, z = (int) pos.Z;
            if (pos.X > p[1]) {
                x = (int) pos.X - p[1] - 1;
            }
            if (pos.X < p[0]) {
                x = p[1] + (int) pos.X + 1;
            }
            if (pos.Y > p[3]) {
                y = (int) pos.Y - p[3] - 1;
            }
            if (pos.Y < p[2]) {
                y = p[3] + (int) pos.Y + 1;
            }
            return new CoordInt(x, y, z);
        }

        public void GenerateRandomFloorPosition(Base playbase, CoordInt center, int max = 6, int i = 0) {

            Outline _outline = new Outline(world.Collection.Layers[0], world.Collection.GroundTiles[1]);

            var positions = _outline.GetSurroundingPos(center.ToVector3Int());
            var scattering_center = new List<CoordInt>();
            var space_left = 0;

            //Determines how many unoccupied sorrounding tile in the floormap
            foreach (var position in positions) {
                if (!world.Map.Maps.FloorMap.ContainsKey(IlluminaConverter.ToCoordInt(position))) {
                    space_left++;
                }
            }

            //If the space_left exceeds 3 use the minimum value which is 5, otherwise don't change the value
            space_left = space_left > 3 ? 5 : space_left;

            //Get the center of the next scattering positions
            for (int j = 0; j < space_left; j++) {
                var pos_index = 0;
                var pos = IlluminaConverter.ToCoordInt(positions[pos_index]);
                do {
                    pos_index = Random.Range(0, 7);
                    pos = IlluminaConverter.ToCoordInt(positions[pos_index]);
                } while (world.Map.Maps.FloorMap.ContainsKey(pos) || pos == center);
                scattering_center.Add(new CoordInt((int) pos.Y, (int) pos.X, (int) pos.Z));
                //FIXME : x becomes y, and y becomes xs
            }

            //Adds all the positions to the FloorMap
            for (int l = 0; l < positions.Length; l++) {
                var _position = IlluminaConverter.ToCoordInt(positions[l]);
                _position = ReturnSafePos(new int[] { 0, 24, 0, 24 }, _position);
                if (!world.Map.Maps.FloorMap.ContainsKey(_position)) {
                    world.Map.Maps.FloorMap.Add(_position, new Floor(playbase.owner, positions[l]));
                }
            }

            //Recursion : ScatterAgain
            if (i < max && scattering_center.Count > 0) {
                for (int k = 0; k < scattering_center.Count; k++) {
                    i++;
                    GenerateRandomFloorPosition(playbase, scattering_center[k], max, i);
                }
            }
        }
    }
}