using UnityEngine;
using System.Collections;

namespace SupHero {
    public class Hero : Player {

        private int shield = Constants.defaultShield;

        public Hero(int number) : base(number) {
            playerName = "Hero";
        }
    }
}
