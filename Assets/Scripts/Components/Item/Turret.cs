using UnityEngine;
using System.Collections;
using SupHero.Components.Weapon;

namespace SupHero.Components.Item {
    public class Turret : ItemController {

        public GameObject machineGun;
            
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
            ready = false;

            // Find rotation of deployment
            Vector3 rotation = owner.directionMark.rotation.eulerAngles;
            rotation.x = 0f;
            GameObject instance = Instantiate(machineGun, owner.directionMark.position, Quaternion.Euler(rotation)) as GameObject;
            instance.transform.SetParent(owner.transform.parent);
            // Setting properties of the turret
            TurretMachineGun turret = instance.GetComponent<TurretMachineGun>();
            turret.data = item;
            turret.owner = owner;

            Timer life = instance.AddComponent<Timer>();
            life.time = item.activeData.duration;
            life.OnEnd += delegate () {
                turret.Stop();
            };
            life.Launch();

            Cooldown();
        }

        protected override void enablePassive() {

        }
    }
}
