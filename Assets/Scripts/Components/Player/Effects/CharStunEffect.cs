using UnityEngine;
using System.Collections;

namespace SupHero.Components.Character {
    public class CharStunEffect : CharEffect {

        public override void Start() {
            base.Start();
        }

        public override void Update() {
            base.Update();
        }

        protected override void onEffectStart() {
            owner.player.isStunned = true;
            Debug.Log(owner + " is stunned");
        }

        protected override void onEffectTick() {

        }

        protected override void onEffectFinish() {
            owner.player.isStunned = false;
            Debug.Log(owner + " is free to go");
        }
    }
}
