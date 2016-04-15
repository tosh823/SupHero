using UnityEngine;
using System.Collections;
using SupHero.Components.Character;
using SupHero.Model;

namespace SupHero.Components.Weapon {
    public class Projectile : MonoBehaviour {

        public float speed;
        private bool launched = false;
        public WeaponController gun;
        private Vector3 direction;
        private Vector3 initialPosition;
        private float madeDistance {
            get {
                return Vector3.Distance(transform.position, initialPosition);
            }
        }

        public virtual void Start() {
            
        }

        public virtual void OnTriggerEnter(Collider other) {
            GameObject target = other.gameObject;
            if (target.CompareTag(Tags.Player)) {
                DamageResult hit = target.GetComponent<PlayerController>().receiveDamage(gun.weapon.damage);
            }
            else if (target.CompareTag(Tags.Cover)) {
                target.GetComponent<CoverController>().takeDamage(gun.weapon.damage);
            }
            Stop();
        }

        public virtual void OnCollisionEnter(Collision collision) {
            Stop();
        }

        public virtual void Update() {
            if (launched) {
                transform.Translate(direction * speed * Time.deltaTime);
                if (madeDistance >= 2*gun.weapon.range) {
                    Stop();
                }
            }
        }

        public virtual void Launch(Vector3 start, Vector3 direction) {
            initialPosition = start;
            this.direction = direction;
            launched = true;
            gameObject.SetActive(true);
        }

        public virtual void Stop() {
            launched = false;
            gameObject.SetActive(false);
            if (!gun.returnProjectile(this)) Destroy(gameObject);
        }

        void OnBecameVisible() {
            
        }

        void OnBecameInvisible() {
            if (launched) {
                Debug.Log("Projectile becomes invisible");
                Stop();
            }
        }
    }
}
