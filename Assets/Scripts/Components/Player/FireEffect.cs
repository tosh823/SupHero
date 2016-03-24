using UnityEngine;
using System.Collections;

namespace SupHero.Components {
    public class FireEffect : Effect {

        public override void Start() {
            base.Start();
        }

        public override void Update() {
            base.Update();
        }

        protected override void onEffectStart() {

        }

        protected override void onEffectTick() {
            owner.player.receiveDamage(effect.value);
        }

        protected override void onEffectFinish() {

        }
    }
}
