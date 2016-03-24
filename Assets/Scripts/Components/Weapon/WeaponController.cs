using UnityEngine;
using System.Collections;
using SupHero;
using SupHero.Model;

namespace SupHero.Components {
    public class WeaponController : MonoBehaviour {

        public WeaponData weapon; // Data of this weapon
        public int ammo; // Curent amount of ammo
        public bool reloading; // Do we reloading weapon now?

        protected PlayerController owner; // Player-owner
        protected float timeBetweenUsage; // timer to handle rate of usage
        protected AudioSource audioSource; // AudioSource attached to the weapon

        public virtual void Start() {
            owner = GetComponentInParent<PlayerController>();
            audioSource = GetComponent<AudioSource>();
            timeBetweenUsage = weapon.rate;
            ammo = weapon.ammo;
            reloading = false;
        }

        public virtual void Update() {
            if (timeBetweenUsage < weapon.rate) {
                timeBetweenUsage += Time.deltaTime;
            }
        }

        // Check availability of weapon
        // Check additional stuff in subclasses
        public virtual bool canUseWeapon() {
            bool ready = (timeBetweenUsage >= weapon.rate); // Is enough time went since last usage
            if (ready && !reloading) return true;
            else return false;
        }

        // Try to trigger
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

            // ATTENTION: Assuming all checks
            // are made before trigger
        }

        // Reload the weapon
        public virtual void reload() {
            // If weapon has more the 0 ammo initialy
            // Then I suppose it could be reload?
            if (weapon.ammo != 0 && !reloading) {
                Timer reload = gameObject.AddComponent<Timer>();
                reload.time = weapon.reloadTime;
                reloading = true;
                reload.OnEnd += delegate () {
                    ammo = weapon.ammo;
                    reloading = false;
                };
                reload.launch();
                playReloadSound();
            }
        }

        protected virtual void playTriggerSound() {
            if (weapon.triggerSound != null) {
                audioSource.clip = weapon.triggerSound;
                audioSource.Play();
            }
        }

        protected virtual void playReloadSound() {
            if (weapon.reloadSound != null) {
                audioSource.clip = weapon.reloadSound;
                audioSource.Play();
            }
        }
    }
}
