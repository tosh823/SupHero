using UnityEngine;
using System.Collections;
using SupHero.Components.Character;

namespace SupHero.Components.Level {
    public class Drop : MonoBehaviour {

        private Light light;

        public GameObject dropItem;
        public bool autoDestroy = true;

        public Entity entity;
        public int id;

        void Start() {
            light = GetComponent<Light>();

            dropItem = Instantiate(dropItem) as GameObject;
            dropItem.transform.SetParent(transform);
            dropItem.transform.position = transform.position;
            dropItem.transform.eulerAngles = new Vector3(-90f, 0f, 0f);
            configureLight();
        }

        void Update() {
            transform.Rotate(new Vector3(0f, 20f, 0f) * Time.deltaTime);
        }

        void OnTriggerEnter(Collider other) {
            Transform incoming = other.gameObject.transform;
            if (incoming.CompareTag(Tags.Player)) {
                // Detecting player to receive drop
                incoming.GetComponent<PlayerController>().receiveDrop(entity, id);
                if (autoDestroy) Destroy(gameObject);
            }
        }

        void configureLight() {
            switch (entity) {
                case Entity.WEAPON:
                    light.color = Color.cyan;
                    light.intensity = 1f;
                    break;
                case Entity.ITEM:
                    light.color = Color.red;
                    light.intensity = 4f;
                    break;
                case Entity.SUPPLY:
                    light.color = Color.white;
                    break;
                default:
                    break;
            }
        }
    }
}
