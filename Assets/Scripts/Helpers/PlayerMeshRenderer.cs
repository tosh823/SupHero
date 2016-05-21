using UnityEngine;
using System.Collections;
using SupHero.Components.Character;

namespace SupHero.Components.Helpers {
    public class PlayerMeshRenderer : MonoBehaviour {

        private PlayerController player;

        void Start() {
            player = GetComponentInParent<PlayerController>();
        }

        void Update() {

        }

        void OnBecameVisible() {
            player.stopTracing();
        }

        void OnBecauseInVisible() {
            player.startTracing();
        }
    }
}
