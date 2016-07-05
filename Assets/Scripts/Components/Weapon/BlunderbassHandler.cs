using UnityEngine;
using System.Collections.Generic;

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
            // Create list to store each fraction of charge
            List<WeaponProjectile> charge = new List<WeaponProjectile>();
            while (angle < end) {
                Vector3 direction = Quaternion.AngleAxis(angle, Vector3.up) * owner.transform.forward;
                WeaponProjectile projectile = projectiles.popOrCreate(weapon.projectile.prefab.GetComponent<WeaponProjectile>(), barrelEnd.transform.position, Quaternion.identity);
                projectile.gameObject.SetActive(true);
                projectile.transform.parent = owner.transform.parent;
                Physics.IgnoreCollision(projectile.GetComponent<Collider>(), owner.GetComponent<Collider>());
                if (owner.isHero()) {
                    if (owner.shield != null) {
                        Physics.IgnoreCollision(projectile.GetComponent<Collider>(), owner.shield.GetComponent<Collider>());
                    }
                }
                // Ignore collision with other fractions
                foreach (WeaponProjectile other in charge) {
                    Physics.IgnoreCollision(projectile.GetComponent<Collider>(), other.GetComponent<Collider>());
                }
                projectile.gun = this;
                projectile.Launch(barrelEnd.transform.position, direction);
                charge.Add(projectile);
                angle = angle + perShot;
            }
            playTriggerSound();
            ammo--;
            charge.Clear();
        }

        private void disableEffects() {

        }
    }
}
