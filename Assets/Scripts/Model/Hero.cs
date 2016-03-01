using UnityEngine;
using System.Collections;

namespace SupHero.Model {
    public class Hero : Player {

        private int shield = Constants.defaultShield;

        public Hero(int number = 0) : base(number) {
            
        }

        public Hero(Player player) {
            playerName = player.playerName;
            number = player.number;
            points = player.points;
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
    }
}
