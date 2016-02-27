using UnityEngine;
using System.Collections;

namespace SupHero.Model {
    public class Hero : Player {

        private int shield = Constants.defaultShield;

        public Hero(int number) : base(number) {
            playerName = "Hero";
        }
    }
}
