using UnityEngine;
using System.Collections;

namespace SupHero {

    [System.Serializable]
    public struct HeroData {
        // Hero
        public float health;
        public float armor;
        public float shield;
        public float speed;
        public float shieldReplenishTime;

        public int starterPrimary;
        public int starterSecondary;

        public GameObject prefab;
    }

    [System.Serializable]
    public struct GuardData {
        // Hero
        public float health;
        public float armor;
        public float speed;

        public int starterPrimary;
        public int starterSecondary;

        public GameObject prefab;
    }

    [System.Serializable]
    public class SettingsData {

        public string name = "Main Settings";

        // Fields
        public float turnTime = 120;

        // Players
        public HeroData hero;
        public GuardData guard;

        // Music
        public AudioClip menuClip;
        public AudioClip gameClip;
        public AudioClip resultsClip;
    }
}
