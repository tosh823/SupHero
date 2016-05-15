using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using SupHero.Model;

namespace SupHero.Components.UI {
    public class HeadsUpHero : HeadsUpController {

        public Image shieldBar;
        private float maxShield;

        public override void Start() {
            base.Start();
            maxHealth = Data.Instance.mainSettings.hero.health;
            maxShield = Data.Instance.mainSettings.hero.shield;
        }

        public override void Update() {
            base.Update();
            if (carry != null) {
                updateHealth();
                updateShield();
            }
        }

        private void updateShield() {
            float ratio = (carry.player as Hero).shield / maxShield;
            shieldBar.fillAmount = ratio;
        }
    }
}
