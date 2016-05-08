using UnityEngine;
using System.Collections;

namespace SupHero.Components.Item {
    public class AimingGlass : ItemController {

        public GameObject vision;
        private GameObject instance;

        public override void Start() {
            base.Start();
            enablePassive();
        }

        public override void Update() {
            base.Update();
            if (instance != null) {
                Vector3 position = owner.directionMark.position;
                position.y = 0f;
                LineRenderer line = instance.GetComponent<LineRenderer>();
                line.SetPosition(0, position);
                line.SetPosition(1, position + (item.passiveData.range * owner.transform.forward));
            }
        }

        public override ItemStatus checkStatus() {
            return ItemStatus.ONLY_PASSIVE;
        }

        protected override void Trigger() {

        }

        protected override void enablePassive() {

            // Find location
            Vector3 rotation = owner.directionMark.rotation.eulerAngles;
            rotation.x = 0f;
            Vector3 position = owner.directionMark.position;
            position.y = 0f;

            GameObject aim = Instantiate(vision, position, Quaternion.identity) as GameObject;
            aim.transform.SetParent(owner.transform);
            LineRenderer line = aim.GetComponent<LineRenderer>();
            line.SetPosition(0, aim.transform.position);
            line.SetPosition(1, aim.transform.position * item.passiveData.range);
            instance = aim;
        }

        protected override void disablePassive() {
            Destroy(instance.gameObject);
        }
    }
}
