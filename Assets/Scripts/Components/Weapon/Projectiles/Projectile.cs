using UnityEngine;
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
                Debug.Log("Hit player " + target.GetComponent<PlayerController>().tokenName);
                DamageResult hit = target.GetComponent<PlayerController>().receiveDamage(gun.weapon.damage);
                Stop();
            }
            else if (target.CompareTag(Tags.Cover)) {
                Debug.Log("Hit cover");
                target.GetComponent<CoverController>().takeDamage(gun.weapon.damage);
                Stop();
            }
        }

        public virtual void OnCollisionEnter(Collision collision) {
            Debug.Log("Hit " + collision.gameObject);
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
