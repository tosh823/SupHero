using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace SupHero.Components.UI {
    public class HeadsUpGuard : HeadsUpController {

        public override void Start() {
            base.Start();
            maxHealth = Data.Instance.mainSettings.guard.health;
        }

        public override void Update() {
            base.Update();
            if (carry != null) {
                updateHealth();
            }
        }
    }
}
