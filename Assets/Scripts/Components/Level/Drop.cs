using UnityEngine;
using System.Collections;
using SupHero.Components.Character;

namespace SupHero.Components.Level {
    public class Drop : MonoBehaviour {

        public bool autoDestroy = true;
        public Entity entity;
        public int id;

        protected Light neon;
        protected GameObject dropItem;

        void Start() {
            neon = GetComponent<Light>();
            switch (entity) {
                case Entity.WEAPON:
                    createWeaponDrop();
                    break;
                case Entity.ITEM:
                    createItemDrop();
                    break;
                case Entity.SUPPLY:
                    createSupplyDrop();
                    break;
                default:
                    break;
            }
        }

        void Update() {
            transform.Rotate(new Vector3(0f, 20f, 0f) * Time.deltaTime);
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

        protected void createWeaponDrop() {
            WeaponData data = Data.Instance.getWeaponById(id);
            if (data != null) {
                Vector3 position = transform.position;
                position.y += 1f;
                Quaternion rotation = Random.rotationUniform;
                dropItem = Instantiate(data.prefab, position, rotation) as GameObject;
                dropItem.transform.SetParent(transform);
                configureLight();
            }
            else Destroy(gameObject);
        }

        protected void createItemDrop() {
            ItemData data = Data.Instance.getItemById(id);
            if (data != null) {
                Vector3 position = transform.position;
                position.y += 1f;
                Quaternion rotation = Random.rotationUniform;
                dropItem = Instantiate(data.prefab, position, rotation) as GameObject;
                dropItem.transform.SetParent(transform);
                configureLight();
            }
            else Destroy(gameObject);
        }

        protected void createSupplyDrop() {
            SupplyData data = Data.Instance.getSupplyById(id);
            if (data != null) {
                Vector3 position = transform.position;
                position.y += 1f;
                Quaternion rotation = Quaternion.Euler(-90f, 0f, 90f);
                dropItem = Instantiate(data.prefab, position, rotation) as GameObject;
                dropItem.transform.SetParent(transform);
                configureLight();
            }
            else {
                Destroy(gameObject);
            }
        }

        void configureLight() {
            switch (entity) {
                case Entity.WEAPON:
                    neon.color = Color.cyan;
                    neon.range = 4f;
                    neon.intensity = 1f;
                    break;
                case Entity.ITEM:
                    neon.color = Color.yellow;
                    neon.range = 4f;
                    neon.intensity = 1f;
                    break;
                case Entity.SUPPLY:
                    neon.color = Color.white;
                    neon.range = 4f;
                    neon.intensity = 1f;
                    break;
                default:
                    break;
            }
        }
    }
}
