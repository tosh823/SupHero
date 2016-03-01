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

        protected override void setupDefaultProperties() {
            health = Constants.defaultHealth;
            armor = Constants.defaultArmor;
            speed = Constants.defaultGuardSpeed;
        }
    }
}
