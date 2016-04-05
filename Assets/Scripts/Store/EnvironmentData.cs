using UnityEngine;
using System.Collections;

namespace SupHero {

    public enum Theme {
        FOREST,
        CAVE,
        CITY
    }

    [System.Serializable]
    public class EnvironmentData {

        public string name = "Forest theme";
        public Theme theme = Theme.FOREST;
        public GameObject[] covers;
        public GameObject[] interior;

        public EnvironmentData() {

        }
    }
}
