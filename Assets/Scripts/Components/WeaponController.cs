using UnityEngine;
using System.Collections;
using SupHero;
using SupHero.Model;

namespace SupHero.Components {
    public class WeaponController : MonoBehaviour {

        public WeaponData weapon; // Data of this weapon

        protected PlayerController owner; // Player-owner
        protected float timeBetweenUsage; // timer to handle rate of usage

        public virtual void Start() {
            owner = GetComponentInParent<PlayerController>();
            timeBetweenUsage = weapon.rate;
        }

        public virtual void Update() {
            if (timeBetweenUsage < weapon.rate) {
                timeBetweenUsage += Time.deltaTime;
            }
        }

        // Check availability of weapon
        // Check rate, ammo, etc.
        public bool canUseWeapon() {
            if (timeBetweenUsage >= weapon.rate) return true;
            else return false;
        }

        // 
        public virtual void useWeapon() {
            if (canUseWeapon()) {
                timeBetweenUsage = 0f;
                // Shot!
                trigger();
            }
        }

        protected virtual void trigger() {
            // Overdrive this method in childred
            // to add custom behavior on using
        }
    }
}
