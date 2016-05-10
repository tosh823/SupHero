using UnityEngine;
using System.Collections;

namespace SupHero.Components.Item {
    public class FireGauntlet : ItemController {

        public GameObject fireball;

        public override void Start() {
            base.Start();
            if (owner != null && item.hasPassive) enablePassive();
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

            // Find location
            Vector3 rotation = owner.directionMark.rotation.eulerAngles;
            rotation.x = 0f;
            Vector3 position = owner.directionMark.position;
            position.y = 0f;
            // Create fireball
            GameObject instance = Instantiate(fireball, position, Quaternion.identity) as GameObject;
            instance.transform.SetParent(owner.transform.parent);
            Physics.IgnoreCollision(instance.GetComponent<Collider>(), owner.GetComponent<Collider>());
            Fireball projectile = instance.GetComponent<Fireball>();
            projectile.data = item;
            projectile.owner = owner;
            projectile.Launch(position, owner.transform.forward, item.activeData.range);

            Cooldown();
        }

        protected override void enablePassive() {
            
        }
    }
}
