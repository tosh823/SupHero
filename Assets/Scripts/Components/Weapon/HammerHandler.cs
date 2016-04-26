using UnityEngine;
using System.Collections;
using SupHero.Components.Character;
using SupHero.Model;

namespace SupHero.Components.Weapon {
    public class HammerHandler : WeaponController {

        //private Collider edge;
        private bool doingSlash;

        public override void Start() {
            base.Start();
            //edge = GetComponent<Collider>();
            doingSlash = false;
        }

        public override void Update() {
            base.Update();
        }

        void OnTriggerEnter(Collider other) {
            GameObject target = other.gameObject;
            if (doingSlash && target.CompareTag(Tags.Player)) {
                dealDamageTo(target.GetComponent<PlayerController>());
            }
        }

        protected override void trigger() {
            doingSlash = true;
            playTriggerSound();
            Invoke(Utils.getActionName(disableEffects), 0.5f);
        }

        private void disableEffects() {
            doingSlash = false;
        }
    }
}
