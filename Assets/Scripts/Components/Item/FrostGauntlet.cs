using UnityEngine;
using System.Collections;

namespace SupHero.Components.Item {
    public class FrostGauntlet : ItemController {

        public GameObject frostArrow;

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
            // Create frost arrow
            GameObject instance = Instantiate(frostArrow, position, Quaternion.identity) as GameObject;
            instance.transform.SetParent(owner.transform.parent);
            Physics.IgnoreCollision(instance.GetComponent<Collider>(), owner.GetComponent<Collider>());
            FrostArrow projectile = instance.GetComponent<FrostArrow>();
            projectile.data = item;
            projectile.owner = owner;
            projectile.Launch(position, owner.transform.forward, item.activeData.range);

            Cooldown();
        }
    }
}
