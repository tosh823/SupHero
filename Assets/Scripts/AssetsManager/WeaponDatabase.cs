using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace SupHero.Assets {
    public class WeaponDatabase : ScriptableObject {
        
        // Storage
        public List<WeaponData> weapons;

        // Methods
        public void add(WeaponData weapon) {
            weapons.Add(weapon);
        }

        public void add() {
            WeaponData weapon = new WeaponData();
            weapons.Add(weapon);
        }

        public WeaponData getWeaponAtIndex(int index) {
            return weapons[index];
        }

        public void removeWeaponAtIndex(int index) {
            weapons.RemoveAt(index);
        }
    }
}
