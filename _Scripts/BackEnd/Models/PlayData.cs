using System;
using System.Collections.Generic;
using Illumina.Security;
using UnityEngine;

namespace Illumina.Models {

    public enum PlayerStatus {
        Active = 1, Inactive = 2, Out = 3
    }

    [System.Serializable]
    public class PlayData : IlluminaModel {
        public bool old = false;
        public string key;
        public List<Player> players = new List<Player>();
        public WorldMap worldMap;
        public PlayData() {
            key = (new Key()).GenerateRandom(7);
        }
        public GamePlayData ToServerData() {
            GamePlayData data = new GamePlayData();
            data.play_key = this.key;
            //data.play_id get the latest id in the laravel server;
            for (int i = 0; i < this.players.Count; i++) {
                data.players_username[i] = players[i].username;
                data.players_status[i] = players[i].status.ToString();
                data.players_is_host[i] = players[i].host;
                data.players_tribe[i] = (int) players[i].tribe;
                data.count_navigators[i] = players[i].navigators.Count;
                data.count_traps[i] = players[i].traps.Count;
            }

            List<string> owners = new List<string>();
            List<string> fowners = new List<string>();
            List<bool> ownersIsHost = new List<bool>();
            List<int> ownersTribe = new List<int>();
            List<string> keys = new List<string>();
            List<string> fkeys = new List<string>();
            List<int> types = new List<int>();
            List<int> posx = new List<int>();
            List<int> fposx = new List<int>();
            List<int> posy = new List<int>();
            List<int> fposy = new List<int>();
            List<bool> flipx = new List<bool>();
            foreach (var item in worldMap.Maps.EntitiesMap) {
                owners.Add(item.Value.owner.username);
                ownersTribe.Add((int) item.Value.owner.tribe);
                ownersIsHost.Add(item.Value.owner.host);
                keys.Add(item.Value.key);
                types.Add((int) item.Value.type);
                posx.Add((int) item.Key.X);
                posy.Add((int) item.Key.Y);
                if (item.Value.type == WorldEntityType.Navigator) {
                    flipx.Add(((Navigator) item.Value).flipX);
                } else {
                    flipx.Add(false);
                }
            }

            foreach (var item in worldMap.Maps.FloorMap) {
                fowners.Add(item.Value.owner.username);
                fkeys.Add(item.Value.key);
                fposx.Add((int) item.Key.X);
                fposy.Add((int) item.Key.Y);
            }
            data.w_owner = owners.ToArray();
            data.w_owner_host = ownersIsHost.ToArray();
            data.w_owner_tribe = ownersTribe.ToArray();
            data.w_type = types.ToArray();
            data.w_key = keys.ToArray();
            data.w_posx = posx.ToArray();
            data.w_posy = posy.ToArray();
            data.w_flipx = flipx.ToArray();
            data.f_owner = fowners.ToArray();
            data.f_key = fkeys.ToArray();
            data.f_posx = fposx.ToArray();
            data.f_posy = fposy.ToArray();
            return data;
        }
    }

    public class GamePlayData {
        public int play_id;
        public string play_key;
        public string[] players_username = new string[4];
        public bool[] players_is_host = new bool[4];
        public string[] players_status = new string[4];
        public int[] players_tribe = new int[4];
        public int[] count_traps = new int[4];
        public int[] count_navigators = new int[4];
        public string[] w_owner;
        public bool[] w_owner_host;
        public int[] w_owner_tribe;
        public string[] w_key;
        public int[] w_type;
        public int[] w_posx;
        public int[] w_posy;
        public bool[] w_flipx;
        public string[] f_owner;
        public string[] f_key;
        public int[] f_posx;
        public int[] f_posy;

        public PlayData ToPlayData() {
            PlayData data = new PlayData();
            data.worldMap = new WorldMap();
            data.old = true;
            data.key = this.play_key;
            for (int i = 0; i < players_username.Length; i++) {
                User user = new User();
                user.username = players_username[i];
                Player owner = new Player(user, players_is_host[i]);
                owner.status = (PlayerStatus) Enum.Parse(typeof(PlayerStatus), players_status[i]);
                owner.tribe = (Tribe) players_tribe[i];
                data.players.Add(owner);
            }

            for (int i = 0; i < w_owner.Length; i++) {
                Vector3Int pos = new Vector3Int(w_posx[i], w_posy[i], 0);
                WorldEntityType type = (WorldEntityType) w_type[i];
                Player owner = Player.GetPlayer(data.players, w_owner[i]);

                switch (type) {
                    case WorldEntityType.Base:
                        var _base = new Base(owner, pos);
                        _base.key = w_key[i];
                        data.worldMap.Maps.BasesMap.Add(IlluminaConverter.ToCoordInt(pos), _base);
                        data.worldMap.Maps.EntitiesMap.Add(IlluminaConverter.ToCoordInt(pos), _base);
                        break;
                    case WorldEntityType.General:
                        var general = new General(owner, pos);
                        general.key = w_key[i];
                        data.worldMap.Maps.GeneralsMap.Add(IlluminaConverter.ToCoordInt(pos), general);
                        data.worldMap.Maps.EntitiesMap.Add(IlluminaConverter.ToCoordInt(pos), general);
                        break;
                    case WorldEntityType.Navigator:
                        var navigator = new Navigator(owner, pos, w_flipx[i]);
                        navigator.key = w_key[i];
                        data.worldMap.Maps.NavigatorsMap.Add(IlluminaConverter.ToCoordInt(pos), navigator);
                        data.worldMap.Maps.EntitiesMap.Add(IlluminaConverter.ToCoordInt(pos), navigator);
                        break;
                    case WorldEntityType.Trap:
                        var trap = new Trap(owner, pos);
                        trap.key = w_key[i];
                        data.worldMap.Maps.TrapsMap.Add(IlluminaConverter.ToCoordInt(pos), trap);
                        data.worldMap.Maps.EntitiesMap.Add(IlluminaConverter.ToCoordInt(pos), trap);
                        break;
                    default:
                        break;
                }
            }

            for (int i = 0; i < f_owner.Length; i++) {
                Vector3Int pos = new Vector3Int(f_posx[i], f_posy[i], 0);
                Player owner = Player.GetPlayer(data.players, f_owner[i]);
                var floor = new Floor(owner, pos);
                floor.key = f_key[i];
                data.worldMap.Maps.FloorMap.Add((IlluminaConverter.ToCoordInt(pos)), floor);
            }

            return data;
        }

    }

    public class PlayRoom {
        public int status;
        public int hostid;
        public int turn;
        public string host;
        public Player[] players;
        public GamePlayData data;
    }
}