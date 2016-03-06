using UnityEngine;
using System.Collections;
using System;

namespace SupHero.Model {
    public class Hero : Player {

        public float shield { get; protected set; }
        public float replenishWaitTime { get; protected set; }
        public bool haveTakenDamage;
        public float shieldPercentage {
            get {
                return (shield / Constants.shield) * 100;
            }
        }
        public bool isShieldFull {
            get {
                if (shield >= Constants.shield) return true;
                else return false;
            }
        }

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

        public override DamageResult takeDamage(float damage) {
            // If have shield active, make it take dmg on it
            haveTakenDamage = true;
            if (shield > 0) {
                // If damage is too high, take the rest to your body
                if (damage >= shield) {
                    float rest = damage - shield;
                    shield = 0;
                    return takeDamage(rest);
                }
                else {
                    shield -= damage;
                    return DamageResult.SHIELD_HIT;
                }
            }
            // Else take damage as normal
            else {
                return base.takeDamage(damage);
            }
        }

        public void replenishShield() {
            if (!isShieldFull) {
                shield++;
            }
        }

        protected override void setupDefaultProperties() {
            health = Constants.health;
            armor = Constants.armor;
            shield = Constants.shield;
            replenishWaitTime = Constants.replenishWaitTime;
            speed = Constants.heroSpeed;
            haveTakenDamage = false;
            isAlive = true;
        }

        public override void die() {
            shield = Constants.shield;
            health = Constants.health;
            armor = Constants.armor;
        }

        public override void resurrect() {
            setupDefaultProperties();
        }
    }
}
