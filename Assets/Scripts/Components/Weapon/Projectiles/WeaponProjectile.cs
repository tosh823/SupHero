using UnityEngine;
using SupHero.Components.Character;
using SupHero.Model;

namespace SupHero.Components.Weapon {
    public class WeaponProjectile : BaseProjectile {
        
        public WeaponController gun;
        public GameObject hitEffect;

        public override void Start() {
            base.Start();
        }

        // If trigger
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
                    gun.dealDamageTo(target.GetComponent<PlayerController>());
                    Stop();
                }
            }
            else {
                Stop();
            }
        }

        // If collider
        void OnCollisionEnter(Collision collision) {
            GameObject target = collision.gameObject;
            // Effect
            makeHit(collision.contacts[0]);
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
                    gun.dealDamageTo(target.GetComponent<PlayerController>());
                    Stop();
                }
            }
            else {
                Stop();
            }
        }

        protected virtual void makeHit(ContactPoint contact) {
            Quaternion rot = Quaternion.FromToRotation(Vector3.up, contact.normal);
            Vector3 pos = contact.point;
            Instantiate(hitEffect, pos, rot);
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
