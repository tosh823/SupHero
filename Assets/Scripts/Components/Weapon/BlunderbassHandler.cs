using UnityEngine;
using System.Collections;

namespace SupHero.Components.Weapon {
    public class BlunderbassHandler : WeaponController {

        public Transform barrelEnd;
        public GameObject flashPrefab;
        public int fractionAmount; // Number of frags in one shot
        public float scatter; // Degrees of scatter

        public override void Start() {
            base.Start();
            projectiles.Init(4 * weapon.ammo);
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
            // Muzzleflash
            GameObject flash = Instantiate(flashPrefab) as GameObject;
            flash.transform.position = barrelEnd.transform.position;
            flash.transform.rotation = Quaternion.LookRotation(barrelEnd.transform.forward, barrelEnd.transform.up);
            flash.transform.SetParent(transform);
            // Shot
            float perShot = scatter / fractionAmount;
            float start = -scatter / 2f;
            float angle = start;
            float end = scatter / 2f + 1;
            while (angle < end) {
                Vector3 direction = Quaternion.AngleAxis(angle, Vector3.up) * owner.transform.forward;
                WeaponProjectile instance = projectiles.popOrCreate(weapon.projectile.prefab.GetComponent<WeaponProjectile>(), barrelEnd.transform.position, Quaternion.identity);
                instance.gameObject.SetActive(true);
                instance.transform.parent = null;
                Physics.IgnoreCollision(instance.GetComponent<Collider>(), owner.GetComponent<Collider>());
                if (owner.isHero()) {
                    if (owner.shield != null) {
                        Physics.IgnoreCollision(instance.GetComponent<Collider>(), owner.shield.GetComponent<Collider>());
                    }
                }
                instance.gun = this;
                instance.Launch(barrelEnd.transform.position, direction);
                angle = angle + perShot;
            }
            playTriggerSound();
            ammo--;
        }

        private void disableEffects() {

        }
    }
}
