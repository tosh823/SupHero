using UnityEngine;
using System.Collections;

namespace SupHero.Components {
    public class Drop : MonoBehaviour {

        public GameObject dropItem;
        public Entity entity;
        public int id;

        void Start() {
            dropItem = Instantiate(dropItem) as GameObject;
            dropItem.transform.SetParent(transform);
            dropItem.transform.position = transform.position;
            dropItem.transform.eulerAngles = new Vector3(-90f, 0f, 0f);
        }

        void Update() {
            transform.Rotate(new Vector3(0f, 20f, 0f) * Time.deltaTime);
        }

        void OnTriggerEnter(Collider other) {
            Transform incoming = other.gameObject.transform;
            if (incoming.CompareTag(Tags.Player)) {
                // Detecting player to receive drop
                incoming.GetComponent<PlayerController>().receiveDrop(entity, id);
                Destroy(gameObject);
            }
        }
    }
}
