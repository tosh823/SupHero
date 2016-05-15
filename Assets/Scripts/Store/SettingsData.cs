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
        public float minImpulse;
        public float minRadius;
        public float shieldUnitPerCharge;
        public float maxImpulse;
        public float maxRadius;

        public float spawnDistance;
        public float viewRadius;

        public int starterPrimary;

        public GameObject prefab;
        public GameObject shieldPrefab;
        public GameObject shieldHitPrefab;
        public GameObject headsUpDisplayPrefab;
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
        public GameObject headsUpDisplayPrefab;
    }

    [System.Serializable]
    public struct Points {
        public int fragHero;
        public int fragGuard;
        public int plateFinished;
    }

    [System.Serializable]
    public class SettingsData {

        public string name = "Main Settings";

        // Fields
        public float turnTime = 120;
        public Points points;

        // Players
        public HeroData hero;
        public GuardData guard;

        // Music
        public AudioClip menuClip;
        public AudioClip gameClip;
        public AudioClip resultsClip;
    }
}
