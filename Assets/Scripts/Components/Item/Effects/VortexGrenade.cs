using UnityEngine;
using System.Collections;
using SupHero.Components.Character;

namespace SupHero.Components.Item {
    public class VortexGrenade : BaseThrowable {

        public ItemData data;
        public PlayerController owner;
        public GameObject effect;

        private bool activated = false;

        public override void Start() {
            base.Start();
        }

        public override void Update() {
            base.Update();
            if (activated) {
                float maxDistance = data.activeData.range;
                float maxPower = data.activeData.value;
                Collider[] capturedObjects = Physics.OverlapSphere(transform.position, data.activeData.range);
                foreach (Collider captured in capturedObjects) {
                    GameObject target = captured.gameObject;
                    Rigidbody body = target.GetComponent<Rigidbody>();
                    // Don't affect ours
                    if (body == GetComponent<Rigidbody>()) continue;
                    // Affect objects
                    if (body != null) {
                        Vector3 distance = transform.position - body.transform.position;
                        float fraction = distance.magnitude / maxDistance;
                        if (fraction < 1f) {
                            // Science, bitch!
                            Vector3 force = Vector3.zero;
                            force.x = distance.normalized.x * maxPower * (1f - fraction);
                            force.z = distance.normalized.z * maxPower * (1f - fraction);
                            body.AddForce(force);
                        }
                    }
                }
            }
        }

        private void disableForces() {
            Collider[] capturedObjects = Physics.OverlapSphere(transform.position, data.activeData.range);
            foreach (Collider captured in capturedObjects) {
                GameObject target = captured.gameObject;
                Rigidbody body = target.GetComponent<Rigidbody>();
                // Don't affect ours
                if (body == GetComponent<Rigidbody>()) continue;
                // Affect objects
                if (body != null) {
                    body.velocity = Vector3.zero;
                    body.angularVelocity = Vector3.zero;
                }
            }
        }

        public override void OnStop() {
            base.OnStop();
            GetComponent<Animator>().SetTrigger("open");
        }

        private void Disable() {
            Destroy(gameObject);
        }

        private void createVortex() {
            activated = true;
            Rigidbody body = GetComponent<Rigidbody>();
            body.constraints = RigidbodyConstraints.FreezeAll;

            // Turn it on
            Timer life = gameObject.AddComponent<Timer>();
            life.time = data.activeData.duration;
            life.OnEnd += delegate () {
                activated = false;
                GetComponent<Animator>().SetTrigger("close");
                disableForces();
            };
            life.Launch();

            GameObject singularity = Instantiate(effect, transform.position, Quaternion.identity) as GameObject;
            singularity.transform.SetParent(transform);
        }
    }
}
