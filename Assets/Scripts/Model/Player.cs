using UnityEngine;
using System.Collections;

namespace SupHero.Model {
    public abstract class Player {

        public int armor { get; protected set; }
        public int health { get; protected set; }
        public float speed { get; protected set; }
        public ItemSlot[] itemSlots = new ItemSlot[2];
        public string playerName;
        protected bool isAlive;

        public InputType inputType;
        public int gamepadNumber;
        public string gamepadName;
        public int number;
        public int points;

        public Player(int number = 0) {
            this.number = number;
            isAlive = true;
            playerName = string.Format("{0} {1}", "Player", this.number);
            points = 0;
        }

        protected abstract void setupDefaultProperties();

        public virtual void takeDamage(int damage) {
            // If have armor, make it take dmg on it
            if (armor > 0) {
                // If damage is too high, take the rest to your body
                if (damage >= armor) {
                    int rest = damage - armor;
                    armor = 0;
                    takeDamage(rest);
                }
                else armor -= damage;
            }
            // Else take damage directly to your body
            else {
                // If damage is too high, accept your death
                if (damage >= health) {
                    health = 0;
                    isAlive = false;
                }
                else health -= damage;
            }
        }
    }
}
