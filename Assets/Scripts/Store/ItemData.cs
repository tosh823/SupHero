using UnityEngine;
using System.Collections;

namespace SupHero {

    [System.Serializable]
    public struct PassiveAbility {
        public string name;
        public string description;
        public float value;
        public float range;
        public float perSecond;
    }

    [System.Serializable]
    public struct ActiveAbility {
        public string name;
        public string description;
        public float value;
        public float duration;
        public float cooldown;
        public float range;
        public float perSecond;
    }

    [System.Serializable]
    public class ItemData {
        // Fields
        public int id;
        public string name = "New item";
        public string description = "Default item description";

        public bool hasPassive = false;
        public PassiveAbility passiveData;
        public bool hasActive = false;
        public ActiveAbility activeData;

        public GameObject prefab;

        public AudioClip activationSound;

        public ItemData() {

        }

        public ItemData(int id) {
            this.id = id;
        }
    }
}
