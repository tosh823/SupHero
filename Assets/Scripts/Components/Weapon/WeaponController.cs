using UnityEngine;
using SupHero.Components.Character;
using SupHero.Model;

namespace SupHero.Components.Weapon {

    public struct WeaponAnimState {
        public static string TRIGGER = "trigger";
        public static string RELOAD = "reload";
    }

    public struct EffectAnimState {
        public static string RUN = "run";
    }

    public class Magazine : Pool<WeaponProjectile> {
        
    }

    public class WeaponController : MonoBehaviour {

        public PlayerController owner; // Player-owner
        public WeaponData weapon; // Data of this weapon
        public Magazine projectiles; // Storage for pooling
        public int ammo; // Curent amount of ammo
        public bool reloading; // Do we reloading weapon now?
        
        protected AudioSource audioSource; // AudioSource attached to the weapon

        // Events
        public delegate void onDrawAction();
        public event onDrawAction OnDraw;
        public delegate void onHideAction();
        public event onHideAction OnHide;
        public delegate void onUnEquipAction();
        public event onUnEquipAction OnUnEquip;
        public delegate void onTriggerAction();
        public event onTriggerAction OnTrigger;
        public delegate void onReloadStartAction();
        public event onReloadStartAction OnReloadStart;

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

        public virtual bool returnProjectile(WeaponProjectile projectile) {
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

        public virtual void drawWeapon() {
            if (OnDraw != null) OnDraw();
            gameObject.SetActive(true);
        }

        public virtual void hideWeapon() {
            if (OnHide != null) OnHide();
            gameObject.SetActive(false);
        }

        public virtual void unequipWeapon() {
            if (OnUnEquip != null) OnUnEquip();
            Destroy(gameObject);
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
            if (OnTrigger != null) OnTrigger();
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
                if (OnReloadStart != null) OnReloadStart();
                reload.Launch();
                playReloadSound();
            }
        }

        public virtual void dealDamageTo(PlayerController pc, bool ignoreShield = false) {
            if (pc != null) {
                DamageResult result = pc.receiveDamage(weapon.damage, ignoreShield);
                if (result == DamageResult.MORTAL_HIT) {
                    if ((owner.player is Guard) && (pc.player is Hero)) {
                        owner.player.applyPoints(Data.Instance.mainSettings.points.fragHero);
                    }
                    if ((owner.player is Hero) && (pc.player is Guard)) {
                        owner.player.applyPoints(Data.Instance.mainSettings.points.fragGuard);
                    }
                }
            }
        }

        protected virtual void playSound(AudioClip sound, bool inLoop = false) {
            if (sound != null) {
                audioSource.clip = sound;
                audioSource.loop = inLoop;
                audioSource.Play();
            }
        }

        public virtual void playTriggerSound() {
            playSound(weapon.triggerSound);
        }

        public virtual void playReloadSound() {
            playSound(weapon.reloadSound, true);
        }
    }
}
