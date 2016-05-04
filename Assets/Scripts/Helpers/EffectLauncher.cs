using UnityEngine;
using System.Collections;

namespace SupHero.Components.Helpers {
    public class EffectLauncher : MonoBehaviour {

        public string command;
        private Animator mecanim;

        // Use this for initialization
        void Start() {
            mecanim = GetComponent<Animator>();
        }

        // Update is called once per frame
        void Update() {
            mecanim.SetTrigger(command);
        }
    }
}
