using UnityEngine;
using System.Collections;
using System;

namespace SupHero.Model {
    public class Hero : Player {

        public float shield;
        public float replenishWaitTime;
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

		public override void applyhealth(float amount) {
			float maxHealth = Data.Instance.mainSettings.hero.health;
			if (health + amount <= maxHealth) {
				health += amount;
			} 
			else health = maxHealth;
		}

        public Hero(Player player) {
            // Copying data
            playerName = player.playerName;
            number = player.number;
            points = player.points;

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
            };
        }

        public DamageResult receiveDamageIgnoreShield(float damage) {
            // If have shield active, make it take dmg on it
            // If have armor, make it take dmg on it
            if (armor > 0) {
                // If damage is too high, take the rest to your body
                if (damage >= armor) {
                    float rest = damage - armor;
                    armor = 0;
                    return receiveDamageIgnoreShield(rest);
                }
                else {
                    armor -= damage;
                    return DamageResult.ARMOR_HIT;
                }
            }
            // Else take damage directly to your body
            else {
                // If damage is too high, accept your death
                if (damage >= health) {
                    health = 0;
                    isAlive = false;
                    return DamageResult.MORTAL_HIT;
                }
                else {
                    health -= damage;
                    return DamageResult.HEALTH_HIT;
                }
            }
        }

        public bool consumeShield(float value) {
            if (!isShieldEmpty) {
                if ((shield - value) > 0f) {
                    shield -= value;
                }
                else shield = 0f;
                return true;
            }
            else return false;
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

            isAlive = true;
        }

        public override void die() {
            isAlive = false;
        }

        public override void born() {
            health = Data.Instance.mainSettings.guard.health;
            armor = Data.Instance.mainSettings.guard.armor;
            shield = Data.Instance.mainSettings.hero.shield;
            replenishWaitTime = Data.Instance.mainSettings.hero.shieldReplenishTime;
            speed = Data.Instance.mainSettings.guard.speed;
            isAlive = true;
            isStunned = false;
            haveTakenDamage = false;
        }
    }
}
