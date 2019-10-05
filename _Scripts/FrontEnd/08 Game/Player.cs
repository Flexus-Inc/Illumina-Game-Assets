using System.Collections.Generic;
using Illumina.Models;

namespace Illumina {
    public enum Tribe {
        Takipsilim = 0, Paraluman, Siklab, Kalinaw
    }

    [System.Serializable]
    public class Player {
        public static int Count;
        public static List<Player> All = new List<Player>();
        public Dictionary<string, Navigator> navigators = new Dictionary<string, Navigator>();
        //Remove when navigator dies
        //Add when summoned
        public Dictionary<string, Trap> traps = new Dictionary<string, Trap>();
        //Remove when detonated, or get by other players;
        //Add when answered a trap message
        User user;
        bool host = false;
        //first player is always the host
        public Tribe tribe = (Tribe) 0;
        public int turnIndex;

        public Player(User user, bool host = false) {
            this.user = user;
            this.host = host;
            turnIndex = Count;
            All.Add(this);
            Count++;
            navigators = new Dictionary<string, Navigator>();
        }
    }
}