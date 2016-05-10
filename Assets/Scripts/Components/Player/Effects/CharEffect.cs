using UnityEngine;
using System.Collections;

namespace SupHero.Components.Character {
    public class CharEffect : MonoBehaviour {

        public EffectData effect;
        protected PlayerController owner;
        protected Timer timer;

        public virtual void Start() {
            owner = GetComponent<PlayerController>();
            timer = gameObject.AddComponent<Timer>();
            timer.time = effect.duration;
            timer.OnStart += onEffectStart;
            timer.OnTick += onEffectTick;
            timer.OnEnd += onEffectFinish;
            timer.Launch();
        }

        public virtual void Update() {
            // When timer runs out, destroy effect
            if (timer == null) {
                Destroy(this);
            }
        }

        protected virtual void onEffectStart() {

        }

        protected virtual void onEffectTick() {

        }

        protected virtual void onEffectFinish() {

        }
    }
}
