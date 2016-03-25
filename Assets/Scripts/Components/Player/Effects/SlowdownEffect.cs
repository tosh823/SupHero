using UnityEngine;
using System.Collections;

namespace SupHero.Components {
    public class SlowdownEffect : Effect {

        public override void Start() {
            base.Start();
        }

        public override void Update() {
            base.Update();
        }

        // TODO!!!
        // Later come up with better way of slowing down the player
        protected override void onEffectStart() {
            owner.player.speed -= 2f;
        }

        protected override void onEffectTick() {

        }

        protected override void onEffectFinish() {
            owner.player.speed += 2f;
        }
    }
}
