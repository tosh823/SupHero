using UnityEngine;
using System.Collections;

namespace SupHero.Components {
    public class Drop : MonoBehaviour {

        public GameObject dropItem;

        void Start() {
            dropItem = Instantiate(dropItem) as GameObject;
            dropItem.transform.SetParent(transform);
            dropItem.transform.position = transform.position;
            dropItem.transform.Translate(new Vector3(0f, 0f, 0.5f));
            dropItem.transform.eulerAngles = new Vector3(-90f, 0f, 0f);
        }

        void Update() {
            transform.Rotate(new Vector3(0f, 20f, 0f) * Time.deltaTime);
        }
    }
}
