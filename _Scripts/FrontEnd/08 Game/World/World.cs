using System.Collections.Generic;
using Illumina.Controller;
using Illumina.Models;
using Illumina.Security;
using UnityEngine;

namespace Illumina {

    [System.Serializable]
    public class World {
        public List<Player> players = new List<Player>();
        public WorldCollection Collection;
        public WorldMap Map;

        public World(WorldCollection collection) {
            Collection = collection;
            Map = new WorldMap();
        }

        public void PaintGround() {
            foreach (var item in Map.Maps.FloorMap) {
                var tileIndex = (int) item.Value.owner.tribe;
                Collection.Layers[0].SetTile(item.Key.ToVector3Int(), Collection.GroundTiles[tileIndex]);
            }
        }

        public void PlaceBaseStructures() {
            foreach (var item in Map.Maps.BasesMap) {
                var tribeIndex = (int) item.Value.owner.tribe;
                var pos = IlluminaConverter.ToFlatTopPos(item.Key.ToVector3Int());
                var worldPos = Collection.Layers[0].CellToWorld(pos);
                worldPos.z = 0;
                var basesParent = Collection.EntitiesParent[0].transform;
                var baseStructure = Collection.BasePrefabs[tribeIndex];
                var baseObject = Object.Instantiate(baseStructure, worldPos, Quaternion.identity, basesParent);
                var p = baseObject.transform.position;
                baseObject.GetComponent<BaseEntityManager>().GridPosition = pos;
                baseObject.GetComponent<BaseEntityManager>().TribeIdentity = item.Value.owner.tribe;
                baseObject.GetComponent<BaseEntityManager>().BasePosition = baseObject.transform.position;
                baseObject.GetComponent<BaseEntityManager>().key = item.Value.key;
            }
        }

        public void AddNavigator(Vector3Int pos, Player owner, Vector3Int gridpos) {
            var vacantSlot = !Map.Maps.NavigatorsMap.ContainsKey(IlluminaConverter.ToCoordInt(pos));
            var hasRights = (int) owner.tribe == GamePlayManager.PlayerTurn;
            if (vacantSlot && hasRights && owner.navigators.Count < 3) {
                var flipX = false;
                if (pos.y < gridpos.y) {
                    flipX = true;
                }
                var navigator = new Navigator(owner, pos, flipX);
                Map.PlaceNavigator(pos, navigator);
                PlaceNavigator(navigator, flipX);
                GamePlayManager.EnableNavButton(players[GamePlayManager.PlayerTurn].navigators.Count - 1);
                if (navigator.owner.tribe == (Tribe) GamePlayManager.PlayerTurn) {
                    GamePlayManager.NavigatorKeys.Add(navigator.key);
                }
                CreateMove();
            }
        }

        public void MoveNavigator(Vector3Int newPos, GameObject navigator, bool flipX) {
            Vector3Int oldPos = navigator.GetComponent<NavigatorEntityManager>().GridPosition;
            Vector3 oldpos = navigator.GetComponent<NavigatorEntityManager>().BasePosition;
            Vector3 newpos = Collection.Layers[0].CellToWorld(newPos);
            navigator.GetComponent<NavigatorEntityManager>().GridPosition = newPos;
            navigator.GetComponent<NavigatorEntityManager>().BasePosition = newpos;
            var flip = navigator.transform.GetChild(0).GetComponent<SpriteRenderer>().flipX;
            if (flipX) {
                flip = !flip;
            }
            navigator.transform.GetChild(0).GetComponent<SpriteRenderer>().flipX = flip;
            navigator.GetComponent<NavigatorEntityManager>().MoveNavigatorEntity(oldpos, newpos, navigator);

        }

        public static void CreateMove() {
            DiceRoll.RemoveHexPos();
            GamePlayManager.TurnMoves++;
            if (GamePlayManager.TurnMoves == GamePlayManager.TurnMaxMoves) {
                GamePlayManager.MovementEnabled = false;
            }
            PlayDataController.SavePlayData();
        }

        public void UpdateNavigatorPosition(Vector3Int oldPos, Vector3Int newPos, GameObject navigator) {
            var flipX = false;
            var flip = navigator.transform.GetChild(0).GetComponent<SpriteRenderer>().flipX;
            if (flip && oldPos.y > newPos.y) {
                flipX = false;
            } else if (!flip && oldPos.y > newPos.y) {
                flipX = true;
            } else if (flip && oldPos.y < newPos.y) {
                flipX = true;
            } else {
                flipX = false;
            }

            if (Map.ChangeNavigatorPosition(oldPos, newPos, flipX) == DestinationEntity.None) {
                MoveNavigator(newPos, navigator, flipX);
                CreateMove();
            }
        }
        public void PlaceNavigator(Navigator navigator, bool flipX = false) {

            var tribeIndex = (int) navigator.owner.tribe;
            var pos = navigator.GridPosition.ToVector3Int();
            var worldPos = Collection.Layers[0].CellToWorld(pos);
            var navigatorsParent = Collection.EntitiesParent[2].transform;
            var navigatorEntity = Collection.NavigatorPrefabs[tribeIndex];
            var navigatorObject = Object.Instantiate(navigatorEntity, worldPos, Quaternion.identity, navigatorsParent);
            var flip = navigatorObject.transform.GetChild(0).GetComponent<SpriteRenderer>().flipX;
            if (flipX) {
                flip = !flip;
            }
            navigatorObject.transform.GetChild(0).GetComponent<SpriteRenderer>().flipX = flip;

            var p = navigatorObject.transform.position;
            navigatorObject.GetComponent<NavigatorEntityManager>().GridPosition = pos;
            navigatorObject.GetComponent<NavigatorEntityManager>().TribeIdentity = navigator.owner.tribe;
            navigatorObject.GetComponent<NavigatorEntityManager>().BasePosition = navigatorObject.transform.position;
            navigatorObject.GetComponent<NavigatorEntityManager>().key = navigator.key;
            if (navigator.owner.tribe == (Tribe) GamePlayManager.PlayerTurn) {
                GamePlayManager.Navigators.Add(navigatorObject);
            }
        }
        public void PlaceNavigators() {
            foreach (var item in Map.Maps.NavigatorsMap) {
                PlaceNavigator(item.Value, item.Value.flipX);
            }
            var player = GameData.PlayData.players[GamePlayManager.PlayerTurn];
            for (int i = 0; i < player.navigators.Count; i++) {
                GamePlayManager.EnableNavButton(i);
            }
            foreach (var item in player.navigators) {
                GamePlayManager.NavigatorKeys.Add(item.Value.key);
            }
        }

        public void PlaceBaseGenerals() {
            foreach (var item in Map.Maps.GeneralsMap) {
                var tribeIndex = (int) item.Value.owner.tribe;
                var pos = item.Key.ToVector3Int();
                var worldPos = Collection.Layers[0].CellToWorld(pos);
                var generalsParent = Collection.EntitiesParent[1].transform;
                var generalEntity = Collection.GeneralPrefabs[tribeIndex];
                var generalObject = Object.Instantiate(generalEntity, worldPos, Quaternion.identity, generalsParent);
            }

        }

        public void CreateNew() {
            Test(); // TODO: remove this if player creation is created;
            var baseCreator = new BasesCreator(this);
            baseCreator.PlaceBases(this.players);
        }

        public void Create() {
            //do refreshing of the map
        }

        public void Render() {
            PaintGround();
            PlaceBaseStructures();
            PlaceBaseGenerals();
            PlaceNavigators();
        }

        public void Test() {
            for (int i = 0; i < 4; i++) {
                var host = i == 0 ? true : false;
                var user = new User();
                user.name = (new Key()).GenerateRandom(7);
                user.username = (new Key()).GenerateRandom(5);
                var player = new Player(user, host);
                player.tribe = (Tribe) i;
                this.players.Add(player);
            }
        }

        //Coroutines
    }
}