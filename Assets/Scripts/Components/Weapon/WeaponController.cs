using UnityEngine;
using SupHero.Components.Character;

namespace SupHero.Components.Weapon {
    public class WeaponController : MonoBehaviour {

        public WeaponData weapon; // Data of this weapon
        public int ammo; // Curent amount of ammo
        public bool reloading; // Do we reloading weapon now?

        protected PlayerController owner; // Player-owner
        protected AudioSource audioSource; // AudioSource attached to the weapon
        protected float timeBetweenUsage; // timer to handle rate of usage

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
            // Overdrive this method in children
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
                    audioSource.Stop();
                };
                reload.launch();
                playReloadSound();
            }
        }

        protected virtual void playSound(AudioClip sound, bool inLoop = false) {
            if (sound != null) {
                audioSource.clip = sound;
                audioSource.loop = inLoop;
                audioSource.Play();
            }
        }

        protected virtual void playTriggerSound() {
            playSound(weapon.triggerSound);
        }

        protected virtual void playReloadSound() {
            playSound(weapon.reloadSound, true);
        }
    }
}
