using UnityEngine;
using System.Collections;

namespace SupHero.Components.Character {
    public class StunEffect : Effect {

        public override void Start() {
            base.Start();
        }

        public override void Update() {
            base.Update();
        }

        protected override void onEffectStart() {
            owner.player.isStunned = true;
        }

        protected override void onEffectTick() {

        }

        protected override void onEffectFinish() {
            owner.player.isStunned = false;
        }
    }
}
