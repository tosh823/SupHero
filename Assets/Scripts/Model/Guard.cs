using UnityEngine;
using System.Collections;
using System;

namespace SupHero.Model {
    public class Guard : Player {

        public Guard(int number = 0) : base(number) {
            setupDefaultProperties();
        }

        public Guard(Player player) {
            // Copying data
            playerName = player.playerName;
            number = player.number;
            points = player.points;

            // Properties
            health = player.health;
            armor = player.armor;
            speed = player.speed;

            // Weapon
            primaryId = player.primaryId;
            secondaryId = player.secondaryId;

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
            health = Data.Instance.mainSettings.guard.health;
            armor = Data.Instance.mainSettings.guard.armor;
            speed = Data.Instance.mainSettings.guard.speed;

            primaryId = Data.Instance.mainSettings.guard.starterPrimary;
            secondaryId = Data.Instance.mainSettings.guard.starterSecondary;

            isAlive = true;
            isStunned = false;
        }
    }
}
