using UnityEngine;
using System.Collections;

namespace SupHero.Components.Helpers {
    public class CollisionDetection : MonoBehaviour {

        void Start() {

        }

        void Update() {

        }

        void OnTriggerEnter(Collider other) {
            Debug.Log("Triggered with " + other.gameObject);
        }

        void OnCollisionEnter(Collision collision) {
            Debug.Log("Collided with " + collision.gameObject);
        }
    }
}
