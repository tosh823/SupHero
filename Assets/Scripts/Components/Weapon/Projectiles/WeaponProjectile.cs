using UnityEngine;
using SupHero.Components.Character;
using SupHero.Model;

namespace SupHero.Components.Weapon {
    public class WeaponProjectile : MonoBehaviour {
        
        public float speed;
        protected bool launched = false;
        public WeaponController gun;
        protected Vector3 direction;
        protected Vector3 initialPosition;
        protected float madeDistance {
            get {
                return Vector3.Distance(transform.position, initialPosition);
            }
        }

        public virtual void Start() {
            
        }

        public virtual void OnTriggerEnter(Collider other) {
            GameObject target = other.gameObject;
            if (target.CompareTag(Tags.Player)) {
                Debug.Log("Hit " + target);
                gun.dealDamageTo(target.GetComponent<PlayerController>());
            }
            if (target.CompareTag(Tags.Cover)) {
                Debug.Log("Hit " + target);
                target.GetComponent<CoverController>().takeDamage(gun.weapon.damage);
            }
            if (target.CompareTag(Tags.Shield)) {
                Debug.Log("Hit " + target);
                gun.dealDamageTo(target.GetComponent<PlayerController>());
            }
            Stop();
        }

        public virtual void Update() {
            if (launched) {
                transform.Translate(direction * speed * Time.deltaTime);
                // If prjectile flew more then 3 effective ranges, return it
                if (madeDistance >= (3 * gun.weapon.range)) {
                    Stop();
                }
            }
        }

        public virtual void Launch(Vector3 start, Vector3 direction) {
            initialPosition = start;
            this.direction = direction;
            speed = gun.weapon.projectile.speed;
            launched = true;
        }

        public virtual void Stop() {
            launched = false;
            if (gun != null) gun.returnProjectile(this);
            else Destroy(gameObject);
        }

        protected void OnBecameVisible() {
            
        }

        protected void OnBecameInvisible() {
            if (launched) {
                Stop();
            }
        }
    }
}
