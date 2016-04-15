using UnityEngine;
using System.Collections;

namespace SupHero.Components.Character {
    public class Effect : MonoBehaviour {

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
            timer.launch();
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
