using UnityEngine;
using System.Collections;

namespace SupHero.Components.Character {
    public class CharSlowdownEffect : CharEffect {

        public override void Start() {
            base.Start();
        }

        public override void Update() {
            base.Update();
        }

        // TODO!!!
        // Later come up with better way of slowing down the player
        protected override void onEffectStart() {
            owner.player.speed -= effect.value;
            Debug.Log(owner + " is slowed");
        }

        protected override void onEffectTick() {

        }

        protected override void onEffectFinish() {
            owner.player.speed += effect.value;
            Debug.Log(owner + " gets back to normal");
        }
    }
}
