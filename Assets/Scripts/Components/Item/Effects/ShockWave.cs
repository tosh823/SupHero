using UnityEngine;
using System.Collections;
using SupHero.Components.Character;

namespace SupHero.Components.Item {
    public class ShockWave : MonoBehaviour {

        public ItemData data;
        public PlayerController owner;

        void Start() {
            GetComponent<Animator>().SetTrigger("activate");
        }

        void Update() {

        }

        public void Deactivate() {
            Destroy(gameObject);
        }

        public void shotWave() {
            Debug.Log("Shooting wave");
            float start = -data.activeData.value / 2f;
            float end = data.activeData.value / 2f;
            float angle = start;
            while (angle < end) {
                Vector3 direction = Quaternion.AngleAxis(angle, Vector3.up) * transform.forward;
                Ray ray = new Ray(transform.position, direction);
                RaycastHit hit;
                if (Physics.Raycast(ray, out hit, data.activeData.range)) {
                    Transform found = hit.transform;
                    if (found.CompareTag(Tags.Player)) {
                        // If target is not the host of the turret
                        Debug.Log("Apply effect on " + found);
                        PlayerController pc = found.GetComponent<PlayerController>();
                        EffectData stun = new EffectData();
                        stun.type = EffectType.STUN;
                        stun.duration = data.activeData.duration;
                        pc.applyEffect(stun);
                    }
                }
                angle++;
            }
        }
    }
}
