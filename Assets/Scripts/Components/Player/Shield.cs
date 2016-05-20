using UnityEngine;
using SupHero.Model;

namespace SupHero.Components.Character {
    public class Shield : MonoBehaviour {

        public Hero hero;
        public PlayerController player;
        public GameObject blastEffect;
        public float capacity;
        public bool charging { get; private set; }

        private bool active = true;
        private float timer = 0f;
        private float charge = 0f;
        private float minRadius;
        private float maxRadius;
        private float shieldUnit;
        private float minImpulse;
        private float maxImpulse;
        private float maxCapacity;
        private GameObject shieldHit;

        void Start() {
            charging = false;
            player = GetComponentInParent<PlayerController>();
            Physics.IgnoreCollision(player.GetComponent<Collider>(), GetComponent<Collider>());
            shieldHit = Data.Instance.mainSettings.hero.shieldHitPrefab;
            minRadius = Data.Instance.mainSettings.hero.minRadius;
            maxRadius = Data.Instance.mainSettings.hero.maxRadius;
            minImpulse = Data.Instance.mainSettings.hero.minImpulse;
            maxImpulse = Data.Instance.mainSettings.hero.maxImpulse;
            shieldUnit = Data.Instance.mainSettings.hero.shieldUnitPerCharge;
            maxCapacity = Data.Instance.mainSettings.hero.shield;
            capacity = maxCapacity;
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

        void Update() {
            if (hero != null && hero.isAlive && !hero.isShieldFull) {
                // If shield is empty, hide it
                if (hero.isShieldEmpty) hideShield();
                else showShield();

                capacity = hero.shield;

                // Ticking timer
                if (timer >= hero.replenishWaitTime && !charging) {
                    hero.replenishShield();
                }
                else {
                    timer += Time.deltaTime;
                }
            }
        }

        public void Charge() {
            if (hero.consumeShield(shieldUnit)) {
                charging = true;
                charge += shieldUnit;
            }
            else {
                Debug.Log("Cannot charge anymore");
            }
        }

        public void Exile() {
            Debug.Log("Exile");
            // Effect
            GameObject blast = Instantiate(blastEffect) as GameObject;
            blast.transform.position = transform.position;
            blast.transform.SetParent(transform);
            // Blast
            float ratio = charge / maxCapacity;
            float radius = (maxRadius - minRadius) * ratio;
            float impulse = (maxImpulse - minImpulse) * ratio;
            Collider[] capturedObjects = Physics.OverlapSphere(transform.position, radius);
            foreach (Collider captured in capturedObjects) {
                if (captured.GetComponent<Rigidbody>() != null) {
                    GameObject target = captured.gameObject;
                    Rigidbody body = target.GetComponent<Rigidbody>();
                    // Don't affect ours
                    if (body == GetComponent<Rigidbody>() || body == player.GetComponent<Rigidbody>()) continue;
                    // Affect objects
                    if (body.GetComponent<PlayerController>()) {
                        Vector3 distance = body.transform.position - transform.position;
                        Vector3 force = Vector3.zero;
                        force.x = distance.normalized.x * impulse;
                        force.z = distance.normalized.z * impulse;
                        body.AddForce(force, ForceMode.Impulse);
                    }
                }
            }
            charge = 0f;
            charging = false;
            refreshTimer();
        }

        // Need to find better solution, possibly via events
        private void hideShield() {
            if (active) {
                active = false;
                GetComponent<Collider>().enabled = false;
                GetComponent<Renderer>().enabled = false;
            }
        }

        private void showShield() {
            if (!active) {
                active = true;
                GetComponent<Collider>().enabled = true;
                GetComponent<Renderer>().enabled = true;
            }
        }

        public void refreshTimer() {
            timer = 0f;
        }
    }
}
