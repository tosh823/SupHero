using UnityEngine;
using System.Collections;

namespace SupHero.Components {
    public class BaseProjectile : MonoBehaviour {

        public float speed = 1f;
        public bool autoLaunch = false;

        protected bool launched = false;
        protected Vector3 startPosition;
        protected Vector3 direction;
        protected float distanceTraveled {
            get {
                return Vector3.Distance(transform.position, startPosition);
            }
        }

        public virtual void Start() {
            if (autoLaunch) Launch(transform.position, Vector3.forward, speed);
        }

        public virtual void Update() {
            if (launched) {
                transform.Translate(direction * speed * Time.deltaTime);
            }
        }

        public virtual void Launch(Vector3 start, Vector3 direction, float speed) {
            startPosition = start;
            this.direction = direction;
            this.speed = speed;
            launched = true;
        }

        public virtual void Stop() {
            launched = false;
        }

        void OnBecameInvisible() {
            if (launched) {
                Stop();
            }
        }
    }
}
