using System.Collections.Generic;
using Illumina.Models;
using UnityEngine;

namespace Illumina {

    public enum DestinationEntity {
        NoDestination = -1, None = 0, Base = 1, General, Navigator, Trap
    }

    [System.Serializable]
    public class WorldMaps {
        //
        public Dictionary<CoordInt, WorldEntity> EntitiesMap;
        public Dictionary<CoordInt, Floor> FloorMap;
        public Dictionary<CoordInt, Base> BasesMap;
        public Dictionary<CoordInt, General> GeneralsMap;
        public Dictionary<CoordInt, Navigator> NavigatorsMap;
        public Dictionary<CoordInt, Trap> TrapsMap;

        public WorldMaps() {
            EntitiesMap = new Dictionary<CoordInt, WorldEntity>();
            FloorMap = new Dictionary<CoordInt, Floor>();
            BasesMap = new Dictionary<CoordInt, Base>();
            GeneralsMap = new Dictionary<CoordInt, General>();
            NavigatorsMap = new Dictionary<CoordInt, Navigator>();
            TrapsMap = new Dictionary<CoordInt, Trap>();
        }
    }

    [System.Serializable]
    public class WorldMap {

        public WorldMaps Maps;
        //TODO: remove public keyword before build

        public WorldMap() {
            Maps = new WorldMaps();
        }

        public DestinationEntity PlaceBase(Vector3Int pos, Base baseEntity) {
            var destination = IlluminaConverter.ToCoordInt(pos);
            if (Maps.EntitiesMap.ContainsKey(destination)) {
                return DestinationEntity.NoDestination;
            } else {
                Maps.BasesMap.Add(destination, baseEntity);
                Maps.EntitiesMap.Add(destination, baseEntity);
                return DestinationEntity.None;
            }
        }

        public DestinationEntity PlaceGeneral(Vector3Int pos, General general) {
            var destination = IlluminaConverter.ToCoordInt(pos);
            if (Maps.EntitiesMap.ContainsKey(destination)) {
                return DestinationEntity.NoDestination;
            } else {
                Maps.GeneralsMap.Add(destination, general);
                Maps.EntitiesMap.Add(destination, general);
                return DestinationEntity.None;
            }
        }

        public DestinationEntity CheckDestinationEntity(CoordInt pos) {

            if (Maps.BasesMap.ContainsKey(pos)) {
                return DestinationEntity.Base;
            }
            if (Maps.NavigatorsMap.ContainsKey(pos)) {
                return DestinationEntity.Navigator;
            }
            if (Maps.GeneralsMap.ContainsKey(pos)) {
                return DestinationEntity.General;
            }

            return DestinationEntity.Trap;
        }

        public Trap RemoveTrap(Vector3Int pos) {
            var trap_pos = IlluminaConverter.ToCoordInt(pos);
            if (Maps.TrapsMap.ContainsKey(trap_pos)) {
                var trap = Maps.TrapsMap[trap_pos];
                Maps.TrapsMap.Remove(trap_pos);
                Maps.EntitiesMap.Remove(trap_pos);
                return trap;
            } else {
                return new Trap(new Player(new User()), pos);
                //TODO: must send the actual player 
            }
        }

        public DestinationEntity PlaceTrap(Vector3Int pos, Trap trap) {
            var trap_pos = IlluminaConverter.ToCoordInt(pos);
            if (Maps.EntitiesMap.ContainsKey(trap_pos)) {
                return CheckDestinationEntity(trap_pos);
            } else {
                Maps.TrapsMap.Add(trap_pos, trap);
                Maps.EntitiesMap.Add(trap_pos, trap);
                return DestinationEntity.None;
            }
        }

        // ChangeNavigatorPosition(,) == DestinationEntity.None;
        public DestinationEntity ChangeNavigatorPosition(Vector3Int oldPos, Vector3Int newPos) {
            var old_pos = IlluminaConverter.ToCoordInt(oldPos);
            var new_pos = IlluminaConverter.ToCoordInt(newPos);
            if (Maps.EntitiesMap.ContainsKey(new_pos)) {
                return CheckDestinationEntity(new_pos);
            }

            if (Maps.NavigatorsMap.ContainsKey(old_pos)) {
                var navigator = Maps.NavigatorsMap[old_pos];
                Maps.NavigatorsMap.Remove(old_pos);
                Maps.EntitiesMap.Remove(old_pos);
                Maps.NavigatorsMap.Add(new_pos, navigator);
                Maps.EntitiesMap.Add(new_pos, navigator);
                return DestinationEntity.None;
            }

            return DestinationEntity.NoDestination;

        }

        public bool RemoveNavigator(Vector3Int pos) {
            var navigator_pos = IlluminaConverter.ToCoordInt(pos);
            if (Maps.NavigatorsMap.ContainsKey(navigator_pos)) {
                Maps.NavigatorsMap.Remove(navigator_pos);
                Maps.EntitiesMap.Remove(navigator_pos);
                return true;
            }

            return false;
        }

    }
}