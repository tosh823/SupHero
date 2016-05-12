using UnityEngine;
using SupHero.Model;

namespace SupHero.Components.Character {
    public class Shield : MonoBehaviour {

        public Hero owner;
        public GameObject shield;

        private PlayerController player;
        private float timer;

        void Start() {
            timer = 0f;
            player = gameObject.GetComponent<PlayerController>();
            shield = Instantiate(Data.Instance.mainSettings.hero.shieldPrefab);
            shield.transform.SetParent(player.transform);
            shield.transform.position = player.transform.position;
        }

        void Update() {
            if (owner.isAlive && !owner.isShieldFull) {
                // Disappear shield if empty
                // TODO!!! Make it better later
                if (owner.isShieldEmpty) {
                    if (shield.activeSelf) shield.SetActive(false);
                }
                else {
                    if (!shield.activeSelf) shield.SetActive(true);
                }

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
