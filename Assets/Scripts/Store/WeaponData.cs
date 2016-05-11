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
        SECONDARY,
        NONE
    }

    [System.Serializable]
    public struct EffectData {
        public string name;
        public EffectType type;
        public float value;
        public float duration;
    }

    [System.Serializable]
    public struct ProjectileData {
        public string name;
        public float speed;
        public GameObject prefab;
    }

    public enum WeaponType {
        RIGHT_HAND,
        LEFT_HAND,
        BOTH
    }

    [System.Serializable]
    public class WeaponData {
        // Fields
        public int id;
        public string name = "New weapon";
        public string description = "Default weapon description";
        public WeaponSlot slot;
        public WeaponType weaponType;

        public GameObject prefab;
        public AnimatorOverrideController heroController;
        public AnimatorOverrideController guardController;

        public float damage = Constants.damageMedium;
        public float rate = Constants.rateNormal;
        public float range = Constants.rangeShort;
        public int ammo = Constants.capacityMedium;
        public float reloadTime = Constants.reloadTime;

        public bool hasEffect = false;
        public EffectData effect;

        public ProjectileData projectile;

        public AudioClip triggerSound;
        public AudioClip reloadSound;

        public Sprite image;

        public WeaponData() {

        }

        public WeaponData(int id) {
            this.id = id;
        }
    }
}
