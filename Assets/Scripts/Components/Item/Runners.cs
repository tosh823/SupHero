using UnityEngine;
using System.Collections;

namespace SupHero.Components.Item {
    public class Runners : ItemController {

        public override void Start() {
            base.Start();
            if (owner != null && item.hasPassive) enablePassive();
        }

        public override void Update() {
            base.Update();
        }

        public override ItemStatus checkStatus() {
            if (ready) return ItemStatus.ACTIVE_READY;
            else return ItemStatus.COOLDOWN;
        }

        protected override void Trigger() {
            Debug.Log("Activate runners");
            ready = false;

            Timer effect = gameObject.AddComponent<Timer>();
            effect.time = item.activeData.duration;
            effect.OnStart += delegate () {
                owner.player.speed += item.activeData.value;
                Debug.Log(item.name + " effect turned on");
            };
            effect.OnEnd += delegate () {
                owner.player.speed -= item.activeData.value;
                Debug.Log(item.name + " effect turned off");
            };
            effect.Launch();

            Cooldown();
        }

        public override void Unequip() {
            disablePassive();
            // If item was activated, remove active effect
            if (GetComponent<Timer>() != null) {
                owner.player.speed -= item.activeData.value;
                Debug.Log(item.name + " effect turned off");
            }
        }

        protected override void enablePassive() {
            owner.player.speed += item.passiveData.value;
        }

        protected override void disablePassive() {
            owner.player.speed -= item.passiveData.value;
        }
    }
}
