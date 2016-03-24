using UnityEngine;
using System.Collections;

namespace SupHero {
    public static class Constants {

        // Constants of the game

        // Players charasterics
        // Common
        public static float health = 100;
        public static float armor = 100;
        // Hero
        public static float heroSpeed = 5f;
        public static float shield = 500;
        public static float replenishWaitTime = 3f; // In seconds
        // Guard
        public static float guardSpeed = 10f;

        // Game charasterics
        public static int playersCount = 4;
        public static float turnTime = 360f; // In seconds
        public static float turnTimeTest = 30f; // In seconds
        public static float heroDistance = 3f;

        // World charasterics
        public static int numberOfZones = 5;

        public static float reloadTime = 3f;

        public static int capacityLow = 1;
        public static int capacityMedium = 10;
        public static int capacityHigh = 30;
        public static int capacityUnlimited = int.MaxValue;

        public static float damageLow = 5;
        public static float damageMedium = 10;
        public static float damageHigh = 20;
        public static float damageVeryHigh = 50;

        public static float durablityWeak = 50;
        public static float durablityNormal = 100;
        public static float durablityStrong = 200;
        public static float durablityImmortal = float.MaxValue;

        public static float rateLow = 0.5f;
        public static float rateNormal = 0.25f;
        public static float rateFast = 0.1f;

        public static float rangeClose = 1f;
        public static float rangeShort = 10f;
        public static float rangeLong = 20f;
    }
}
