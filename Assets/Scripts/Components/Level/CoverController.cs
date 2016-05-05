using UnityEngine;

namespace SupHero.Components {
    public class CoverController : MonoBehaviour {

        // Make values to here for a while
        // Later better create model, parhaps
        public float durability = Constants.durablityNormal;

        void Start() {

        }

        public virtual void OnTriggerEnter(Collider other) {
           
        }

        public virtual void OnCollisionEnter(Collision collision) {
            
        }

        void Update() {
            if (durability <= 0f) {
                Destroy(gameObject);
            }
        }

        public void takeDamage(float damage) {
            durability -= damage;
        }
    }
}
