using UnityEngine;
using System.Collections;

namespace SupHero {

    public enum BodySlot {
        HEAD,
        NOSE,
        LEFT_HAND,
        LEFT_FOREARM,
        RIGHT_HAND,
        RIGHT_FOREARM,
        CHEST,
        BACK,
        LEFT_LEG,
        LEFT_SHIN,
        RIGHT_LEG,
        RIGHT_SHIN,
        NONE
    }

    public enum ItemSlot {
        FIRST,
        SECOND
    }

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
        public bool hasVisual = true;

        public ItemSlot slot;

        public bool hasPassive = false;
        public PassiveAbility passiveData;
        public bool hasActive = false;
        public ActiveAbility activeData;

        public GameObject prefab;
        public GameObject visualPrefab; // Jeez >__<

        public BodySlot[] placement;

        public AudioClip activationSound;
        public Sprite image;

        public ItemData() {

        }

        public ItemData(int id) {
            this.id = id;
        }
    }
}
