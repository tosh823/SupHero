using UnityEngine;
using System.Collections;

namespace SupHero {
    public static class Constants {

        // Class for different constants of the game

        // Players charasterics
        public static int defaultHealth = 100;
        public static int defaultArmor = 100;
        public static int defaultShield = 100;
        public static float defaultHeroSpeed = 5f;
        public static float defaultGuardSpeed = 10f;

        // Game charasterics
        public static int playersCount = 4;
        public static float turnTime = 360f;
        public static float turnTimeTest = 30f;

        // World charasterics
        public static int numberOfZones = 5;

        public static int damageLow = 5;
        public static int damageMedium = 10;
        public static int damageHigh = 20;
        public static int damageVeryHigh = 50;

        public static int durablityWeak = 50;
        public static int durablityNormal = 100;
        public static int durablityStrong = 200;
        public static int durablityImmortal = int.MaxValue;

        public static float rateLow = 0.5f;
        public static float rateNormal = 0.25f;
        public static float rateFast = 0.1f;

        public static float rangeClose = 1f;
        public static float rangeShort = 10f;
        public static float rangeLong = 20f;
    }
}
