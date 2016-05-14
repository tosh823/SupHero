using UnityEngine;
using System.Collections;
using SupHero.Components.Character;

namespace SupHero.Components.Level {

    public enum DropType {
        SIMPLE,
        LIMITED_AMOUNT,
        LIMITED_TIME
    }

    public class Drop : MonoBehaviour {

        public DropType type = DropType.SIMPLE;
        public Entity entity;
        public int id;

        public int amount;
        public float duration;

        protected Light neon;
        protected GameObject dropItem;

        void Start() {
            //neon = GetComponent<Light>();
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
            configureMaterial();
        }

        void Update() {
            transform.Rotate(new Vector3(0f, 20f, 0f) * Time.deltaTime);
        }

        // Creates simple drop that dissolves when taken
        public void createDropSimple(Entity entity, int id) {
            this.entity = entity;
            this.id = id;
            type = DropType.SIMPLE;
        }

        // Creates simple drop that dissolves after certain amount of take
        public void createDropLimitedAmount(Entity entity, int id, int amount) {
            this.entity = entity;
            this.id = id;
            type = DropType.LIMITED_AMOUNT;
            this.amount = amount;
        }

        // Creates simple drop that dissolves after certain time
        public void createDropLimitedTime(Entity entity, int id, float time) {
            this.entity = entity;
            this.id = id;
            type = DropType.LIMITED_TIME;
            duration = time;
            // Create lifetime
            Timer life = gameObject.AddComponent<Timer>();
            life.time = duration;
            life.OnEnd += delegate () {
                Destroy(gameObject);
            };
            life.Launch();
        }

        void OnTriggerEnter(Collider other) {
            Transform incoming = other.gameObject.transform;
            if (incoming.CompareTag(Tags.Player)) {
                // Detecting player to receive drop
                incoming.GetComponent<PlayerController>().receiveDrop(entity, id);
                switch (type) {
                    case DropType.SIMPLE:
                        Destroy(gameObject);
                        break;
                    case DropType.LIMITED_AMOUNT:
                        amount--;
                        if (amount <= 0) Destroy(gameObject);
                        break;
                    case DropType.LIMITED_TIME:
                        Destroy(gameObject);
                        break;
                    default:
                        break;
                }
            }
        }

        protected void createWeaponDrop() {
            WeaponData data = Data.Instance.getWeaponById(id);
            if (data != null) {
                Vector3 position = transform.position;
                position.y += 0.5f;
                dropItem = Instantiate(data.visualPrefab, position, data.visualPrefab.transform.rotation) as GameObject;
                dropItem.transform.SetParent(transform);
            }
            else Destroy(gameObject);
        }

        protected void createItemDrop() {
            ItemData data = Data.Instance.getItemById(id);
            if (data != null) {
                Vector3 position = transform.position;
                position.y += 0.2f;
                dropItem = Instantiate(data.visualPrefab, position, data.visualPrefab.transform.rotation) as GameObject;
                dropItem.transform.SetParent(transform);
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

        void configureMaterial() {
            EnvironmentData data = Data.Instance.getEnvByTheme(Theme.FOREST);
            Renderer render = GetComponent<Renderer>();
            switch (entity) {
                case Entity.WEAPON:
                    render.sharedMaterial = data.materials.weaponMaterial;
                    break;
                case Entity.ITEM:
                    render.sharedMaterial = data.materials.itemMaterial;
                    break;
                case Entity.SUPPLY:
                    render.sharedMaterial = data.materials.supplyMaterial;
                    break;
                default:
                    break;
            }
        }
    }
}
