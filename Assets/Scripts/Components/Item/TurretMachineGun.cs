using UnityEngine;
using System.Collections;
using SupHero.Components.Weapon;
using SupHero.Components.Character;

namespace SupHero.Components.Item {
    public class TurretMachineGun : MonoBehaviour {

        public bool scanning = false;
        public float speed = 30f;

        private Transform target = null;
        private Turret turret;
        private WeaponController machineGun;
        private float timeBetweenShots = 0f;
        private float rate = 1f;

        // Use this for initialization
        void Start() {
            turret = GetComponent<Turret>();
            machineGun = GetComponent<WeaponController>();
            rate = 1f / turret.item.activeData.perSecond;
        }

        // Update is called once per frame
        void Update() {
            if (scanning) {
                Scan();
                if (target != null) {
                    // Tracing target
                    Quaternion q = Quaternion.LookRotation(target.position - transform.position);
                    turret.machineGun.rotation = Quaternion.RotateTowards(turret.machineGun.rotation, q, speed * Time.deltaTime);
                    // According to rate of weapon, shooting
                    if (timeBetweenShots <= rate) {
                        timeBetweenShots += Time.deltaTime;
                    }
                    else {
                        Shoot();
                    }
                }
                else {
                    // Back to forward direction
                    Quaternion q = Quaternion.LookRotation(turret.transform.forward);
                    turret.machineGun.rotation = Quaternion.RotateTowards(turret.machineGun.rotation, q, speed * Time.deltaTime);
                }
            }
        }

        private void Shoot() {
            timeBetweenShots = 0f;
            Debug.Log("Shoot!");
            WeaponProjectile instance = machineGun.projectiles.popOrCreate(machineGun.weapon.projectile.prefab.GetComponent<WeaponProjectile>(), turret.barrelEnd.transform.position, Quaternion.identity);
            instance.gameObject.SetActive(true);
            instance.transform.parent = null;
            instance.gun = machineGun;
            instance.Launch(turret.barrelEnd.transform.position, turret.machineGun.transform.forward);
            machineGun.playTriggerSound();
        }

        public void Launch() {
            scanning = true;
        }

        public void Stop() {
            scanning = false;
        }

        private void Scan() {
            target = null;
            float start = -turret.scanAngle / 2f;
            float end = turret.scanAngle / 2f;
            float angle = start;
            while (angle < end) {
                Vector3 direction = Quaternion.AngleAxis(angle, Vector3.up) * transform.forward;
                Ray ray = new Ray(transform.position, direction);
                RaycastHit hit;
                if (Physics.Raycast(ray, out hit, turret.item.activeData.range)) {
                    Transform found = hit.transform;
                    if (found.CompareTag(Tags.Player)) {
                        // If target is not the host of the turret
                        PlayerController pc = found.GetComponent<PlayerController>();
                        if (pc != turret.owner && pc.player.isAlive) target = found;
                    }
                }
                angle++;
            }
        }
    }
}
