using UnityEngine;
using System.Collections;

namespace SupHero.Components.Item {
    public class FrostGauntlet : ItemController {

        public override void Start() {
            base.Start();
        }

        public override void Update() {
            base.Update();
        }

        public override ItemStatus checkStatus() {
            if (ready) return ItemStatus.NEED_AIM;
            else return ItemStatus.COOLDOWN;
        }

        protected override void Trigger() {

        }
    }
}
