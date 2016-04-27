using UnityEngine;
using System.Collections;

namespace SupHero.Components.Level {
    public class ScoresController : MonoBehaviour {

        void Start() {
            Invoke(Utils.getActionName(transferToLounge), 7f);
        }

        private void transferToLounge() {
            Game.Instance.loadLounge();
        }

        void Update() {

        }
    }
}
