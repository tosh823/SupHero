using UnityEngine;
using System.Collections;
using SupHero.Components.Weapon;
using SupHero.Components.Character;
using SupHero.Model;

namespace SupHero.Components.Item {
    public class TurretMachineGun : MonoBehaviour {

        public bool scanning = false;
        public float rotationSpeed = 50f;
        public float scanAngle = 120f;
        public Transform machineGun;
        public Transform barrelEnd;
        public ItemData data;
        public PlayerController owner;

        private Transform target = null;
        private WeaponController gun;
        private float timeBetweenShots = 0f;
        private float rate = 1f;

        // Use this for initialization
        void Start() {
            gun = GetComponent<WeaponController>();
            gun.owner = owner;
            gun.weapon.damage = data.activeData.value;
            gun.weapon.range = data.activeData.range;
            gun.weapon.rate = data.activeData.perSecond * 60f;
            rate = 1f / data.activeData.perSecond;
        }

        // Update is called once per frame
        void Update() {
            if (scanning) {
                target = null;
                Scan();
                if (target != null) {
                    // Tracing target
                    Quaternion direction = Quaternion.LookRotation(target.position - transform.position);
                    machineGun.rotation = Quaternion.RotateTowards(machineGun.rotation, direction, rotationSpeed * Time.deltaTime);
                    // According to rate of weapon, shooting
                    if (timeBetweenShots <= rate) timeBetweenShots += Time.deltaTime;
                    else Shoot();
                }
                else {
                    // Back to forward direction
                    Quaternion direction = Quaternion.LookRotation(transform.forward);
                    machineGun.rotation = Quaternion.RotateTowards(machineGun.rotation, direction, rotationSpeed * Time.deltaTime);
                }
            }
        }

        private void Shoot() {
            timeBetweenShots = 0f;
            Debug.Log("Shoot!");
            WeaponProjectile instance = gun.projectiles.popOrCreate(gun.weapon.projectile.prefab.GetComponent<WeaponProjectile>(), barrelEnd.transform.position, Quaternion.identity);
            instance.gameObject.SetActive(true);
            instance.transform.parent = owner.transform.parent;
            instance.gun = gun;
            Physics.IgnoreCollision(instance.GetComponent<Collider>(), GetComponent<Collider>());
            instance.Launch(barrelEnd.transform.position, barrelEnd.transform.forward);
            gun.playTriggerSound();
        }

        public void Launch() {
            scanning = true;
            GetComponent<Animator>().enabled = false;
        }

        public void Stop() {
            scanning = false;
            GetComponent<Animator>().enabled = true;
            GetComponent<Animator>().SetTrigger("disable");
        }

        public void Disable() {
            Destroy(gameObject);
        }

        private void Scan() {
            float start = -scanAngle / 2f;
            float end = scanAngle / 2f;
            float angle = start;
            while (angle < end) {
                Vector3 direction = Quaternion.AngleAxis(angle, Vector3.up) * transform.forward;
                Ray ray = new Ray(transform.position, direction);
                RaycastHit hit;
                if (Physics.Raycast(ray, out hit, data.activeData.range)) {
                    Transform found = hit.transform;
                    if (found.CompareTag(Tags.Player)) {
                        // If target is not the host of the turret
                        // And not on the same side
                        PlayerController foundPC = found.GetComponent<PlayerController>();
                        bool heroVSguard = ((owner.player is Hero) && (foundPC.player is Guard));
                        bool guardVShero = ((owner.player is Guard) && (foundPC.player is Hero));
                        if (heroVSguard || guardVShero) target = found;
                    }
                }
                angle++;
            }
        }
    }
}
