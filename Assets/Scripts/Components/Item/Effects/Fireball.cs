using UnityEngine;
using System.Collections;
using SupHero.Components.Character;
using SupHero.Model;

namespace SupHero.Components.Item {
    public class Fireball : BaseProjectile {

        public ItemData data;
        public PlayerController owner;
        public GameObject explosion;

        public override void Start() {
            base.Start();
            if (owner.isHero()) {
                if (owner.shield != null) {
                    Physics.IgnoreCollision(GetComponent<Collider>(), owner.shield.GetComponent<Collider>());
                }
            }
        }

        public override void Update() {
            base.Update();
        }

        void OnTriggerEnter(Collider other) {
            GameObject target = other.gameObject;
            Debug.Log("Hit " + target);
            // Explostion
            GameObject effect = Instantiate(explosion, transform.position, Quaternion.identity) as GameObject;
            effect.transform.SetParent(transform.parent);
            if (target.CompareTag(Tags.Player)) {
                target.GetComponent<PlayerController>().receiveDamage(data.activeData.value, false);
            }
            if (target.CompareTag(Tags.Shield)) {
                target.GetComponent<Shield>().player.receiveDamage(data.activeData.value, false);
            }
            if (target.CompareTag(Tags.Cover)) {
                target.GetComponent<CoverController>().takeDamage(data.activeData.value);
            }
            Stop();
        }

        public override void Launch(Vector3 start, Vector3 direction, float speed) {
            base.Launch(start, direction, speed);
        }

        public override void Stop() {
            base.Stop();
            Destroy(gameObject);
        }
    }
}
