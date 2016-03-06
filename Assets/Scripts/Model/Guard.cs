using UnityEngine;
using System.Collections;
using System;

namespace SupHero.Model {
    public class Guard : Player {

        public Guard(int number = 0) : base(number) {
            setupDefaultProperties();
        }

        public Guard(Player player) {
            // Copying needed values
            playerName = player.playerName;
            number = player.number;
            points = player.points;
            // Input properties
            inputType = player.inputType;
            gamepadNumber = player.gamepadNumber;
            gamepadName = player.gamepadName;

            setupDefaultProperties();
        }

        public override void die() {
            health = Constants.health;
            armor = Constants.armor;
        }

        public override void resurrect() {
            setupDefaultProperties();
        }

        protected override void setupDefaultProperties() {
            health = Constants.health;
            armor = Constants.armor;
            speed = Constants.guardSpeed;
            isAlive = true;
        }
    }
}
