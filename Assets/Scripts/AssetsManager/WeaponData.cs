using UnityEngine;
using System.Collections;

namespace SupHero.Assets {

    [System.Serializable]
    public class WeaponData {
        // Fields
        public string name;
        public string description;
        public float damage;
        public float rate;
        public GameObject weaponPrefab;
    }
}
