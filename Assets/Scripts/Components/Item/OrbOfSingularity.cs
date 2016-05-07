using UnityEngine;
using System.Collections;

namespace SupHero.Components.Item {
    public class OrbOfSingularity : ItemController {

        public GameObject grenade;

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

            // Find location
            Vector3 rotation = owner.directionMark.rotation.eulerAngles;
            rotation.x = 0f;
            Vector3 position = owner.directionMark.position;
            position.y = 0f;
            // Create fireball
            GameObject instance = Instantiate(grenade, position, Quaternion.identity) as GameObject;
            instance.transform.SetParent(owner.transform.parent);
            Physics.IgnoreCollision(instance.GetComponent<Collider>(), owner.GetComponent<Collider>());
            VortexGrenade projectile = instance.GetComponent<VortexGrenade>();
            projectile.data = item;
            projectile.owner = owner;
            projectile.Throw(position, owner.transform.forward, 2f, 6f);

            Cooldown();
        }

        protected override void enablePassive() {

        }
    }
}
