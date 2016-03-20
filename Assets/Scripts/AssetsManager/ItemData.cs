using UnityEngine;
using System.Collections;

namespace SupHero {

    [System.Serializable]
    public class ItemData {
        // Fields
        public int id;
        public string name = "New item";
        public string description = "Default item description";
        public GameObject itemPrefab;

        public ItemData() {

        }

        public ItemData(int id) {
            this.id = id;
        }
    }
}
