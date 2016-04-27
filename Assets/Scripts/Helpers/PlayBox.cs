using UnityEngine;
using System.Collections;
using SupHero.Components.UI;

namespace SupHero.Components.Helpers {
    public class PlayBox : MonoBehaviour {

        private HUDController HUD;

        void Start() {
            HUD = GameObject.FindGameObjectWithTag(Tags.MainUI).GetComponent<HUDController>();
        }

        void Update() {

        }

        void OnTriggerEnter(Collider other) {
            GameObject entered = other.gameObject;
            if (entered.CompareTag(Tags.Player)) {
                HUD.showMessage("Starting in a moment!", 2f);
                Invoke(Utils.getActionName(transferToLevel), 2.1f);
            }
        }

        void transferToLevel() {
            Game.Instance.loadLevel();
        }
    }
}
