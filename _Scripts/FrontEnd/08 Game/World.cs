namespace Illumina {

    public class World {
        public WorldCollection Collection;
        public WorldMap Map;

        public World(WorldCollection collection) {
            Collection = collection;
            Map = new WorldMap();
        }


    }
}