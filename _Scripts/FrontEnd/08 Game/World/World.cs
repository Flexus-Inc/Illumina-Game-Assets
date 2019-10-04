using System.Collections.Generic;
using Illumina.Models;
using Illumina.Security;
using UnityEngine;

namespace Illumina {

    public class World {
        public WorldCollection Collection;
        public WorldMap Map;

        public World(WorldCollection collection) {
            Collection = collection;
            Map = new WorldMap();
        }

        public void Test() {
            List<Player> players = new List<Player>();
            for (int i = 0; i < 4; i++) {
                var host = i == 0 ? true : false;
                var user = new User();
                user.name = Keys.RandomKey(7);
                var player = new Player(user, host);
                player.tribe = (Tribe) i;
                players.Add(player);
            }
            var bc = new BasesCreator(this);
            bc.PlaceBases(players);
            foreach (var item in Map.Maps.FloorMap) {
                var tileIndex = (int) item.Value.owner.tribe;
                Collection.Layers[0].SetTile(item.Key.ToVector3Int(), Collection.GroundTiles[tileIndex]);
            }
        }
    }
}