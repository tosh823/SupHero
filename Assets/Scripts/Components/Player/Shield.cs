using UnityEngine;
using System.Collections;
using SupHero;
using SupHero.Model;

namespace SupHero.Components.Character {
    public class Shield : MonoBehaviour {

        public Hero owner;
        private PlayerController player;
        private float timer;
        private GameObject shield;

        void Start() {
            timer = 0f;
            player = gameObject.GetComponent<PlayerController>();
            shield = Instantiate(Data.Instance.mainSettings.hero.shieldPrefab);
            shield.transform.SetParent(player.transform);
            shield.transform.position = player.transform.position;
        }

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
