using UnityEngine;
using System.Collections;
using SupHero;
using SupHero.Model;

namespace SupHero.Controllers {
    public class WeaponController : MonoBehaviour {

        public Weapon weapon;
        private float timeBetweenUsage;
        private LineRenderer lazer;

        private Ray shootRay;
        private RaycastHit shootHit;

        // Use this for initialization
        void Start() {
            weapon = new Weapon();
            timeBetweenUsage = 0f;
            lazer = GetComponent<LineRenderer>();
        }

        // Update is called once per frame
        void Update() {
            if (timeBetweenUsage < weapon.rate) {
                timeBetweenUsage += Time.deltaTime;
            }

        }

        public void useWeapon() {
            if (timeBetweenUsage >= weapon.rate) {
                timeBetweenUsage = 0f;
                // Shot!
                if (weapon.trigger()) {
                    lazer.enabled = true;
                    lazer.SetPosition(0, transform.position);
                    shootRay.origin = transform.position;
                    shootRay.direction = transform.forward;
                    lazer.SetPosition(1, shootRay.origin + shootRay.direction * weapon.range);

                    if (Physics.Raycast(shootRay, out shootHit, weapon.range)) {
                        GameObject target = shootHit.transform.gameObject;
                        lazer.SetPosition(1, shootHit.point);
                        if (target.CompareTag("Player")) {
                            target.GetComponent<PlayerController>().takeDamage(weapon.damage);
                        }
                    }

                    Invoke(Utils.getActionName(disableEffects), 0.05f);
                }
            }
        }

        private void disableEffects() {
            lazer.enabled = false;
        }
    }
}
