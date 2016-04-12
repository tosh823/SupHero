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

    public enum WeaponSlot {
        PRIMARY,
        SECONDARY
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
        public WeaponSlot slot;

        public GameObject prefab;
        public AnimatorOverrideController controller;
        public AnimatorOverrideController guardVersion;

        public float damage = Constants.damageMedium;
        public float rate = Constants.rateNormal;
        public float range = Constants.rangeShort;
        public int ammo = Constants.capacityMedium;
        public float reloadTime = Constants.reloadTime;

        public bool hasEffect = false;
        public EffectData effect;

        public AudioClip triggerSound;
        public AudioClip reloadSound;

        public WeaponData() {

        }

        public WeaponData(int id) {
            this.id = id;
        }
    }
}
