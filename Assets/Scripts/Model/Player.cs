using UnityEngine;
using System.Collections;

namespace SupHero.Model {

    public enum DamageResult {
        SHIELD_HIT,
        ARMOR_HIT,
        HEALTH_HIT,
        MORTAL_HIT
    }

    public abstract class Player {

        public float armor { get; protected set; }
        public float health { get; protected set; }
        public float speed { get; protected set; }
        public bool isAlive { get; protected set; }
        public int points { get; protected set; }
        public int number { get; protected set; }

        public ItemSlot[] itemSlots = new ItemSlot[2];
        public string playerName;

        public InputType inputType;
        public int gamepadNumber;
        public string gamepadName;

        public Player(int number = 0) {
            this.number = number;
            isAlive = true;
            playerName = string.Format("{0} {1}", "Player", this.number);
            points = 0;
        }

        protected abstract void setupDefaultProperties();
        public abstract void die();
        public abstract void resurrect();

        public virtual void applyPoints(int amount) {
            points += amount;
        }

        public virtual DamageResult takeDamage(float damage) {
            // If have armor, make it take dmg on it
            if (armor > 0) {
                // If damage is too high, take the rest to your body
                if (damage >= armor) {
                    float rest = damage - armor;
                    armor = 0;
                    return takeDamage(rest);
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
