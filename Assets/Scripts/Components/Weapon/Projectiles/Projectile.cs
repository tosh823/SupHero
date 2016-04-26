using UnityEngine;
using SupHero.Components.Character;
using SupHero.Model;

namespace SupHero.Components.Weapon {
    public class Projectile : MonoBehaviour {
        
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
                gun.dealDamageTo(target.GetComponent<PlayerController>());
                Stop();
            }
            else if (target.CompareTag(Tags.Cover)) {
                target.GetComponent<CoverController>().takeDamage(gun.weapon.damage);
                Stop();
            }
        }

        public virtual void OnCollisionEnter(Collision collision) {
            Stop();
        }

        public virtual void Update() {
            if (launched) {
                transform.Translate(direction * speed * Time.deltaTime);
                Debug.DrawLine(initialPosition, transform.position, Color.cyan, Time.deltaTime);
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
            gun.returnProjectile(this);
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
