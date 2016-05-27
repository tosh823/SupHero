using UnityEngine;
using System.Collections;

namespace SupHero.Components.Character {
    public class CharStunEffect : CharEffect {

        private GameObject stars;

        public override void Start() {
            base.Start();
        }

        public override void Update() {
            base.Update();
        }

        protected override void onEffectStart() {
            owner.player.isStunned = true;
            stars = Instantiate(effect.prefab, transform.position, Quaternion.identity) as GameObject;
            stars.transform.SetParent(transform);
            stars.transform.Translate(0f, 1.5f, 0f);
            owner.mecanim.SetBool(State.STUN, true);
            Debug.Log(owner + " is stunned");
        }

        protected override void onEffectTick() {

        }

        protected override void onEffectFinish() {
            owner.player.isStunned = false;
            owner.mecanim.SetBool(State.STUN, false);
            stars.GetComponent<BaseVisualEffect>().destroyEffect();
            Debug.Log(owner + " is free to go");
        }
    }
}
