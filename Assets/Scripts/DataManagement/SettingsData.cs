using UnityEngine;
using System.Collections;

namespace SupHero {

    [System.Serializable]
    public class SettingsData {

        public string name = "Main Settings";

        // Fields
        public float turnTime = 120;

        // Players
        public int starterPrimary = 0;
        public int starterSecondary = 1;

        // Hero
        public float heroHealth = 100;
        public float heroArmor = 100;
        public float heroShield = 500;
        public float heroSpeed = 6;
        public float shieldReplenishTime = 10;

        // Guard
        public float guardHealth = 50;
        public float guardArmor = 100;
        public float guardSpeed = 8;
    }
}
