using UnityEngine;
using System.Collections;

namespace SupHero.Components.Item {
    public class ShockWaveTeddy : ItemController {

        public GameObject teddy;

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
            GameObject instance = Instantiate(teddy, owner.directionMark.position, Quaternion.Euler(rotation)) as GameObject;
            instance.transform.SetParent(owner.transform.parent);
            // Setting properties of the teddy
            Teddy shockWave = instance.GetComponent<Teddy>();
            shockWave.data = item;
            shockWave.owner = owner;

            Cooldown();
        }

        protected override void enablePassive() {

        }
    }
}
