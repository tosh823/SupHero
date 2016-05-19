using UnityEngine;
using System.Collections;

namespace SupHero.Components.Weapon {
    public class DuetteHandler : WeaponController {

        public Transform barrelEnd;
        public GameObject flashPrefab;

        public override void Start() {
            base.Start();
        }

        public override void Update() {
            base.Update();
        }

        public override bool canUseWeapon() {
            bool haveAmmo = (ammo > 0);
            if (!haveAmmo) reload();
            return (haveAmmo && base.canUseWeapon());
        }

        protected override void trigger() {
            // Flashmuzzle
            GameObject flash = Instantiate(flashPrefab) as GameObject;
            flash.transform.position = barrelEnd.transform.position;
            flash.transform.rotation = Quaternion.LookRotation(barrelEnd.transform.forward, barrelEnd.transform.up);
            flash.transform.SetParent(transform);
            // Shot
            WeaponProjectile instance = projectiles.popOrCreate(weapon.projectile.prefab.GetComponent<WeaponProjectile>(), barrelEnd.transform.position, Quaternion.identity);
            instance.gameObject.SetActive(true);
            instance.transform.parent = owner.transform.parent;
            Physics.IgnoreCollision(instance.GetComponent<Collider>(), owner.GetComponent<Collider>());
            if (owner.isHero()) {
                if (owner.shield != null) {
                    Physics.IgnoreCollision(instance.GetComponent<Collider>(), owner.shield.GetComponent<Collider>());
                }
            }
            instance.gun = this;
            instance.Launch(barrelEnd.transform.position, owner.transform.forward);
            playTriggerSound();
            ammo--;
            // Triggering left weapon
            base.trigger();
        }

        private void disableEffects() {

        }
    }
}
