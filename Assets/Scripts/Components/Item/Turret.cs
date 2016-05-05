using UnityEngine;
using System.Collections;

namespace SupHero.Components.Item {
    public class Turret : ItemController {

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
            Debug.Log("Deploy turret");
            GameObject instance = Instantiate(item.prefab, owner.directionMark.position, owner.directionMark.rotation) as GameObject;
            instance.transform.SetParent(owner.transform.parent);
            ready = false;

            Timer life = gameObject.AddComponent<Timer>();
            life.time = item.activeData.duration;
            life.OnEnd += delegate () {
                Destroy(instance.gameObject);
            };
            life.Launch();

            Cooldown();
        }

        protected override void enablePassive() {

        }
    }
}
