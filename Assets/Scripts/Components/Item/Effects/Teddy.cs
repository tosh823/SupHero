using UnityEngine;
using System.Collections;
using SupHero.Components.Character;

namespace SupHero.Components.Item {
    public class Teddy : MonoBehaviour {

        public ItemData data;
        public PlayerController owner;
        public Transform sourceLocation;

        // Effects
        public GameObject chargeEffectPrefab;
        public GameObject blastEffectPrefab;
        public GameObject waveEffectPrefab;

        private GameObject chargeEffect;

        void Start() {
            GetComponent<Animator>().SetTrigger("activate");
        }

        void Update() {

        }

        public void Deactivate() {
            Destroy(gameObject);
        }

        public void createChargeEffect() {
            chargeEffect = Instantiate(chargeEffectPrefab, sourceLocation.position, Quaternion.identity) as GameObject;
            chargeEffect.transform.SetParent(transform);
        }

        public void createBlastEffect() {
            Destroy(chargeEffect.gameObject);
            Instantiate(blastEffectPrefab, sourceLocation.position, Quaternion.identity);
        }

        public void createWaveEffect(Vector3 direction) {
            GameObject instance = Instantiate(waveEffectPrefab, transform.position + transform.forward * 2f, Quaternion.identity) as GameObject;
            instance.transform.SetParent(transform.parent);
            ShockWave wave = instance.GetComponent<ShockWave>();
            wave.source = this;
            wave.speed = data.activeData.perSecond;
            wave.range = data.activeData.range;
            wave.Launch(direction);
        }

        public void shotWave() {
            float start = -data.activeData.value / 2f;
            float end = data.activeData.value / 2f;
            float angle = start;
            while (angle < end) {
                Vector3 direction = Quaternion.AngleAxis(angle, Vector3.up) * transform.forward;
                createWaveEffect(direction);
                angle += 10;
            }
        }

        // Old variant
        /*public void shotWave() {
            createWaveEffect();
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
        }*/
    }
}
