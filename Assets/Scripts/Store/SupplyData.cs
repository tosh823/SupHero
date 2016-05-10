using UnityEngine;
using System.Collections;

namespace SupHero {

    [System.Serializable]
    public class SupplyData {

        public string name = "Default supply";
        public string description = "Default description";
        public int id;
        public GameObject prefab;

        public SupplyData() {

        }

        public SupplyData(int id) {
            this.id = id;
        }
    }
}
