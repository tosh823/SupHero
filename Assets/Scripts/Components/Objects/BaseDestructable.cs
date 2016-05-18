using UnityEngine;

namespace SupHero.Components {
    public class BaseDestructable : MonoBehaviour {

        public float durability;
        public GameObject hitEffect;

        public virtual void Start() {

        }

        public virtual void OnTriggerEnter(Collider other) {
           
        }

        public virtual void OnCollisionEnter(Collision collision) {
            GameObject target = collision.gameObject;
            if (target.CompareTag(Tags.Projectile) && hitEffect != null) {
                ContactPoint contact = collision.contacts[0];
                Quaternion rot = Quaternion.FromToRotation(Vector3.up, contact.normal);
                Vector3 pos = contact.point;
                Instantiate(hitEffect, pos, rot);
            }
        }

        public virtual void Update() {
            if (durability <= 0f) {
                Destroy(gameObject);
            }
        }

        public virtual void takeDamage(float damage) {
            durability -= damage;
        }
    }
}
