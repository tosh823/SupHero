using UnityEngine;
using System.Collections;

namespace SupHero.Components {
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
            playTriggerSound();
            ammo--;
        }

        private void disableEffects() {
            
        }
    }
}
