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
                Vector3 moveVector = new Vector3(0f, 0f, speed * Time.deltaTime);
                //transform.Translate(direction * speed * Time.deltaTime);
                transform.Translate(moveVector);
            }
        }

        public virtual void Launch(Vector3 start, Vector3 direction, float speed) {
            startPosition = start;
            this.direction = direction;
            transform.rotation = Quaternion.LookRotation(direction); // New line
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
