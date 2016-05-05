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
                return (shield / Data.Instance.mainSettings.hero.shield) * 100;
            }
        }
        public bool isShieldFull {
            get {
                if (shield >= Data.Instance.mainSettings.hero.shield) return true;
                else return false;
            }
        }
        public bool isShieldEmpty {
            get {
                if (shield <= 0f) return true;
                else return false;
            }
        }

        public Hero(int number = 0) : base(number) {
            setupDefaultProperties();
        }

        public Hero(Player player) {
            // Copying data
            playerName = player.playerName;
            number = player.number;
            points = player.points;

            // Weapon
            primaryId = player.primaryId;
            secondaryId = player.secondaryId;

            // Input properties
            inputType = player.inputType;
            gamepadNumber = player.gamepadNumber;
            gamepadName = player.gamepadName;

            setupDefaultProperties();
        }

        public override DamageResult receiveDamage(float damage) {
            // If have shield active, make it take dmg on it
            haveTakenDamage = true;
            if (shield > 0) {
                // If damage is too high, take the rest to your body
                if (damage >= shield) {
                    float rest = damage - shield;
                    shield = 0;
                    return receiveDamage(rest);
                }
                else {
                    shield -= damage;
                    return DamageResult.SHIELD_HIT;
                }
            }
            // Else take damage as normal
            else {
                return base.receiveDamage(damage);
            }
        }

        public void replenishShield() {
            if (!isShieldFull) {
                shield++;
            }
        }

        protected override void setupDefaultProperties() {
            health = Data.Instance.mainSettings.hero.health;
            armor = Data.Instance.mainSettings.hero.armor;
            shield = Data.Instance.mainSettings.hero.shield;
            replenishWaitTime = Data.Instance.mainSettings.hero.shieldReplenishTime;
            speed = Data.Instance.mainSettings.hero.speed;
            haveTakenDamage = false;
            isStunned = false;

            primaryId = Data.Instance.mainSettings.hero.starterPrimary;
            secondaryId = Data.Instance.mainSettings.guard.starterSecondary;

            isAlive = true;
        }

        public override void die() {
            isAlive = false;
        }

        public override void resurrect() {
            setupDefaultProperties();
        }
    }
}
