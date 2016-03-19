using UnityEngine;
using System.Collections;

namespace SupHero.Assets {

    [System.Serializable]
    public class WeaponData {
        // Fields
        public int id;
        public string name = "New weapon";
        public string description = "Default weapon description";
        public float damage = Constants.damageMedium;
        public float rate = Constants.rateNormal;
        public float range = Constants.rangeShort;
        public GameObject weaponPrefab;

        public WeaponData() {

        }

        public WeaponData(int id) {
            this.id = id;
        }
    }
}
