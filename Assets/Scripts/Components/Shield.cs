using UnityEngine;
using System.Collections;
using SupHero;
using SupHero.Model;

namespace SupHero.Components {
    public class Shield : MonoBehaviour {

        public Hero owner;
        private float timer;

        // Use this for initialization
        void Start() {
            timer = 0f;
        }

        // Update is called once per frame
        void Update() {
            if (owner.isAlive && !owner.isShieldFull) {
                // Ticking timer
                if (timer >= owner.replenishWaitTime) {
                    owner.replenishShield();
                }
                else {
                    timer += Time.deltaTime;
                }
            }
        }

        public void refreshTimer() {
            timer = 0f;
        }
    }
}
