using UnityEngine;
using System.Collections;
using SupHero.Components.Weapon;

namespace SupHero.Components.Item {
    public class Turret : ItemController {

        public Transform machineGun;
        public Transform barrelEnd;
        public float scanAngle;

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
            GameObject instance = Instantiate(item.prefab, owner.directionMark.position, Quaternion.Euler(rotation)) as GameObject;
            instance.transform.SetParent(owner.transform.parent);
            // Setting properties of the turret
            Turret turret = instance.GetComponent<Turret>();
            WeaponController machineGun = instance.GetComponent<WeaponController>();
            turret.owner = owner;
            turret.item = item;
            machineGun.owner = owner;
            machineGun.weapon.damage = item.activeData.value;
            machineGun.weapon.rate = 60 * item.activeData.perSecond;
            machineGun.weapon.range = item.activeData.range;

            TurretMachineGun weapon = instance.AddComponent<TurretMachineGun>();
            Timer life = instance.AddComponent<Timer>();
            life.time = item.activeData.duration;
            life.OnStart += delegate () {
                weapon.Launch();
            };
            life.OnEnd += delegate () {
                weapon.Stop();
                Destroy(instance.gameObject);
            };
            life.Launch();

            Cooldown();
        }

        protected override void enablePassive() {

        }
    }
}
