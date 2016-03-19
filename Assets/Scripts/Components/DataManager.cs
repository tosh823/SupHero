using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using SupHero;
using SupHero.Assets;

namespace SupHero.Components {
    public class DataManager : MonoBehaviour {

        public static DataManager instance = null;
        public WeaponDatabase weapons;
        public ItemDatabase items;
        public SettingsDatabase settings;

        // Singleton realization
        void Awake() {
            if (instance == null) {
                instance = this;
            }
            else if (instance != this) {
                Destroy(gameObject);
            }
            DontDestroyOnLoad(gameObject);
        }

        // Use this for initialization
        void Start() {

        }

        // Update is called once per frame
        void Update() {

        }
    }
}
