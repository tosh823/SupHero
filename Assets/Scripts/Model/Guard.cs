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
            // Item
            firstItemId = player.firstItemId;
            secondItemId = player.secondItemId;

            character = player.character;

            // Input properties
            inputType = player.inputType;
            gamepadNumber = player.gamepadNumber;
            gamepadName = player.gamepadName;

            setupDefaultProperties();
        }

		public override void applyhealth(float amount) {
			float maxHealth = Data.Instance.mainSettings.guard.health;
			if (health + amount <= maxHealth) {
				health += amount;
			} 
			else health = maxHealth;
		}

        public override void die() {
            isAlive = false;
        }

        public override void born() {
            health = Data.Instance.mainSettings.guard.health;
            armor = Data.Instance.mainSettings.guard.armor;
            speed = Data.Instance.mainSettings.guard.speed;
            isAlive = true;
            isStunned = false;
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
