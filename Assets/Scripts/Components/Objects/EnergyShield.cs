using UnityEngine;
using System.Collections;

namespace SupHero.Components {
    public class EnergyShield : MonoBehaviour {

        private GameObject shieldHit;

        void Start() {
            shieldHit = Data.Instance.mainSettings.hero.shieldHitPrefab;
        }

        void Update() {

        }

        // Creating effects
        void OnCollisionEnter(Collision collision) {
            GameObject target = collision.gameObject;
            if (target.CompareTag(Tags.Projectile)) {
                ContactPoint contact = collision.contacts[0];
                Quaternion rot = Quaternion.FromToRotation(Vector3.forward, contact.normal);
                Vector3 pos = contact.point;
                Instantiate(shieldHit, pos, rot);
            }
            else if (target.CompareTag(Tags.Player)) {
                Physics.IgnoreCollision(GetComponent<Collider>(), target.GetComponent<Collider>());
            }
        }
    }
}
