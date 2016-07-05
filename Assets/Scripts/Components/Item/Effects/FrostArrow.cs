using UnityEngine;
using System.Collections;
using SupHero.Components.Character;
using SupHero.Model;

namespace SupHero.Components.Item {
    public class FrostArrow : BaseProjectile {

        public ItemData data;
        public PlayerController owner;
        public GameObject explosion;

        public override void Start() {
            base.Start();
            if (owner != null && owner.isHero()) {
                if (owner.shield != null) {
                    Physics.IgnoreCollision(GetComponent<Collider>(), owner.shield.GetComponent<Collider>());
                }
            }
        }

        public override void Update() {
            base.Update();
            if (launched && distanceTraveled >= (data.activeData.range)) {
                Stop();
            }
        }

        void OnCollisionEnter(Collision collision) {
            GameObject target = collision.gameObject;
            // Effect
            Quaternion rot = Quaternion.FromToRotation(Vector3.up, collision.contacts[0].normal);
            Vector3 pos = collision.contacts[0].point;
            GameObject effect = Instantiate(explosion, pos, rot) as GameObject;
            effect.transform.SetParent(transform.parent);
            // Damage and effect
            if (target.CompareTag(Tags.Player)) {
                target.GetComponent<PlayerController>().receiveDamage(data.activeData.value, false);
                EffectData freeze = new EffectData();
                freeze.type = EffectType.SLOWDOWN;
                freeze.duration = data.activeData.duration;
                freeze.value = data.activeData.value2;
                target.GetComponent<PlayerController>().applyEffect(freeze);
            }
            if (target.CompareTag(Tags.Shield)) {
                target.GetComponent<Shield>().player.receiveDamage(data.activeData.value, false);
                EffectData freeze = new EffectData();
                freeze.type = EffectType.SLOWDOWN;
                freeze.duration = data.activeData.duration;
                freeze.value = data.activeData.value2;
                target.GetComponent<Shield>().player.applyEffect(freeze);
            }
            if (target.CompareTag(Tags.Cover)) {
                target.GetComponent<BaseDestructable>().takeDamage(data.activeData.value);
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
