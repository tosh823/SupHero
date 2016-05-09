﻿using UnityEngine;
using SupHero.Components.Character;
using SupHero.Model;

namespace SupHero.Components.Weapon {
    public class WeaponProjectile : BaseProjectile {
        
        public WeaponController gun;

        public override void Start() {
            base.Start();
        }

        void OnTriggerEnter(Collider other) {
            GameObject target = other.gameObject;
            if (target.CompareTag(Tags.Player)) {
                gun.dealDamageTo(target.GetComponent<PlayerController>());
                Stop();
            }
            else if (target.CompareTag(Tags.Cover)) {
                target.GetComponent<CoverController>().takeDamage(gun.weapon.damage);
                Stop();
            }
            else if (target.CompareTag(Tags.Shield)) {
                if (gun.owner.player is Guard) {
                    Debug.Log("Hit " + target);
                    gun.dealDamageTo(target.GetComponent<PlayerController>());
                    Stop();
                }
            }
            else {
                Stop();
            }
        }

        public override void Update() {
            base.Update();
            if (launched && distanceTraveled >= (3 * gun.weapon.range)) {
                Stop();
            }
        }

        public void Launch(Vector3 start, Vector3 direction) {
            speed = gun.weapon.projectile.speed;
            base.Launch(start, direction, speed);
        }

        public override void Stop() {
            base.Stop();
            if (gun != null) gun.returnProjectile(this);
            else Destroy(gameObject);
        }
    }
}
