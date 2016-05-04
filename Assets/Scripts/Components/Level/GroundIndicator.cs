using UnityEngine;
using System.Collections;

namespace SupHero.Components.Level {

    public struct EffectAnimState {
        public static string ACTIVATE = "activate";
    }

    public class GroundIndicator : MonoBehaviour {

        private Animator mecanim;
        private ParticleSystem[] particles;

        void Start() {
            mecanim = GetComponent<Animator>();
            particles = GetComponentsInChildren<ParticleSystem>();
            //Launch();
        }

        void Update() {

        }

        public void Launch() {
            foreach (ParticleSystem particle in particles) {
                if (particle.isPlaying) particle.Stop();
                if (!particle.isPlaying) particle.Play();
            }
            mecanim.SetTrigger(EffectAnimState.ACTIVATE);
        }
    }
}
