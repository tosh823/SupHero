using UnityEngine;
using System.Collections;
using SupHero.Components.Character;

namespace SupHero.Components.Item {
    public class ShockWave : BaseVisualEffect {

        public Teddy source;
        public float range;
        public float speed;
        public GameObject shockPrefab;
        private bool launched = false;
        private Vector3 initialPosition = Vector3.zero;
        private Vector3 direction = Vector3.zero;

        public override void Start() {
            base.Start();
        }

        void OnTriggerEnter(Collider other) {
            GameObject target = other.gameObject;
            if (target.CompareTag(Tags.Player)) {
                PlayerController pc = target.GetComponent<PlayerController>();
                pc.GetComponent<Rigidbody>().AddForce(-pc.transform.forward * 1.5f, ForceMode.Impulse);
                EffectData stun = new EffectData();
                stun.type = EffectType.STUN;
                stun.duration = source.data.activeData.duration;
                stun.prefab = shockPrefab;
                pc.applyEffect(stun);
            }
        }

        public override void Update() {
            base.Update();
            if (launched) {
                if (Vector3.Distance(transform.position, initialPosition) >= range) {
                    destroyEffect();
                }
                else transform.Translate(direction * speed * Time.deltaTime);
            }
        }

        public void Launch(Vector3 direction) {
            initialPosition = transform.position;
            this.direction = direction;
            launched = true;
        }
    }
}
