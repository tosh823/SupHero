using UnityEngine;
using System.Collections;

namespace SupHero.Model {
    public class Hero : Player {

        private int shield = Constants.defaultShield;

        public Hero(int number = 0) : base(number) {
            
        }

        public Hero(Player player) {
            playerName = player.playerName;
            number = player.number;
        }
    }
}
