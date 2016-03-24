using UnityEngine;
using System.Collections;
using SupHero;
using SupHero.Model;

namespace SupHero.Components {
    public class CoverController : MonoBehaviour {

        // Make values to here for a while
        // Later better create model, parhaps
        public float durability = Constants.durablityNormal;

        void Start() {

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
