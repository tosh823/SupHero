using UnityEngine;
using SupHero.Components.Character;

namespace SupHero.Components.Weapon {

    public class Magazine : Pool<Projectile> {
        
    }

    public class WeaponController : MonoBehaviour {

        public WeaponData weapon; // Data of this weapon
        public int ammo; // Curent amount of ammo
        public bool reloading; // Do we reloading weapon now?

        protected PlayerController owner; // Player-owner
        protected AudioSource audioSource; // AudioSource attached to the weapon

        protected Magazine projectiles; // Storage for pooling

        public virtual void Start() {
            owner = GetComponentInParent<PlayerController>();
            audioSource = GetComponent<AudioSource>();
            projectiles = gameObject.AddComponent<Magazine>();
            projectiles.Init(weapon.ammo);
            ammo = weapon.ammo;
            reloading = false;
        }

        public virtual void Update() {
            
        }

        public virtual bool returnProjectile(Projectile projectile) {
            projectile.transform.position = transform.position;
            projectile.transform.SetParent(transform);
            if (projectiles.push(projectile)) {
                projectile.gameObject.SetActive(false);
                return true;
            }
            else {
                Destroy(projectile.gameObject);
                return false;
            }
        }

        // Check availability of weapon
        // Check additional stuff in subclasses
        public virtual bool canUseWeapon() {
            if (!reloading) return true;
            else return false;
        }

        // Must do a check in PlayerController before shooting
        // Refresh timer and trigger
        public virtual void useWeapon() {
            // Shot!
            trigger();
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
