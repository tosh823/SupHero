using UnityEngine;
using System.Collections;
using SupHero.Components.Character;

namespace SupHero.Components.Item {
    public class Fireball : BaseProjectile {

        public ItemData data;
        public PlayerController owner;
        public GameObject explosion;

        public override void Start() {
            base.Start();
        }

        public override void Update() {
            base.Update();
        }

        void OnTriggerEnter(Collider other) {
            GameObject target = other.gameObject;
            // Explostion
            GameObject effect = Instantiate(explosion, transform.position, Quaternion.identity) as GameObject;
            effect.transform.SetParent(transform.parent);
            if (target.CompareTag(Tags.Player) || target.CompareTag(Tags.Shield)) {
                Debug.Log("Hit " + target);
                target.GetComponent<PlayerController>().receiveDamage(data.activeData.value, false);
            }
            if (target.CompareTag(Tags.Cover)) {
                Debug.Log("Hit " + target);
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
