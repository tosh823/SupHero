using UnityEngine;
using System.Collections;

namespace SupHero.Model {
    public class Player {

        protected int armor = Constants.defaultArmor;
        protected int health = Constants.defaultHealth;
        ItemSlot[] itemSlots = new ItemSlot[2];
        public string playerName = "Default name";
        protected bool isAlive = true;

        public InputType inputType;
        public int number;

        public Player(int number) {
            this.number = number;
        }

        public void takeDamage(int damage) {
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
