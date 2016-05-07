using UnityEngine;
using System.Collections;

namespace SupHero.Components.Item {
    public class InstantCover : ItemController {

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
            ready = false;

            // Find rotation of deployment
            Vector3 rotation = owner.directionMark.rotation.eulerAngles;
            rotation.x = 0f;
            // Create shield
            GameObject instance = Instantiate(item.prefab, owner.directionMark.position, Quaternion.Euler(rotation)) as GameObject;
            instance.transform.SetParent(owner.transform.parent);
            instance.GetComponent<CoverController>().durability = item.activeData.value;
            
            Timer life = instance.AddComponent<Timer>();
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
