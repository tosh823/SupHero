using UnityEngine;
using System.Collections;
using SupHero.Components.Character;

namespace SupHero.Components.Level {
    public class Drop : MonoBehaviour {

        private Light neon;

        public GameObject dropItem;
        public bool autoDestroy = true;

        public Entity entity;
        public int id;

        void Start() {
            neon = GetComponent<Light>();

            if (dropItem != null) {
                dropItem = Instantiate(dropItem) as GameObject;
                dropItem.transform.SetParent(transform);
                dropItem.transform.position = transform.position;
                dropItem.transform.eulerAngles = new Vector3(-90f, 0f, 0f);
                configureLight();
            }
            else {
                Destroy(gameObject);
            }
        }

        void Update() {
            transform.Rotate(new Vector3(0f, 20f, 0f) * Time.deltaTime);
        }

        public void createWeaponDrop(int id) {
            entity = Entity.WEAPON;
            this.id = id;
            dropItem = Data.Instance.getWeaponById(id).prefab;
        }

        public void createDrop(Entity type, int id) {
            entity = type;
            this.id = id;
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
                    neon.color = Color.cyan;
                    neon.range = 8f;
                    neon.intensity = 1f;
                    break;
                case Entity.ITEM:
                    neon.color = Color.red;
                    neon.intensity = 4f;
                    break;
                case Entity.SUPPLY:
                    neon.color = Color.white;
                    break;
                default:
                    break;
            }
        }
    }
}
