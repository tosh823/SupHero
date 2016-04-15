﻿using UnityEngine;
using System.Collections;

namespace SupHero.Components.Weapon {
    public class SidearmHandler : WeaponController {

        public Transform barrelEnd;

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
            Projectile instance = projectiles.popOrCreate(weapon.projectile.prefab.GetComponent<Projectile>(), barrelEnd.transform.position, Quaternion.identity);
            instance.gameObject.SetActive(true);
            instance.transform.parent = null;
            Physics.IgnoreCollision(instance.GetComponent<Collider>(), owner.GetComponent<Collider>());
            instance.gun = this;
            instance.Launch(barrelEnd.transform.position, owner.transform.forward);
            playTriggerSound();
            ammo--;
        }

        private void disableEffects() {
            
        }
    }
}
