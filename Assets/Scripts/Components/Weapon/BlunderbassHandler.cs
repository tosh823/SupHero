using UnityEngine;
using System.Collections;

namespace SupHero.Components.Weapon {
    public class BlunderbassHandler : WeaponController {

        public Transform barrelEnd;
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
            float perShot = scatter / fractionAmount;
            float start = -scatter / 2f;
            float angle = start;
            float end = scatter / 2f + 1;
            while (angle < end) {
                Vector3 direction = Quaternion.AngleAxis(angle, Vector3.up) * owner.transform.forward;
                Projectile instance = projectiles.popOrCreate(weapon.projectile.prefab.GetComponent<Projectile>(), barrelEnd.transform.position, Quaternion.identity);
                instance.gameObject.SetActive(true);
                instance.transform.parent = null;
                Physics.IgnoreCollision(instance.GetComponent<Collider>(), owner.GetComponent<Collider>());
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
