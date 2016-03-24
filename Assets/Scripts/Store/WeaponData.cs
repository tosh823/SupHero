using UnityEngine;
using System.Collections;

namespace SupHero {

    public enum EffectType {
        NONE,
        POISON,
        FIRE,
        STUN,
        SLOWDOWN
    }

    [System.Serializable]
    public struct EffectData {
        public string name;
        public EffectType type;
        public float value;
        public float duration;
    }

    [System.Serializable]
    public class WeaponData {
        // Fields
        public int id;
        public string name = "New weapon";
        public string description = "Default weapon description";

        public float damage = Constants.damageMedium;
        public float rate = Constants.rateNormal;
        public float range = Constants.rangeShort;

        public bool hasEffect = false;
        public EffectData effect;

        public GameObject weaponPrefab;

        public WeaponData() {

        }

        public WeaponData(int id) {
            this.id = id;
        }
    }
}
