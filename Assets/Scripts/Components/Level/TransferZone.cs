using UnityEngine;
using System.Collections;
using SupHero.Model;
using SupHero.Components.Character;

namespace SupHero.Components.Level {
    public class TransferZone : MonoBehaviour {

        void OnTriggerEnter(Collider other) {
            GameObject entered = other.gameObject;
            if (entered.CompareTag(Tags.Player)) {
                // If hero reached the end of zone, go to new one
                if (entered.GetComponent<PlayerController>().player is Hero) {
                    Destroy(LevelController.Instance.GetComponent<Timer>());
                    LevelController.Instance.newTurn();
                }
            }
        }
    }
}
