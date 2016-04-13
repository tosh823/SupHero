using UnityEngine;
using System.Collections;

namespace SupHero {

    public enum Theme {
        FOREST,
        CAVE,
        CITY
    }

    [System.Serializable]
    public struct CoverData {
        public string name;
        public float durability;
        public GameObject prefab;
    }

    [System.Serializable]
    public struct InteriorData {
        public string name;
        public GameObject prefab;
    }

    [System.Serializable]
    public struct PlateData {
        public string name;
        public int coverAmount;
        public int interiorAmount;
        public GameObject prefab;
    }

    [System.Serializable]
    public class EnvironmentData {

        public string name = "Forest theme";
        public Theme theme = Theme.FOREST;
        //public GameObject[] covers;
        //public GameObject[] interior;

        public PlateData[] plates;
        public CoverData[] covers;
        public InteriorData[] interiors;

        public EnvironmentData() {

        }
    }
}
