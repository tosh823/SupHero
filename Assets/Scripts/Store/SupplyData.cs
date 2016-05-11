using UnityEngine;
using System.Collections;

namespace SupHero {

	public enum SupplyType {
		HEALTH,
		ARMOR
	}

    [System.Serializable]
    public class SupplyData {

        public string name = "Default supply";
        public string description = "Default description";
        public float id;
		public SupplyType kind;
		public float value;
		public float duration;
		public float perSecond;

        public GameObject prefab;
		public AudioClip sound;

        public SupplyData() {

        }

        public SupplyData(int id) {
            this.id = id;
        }
    }
}
