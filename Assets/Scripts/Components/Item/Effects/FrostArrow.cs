using UnityEngine;
using System.Collections;
using SupHero.Components.Character;
using SupHero.Model;

namespace SupHero.Components.Item {
    public class FrostArrow : BaseProjectile {

        public ItemData data;
        public PlayerController owner;

        public override void Start() {
            base.Start();
            if (owner.player is Hero) {
                Shield shield = owner.GetComponent<Shield>();
                if (shield != null) {
                    Physics.IgnoreCollision(GetComponent<Collider>(), shield.shield.GetComponent<Collider>());
                }
            }
        }

        public override void Update() {
            base.Update();
        }

        void OnTriggerEnter(Collider other) {
            GameObject target = other.gameObject;
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
