using UnityEngine;
using SupHero.Model;
using SupHero.Components.Character;

namespace SupHero.Components.Weapon {
    public class PistolHandler : WeaponController {

        public GameObject barrelEnd;

        private LineRenderer lazer;
        private Ray shootRay;
        private RaycastHit shootHit;

        public override void Start() {
            base.Start();
            lazer = GetComponentInChildren<LineRenderer>();
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
            lazer.enabled = true;
            lazer.SetPosition(0, transform.position);
            shootRay.origin = barrelEnd.transform.position;
            shootRay.direction = owner.transform.forward;
            lazer.SetPosition(1, shootRay.origin + shootRay.direction * weapon.range);

            if (Physics.Raycast(shootRay, out shootHit, weapon.range)) {
                GameObject target = shootHit.transform.gameObject;
                lazer.SetPosition(1, shootHit.point);
                if (target.CompareTag(Tags.Player)) {
                    DamageResult result = target.GetComponent<PlayerController>().receiveDamage(weapon.damage, false);
                    if (weapon.hasEffect) target.GetComponent<PlayerController>().applyEffect(weapon.effect);
                    if (result == DamageResult.MORTAL_HIT) {
                        owner.player.applyPoints(10);
                    }
                }
                else if (target.CompareTag(Tags.Cover)) {
                    target.GetComponent<BaseDestructable>().takeDamage(weapon.damage);
                }
            }

            playTriggerSound();
            ammo--;

            Invoke(Utils.getActionName(disableEffects), 0.05f);
        }

        private void disableEffects() {
            lazer.enabled = false;
        }
    }
}
