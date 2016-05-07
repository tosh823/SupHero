using UnityEngine;
using System.Collections;

namespace SupHero.Components {
    public class BaseThrowable : MonoBehaviour {

        public float impulse = 1f;
        public float height = 4f;
        public bool autoThrow = false;

        protected bool thrown = false;
        protected bool touched = false;
        protected bool stopped = false;
        protected Vector3 startPosition;
        protected Vector3 direction;
        protected float threshold = 0.001f;
        protected float distanceTraveled {
            get {
                return Vector3.Distance(transform.position, startPosition);
            }
        }

        private float prevDistance;

        public virtual void Start() {
            if (autoThrow) Throw(transform.position, transform.forward, height, impulse);
        }

        public virtual void Update() {
            if (thrown && touched && !stopped) {
                float distance = distanceTraveled;
                if (Mathf.Abs(distance - prevDistance) <= threshold) {
                    OnStop();
                }
                prevDistance = distance;
            }
        }

        protected virtual void OnCollisionEnter(Collision collision) {
            if (thrown) {
                GameObject obstacle = collision.gameObject;
                Debug.Log("Hit the " + obstacle);
                OnTouched();
            }
        }

        public virtual void Throw(Vector3 start, Vector3 direction, float height, float impulse) {
            // Storing data
            startPosition = start;
            this.direction = direction;
            this.height = height;
            this.impulse = impulse;
            prevDistance = distanceTraveled;
            thrown = true;

            // Throw
            Rigidbody body = GetComponent<Rigidbody>();
            Vector3 force = new Vector3(direction.x * impulse, height * impulse/2f, direction.z * impulse);
            body.AddForce(force, ForceMode.Impulse);
        }

        public virtual void OnTouched() {
            touched = true;
        }

        public virtual void OnStop() {
            stopped = true;
        }
    }
}
