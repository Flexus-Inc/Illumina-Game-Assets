using Illumina.Models;

namespace Illumina.Models {

    [System.Serializable]
    public class LobbyRegistrant : IlluminaModel {
        public string username;
    }
    public class LobbyRegistree : IlluminaModel {
        public string username;
        public int hostid;
    }

    public class LobbyRoom : IlluminaModel {
        public int hostid;
        public int status;
        public User[] users;
    }

}