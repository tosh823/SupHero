using UnityEngine;
using System.Collections;

namespace SupHero.Components.Item {
    public class FireGauntlet : ItemController {

        public override void Start() {
            base.Start();
        }

        public override void Update() {
            base.Update();
        }

        public override ItemStatus checkStatus() {
            if (activeReady()) {
                return ItemStatus.NEED_AIM;
            }
            else return ItemStatus.COOLDOWN;
        }

        protected override void trigger() {
            
        }

        protected override void enablePassive() {

        }
    }
}
