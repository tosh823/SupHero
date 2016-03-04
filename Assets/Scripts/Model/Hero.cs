using UnityEngine;
using System.Collections;
using System;

namespace SupHero.Model {
    public class Hero : Player {

        public int shield { get; protected set; }

        public Hero(int number = 0) : base(number) {
            setupDefaultProperties();
        }

        public Hero(Player player) {
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

        public override void takeDamage(int damage) {
            // If have shield active, make it take dmg on it
            if (shield > 0) {
                // If damage is too high, take the rest to your body
                if (damage >= shield) {
                    int rest = damage - shield;
                    shield = 0;
                    takeDamage(rest);
                }
                else shield -= damage;
            }
            // Else take damage as normal
            else {
                base.takeDamage(damage);
            }
        }

        protected override void setupDefaultProperties() {
            isAlive = true;
            health = Constants.defaultHealth;
            armor = Constants.defaultArmor;
            shield = Constants.defaultShield;
            speed = Constants.defaultHeroSpeed;
        }

        public override void die() {
            health = Constants.defaultHealth;
            armor = Constants.defaultArmor;
        }

        public override void resurrect() {
            setupDefaultProperties();
        }
    }
}
