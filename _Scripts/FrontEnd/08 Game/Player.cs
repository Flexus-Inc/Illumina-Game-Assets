using System.Collections.Generic;
using Illumina.Models;

namespace Illumina {
    public enum Tribe {
        Tribe1 = 0, Tribe2, Tribe3, Tribe4
        //TODO: Subject to change
    }
    public class Player {
        public static int Count;
        public static List<Player> All = new List<Player>();
        User user;
        bool host = false;
        public Tribe tribe = (Tribe) 0;
        public int turnIndex;
        public Player(User user, bool host = false) {
            this.user = user;
            this.host = host;
            turnIndex = Count;
            All.Add(this);
            Count++;
        }
    }
}