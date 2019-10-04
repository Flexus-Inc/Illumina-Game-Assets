// public void GenerateRandomFloorPosition(Base playbase, CoordInt center, int max = 2, int i = 0) {
//     Outline outline = new Outline(Collection.Layers[0], Collection.GroundTiles[0]);
//     var scattering_pos = new CoordInt[2];
//     var positions = outline.GetSurroundingPos(center.ToVector3Int());
//     var space_left = 0;
//     foreach (var position in positions) {
//         if (!Map.Maps.FloorMap.ContainsKey(IlluminaConverter.ToCoordInt(position))) {
//             space_left++;
//         }
//     }
//     space_left = space_left > 2 ? 2 : space_left;
//     for (int j = 0; j < space_left; j++) {
//         var pos_index = 0;
//         var pos = IlluminaConverter.ToCoordInt(positions[pos_index]);
//         var random = new System.Random();
//         do {
//             pos_index = Mathf.Clamp(random.Next(0, 7), 0, 6);
//             pos = IlluminaConverter.ToCoordInt(positions[pos_index]);
//             if (!Map.Maps.FloorMap.ContainsKey(pos)) {
//                 break;
//             }
//         } while (Map.Maps.FloorMap.ContainsKey(pos));
//         scattering_pos[j] = pos;
//     }

//     // foreach (var position in positions) {
//     //     
//     // }
//     for (int l = 0; l < positions.Length; l++) {
//         var _position = IlluminaConverter.ToCoordInt(positions[l]);
//         if (!Map.Maps.FloorMap.ContainsKey(_position)) {
//             Map.Maps.FloorMap.Add(_position, new Floor(playbase.owner, positions[l]));
//         }
//     }
//     // if (i < max) {
//     //     for (int k = 0; k < scattering_pos.Length; k++) {
//     //         GenerateRandomFloorPosition(playbase, scattering_pos[k], max, i++);
//     //     }
//     // }
// }