using UnityEngine;
using System.Collections;

namespace SupHero.Model {

    public enum DamageResult {
        SHIELD_HIT,
        ARMOR_HIT,
        HEALTH_HIT,
        MORTAL_HIT,
        NONE,
    }

    [System.Serializable]
    public abstract class Player {

        public float armor { get; set; }
        public float health { get; set; }
        public float speed { get; set; }
        public bool isAlive { get; set; }
        public int points { get; set; }
        public int number { get; set; }
        public int primaryId { get; set; }
        public int secondaryId { get; set; }
        public int firstItemId { get; set; }
        public int secondItemId { get; set; }

        public CharacterData character;

        public string playerName;

        public bool isStunned;

        public InputType inputType;
        public int gamepadNumber;
        public string gamepadName;

        public Player(int number = 0) {
            this.number = number;
            isAlive = true;
            isStunned = false;
            playerName = string.Format("{0} {1}", "Player", this.number);
            points = 0;
        }

        protected abstract void setupDefaultProperties();
		public abstract void applyhealth(float amount);
        public abstract void die();
        public abstract void born();

        public virtual void applyPoints(int amount) {
            points += amount;
        }
			
        public virtual DamageResult receiveDamage(float damage) {
            // If have armor, make it take dmg on it
            if (armor > 0) {
                // If damage is too high, take the rest to your body
                if (damage >= armor) {
                    float rest = damage - armor;
                    armor = 0;
                    return receiveDamage(rest);
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
    }
}
