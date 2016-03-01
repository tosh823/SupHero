using UnityEngine;
using System.Collections;

namespace SupHero.Model {
    public class Guard : Player {

        public Guard(int number = 0) : base(number) {
            
        }

        public Guard(Player player) {
            playerName = player.playerName;
            number = player.number;
            points = player.points;
        }
    }
}
