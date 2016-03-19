using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using SupHero;
using SupHero.Assets;
using SupHero.Model;

namespace SupHero.Components {
    public class Data : MonoBehaviour {

        public static Data instance = null;
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

        public SettingsData getMainSettings() {
            if (settings != null) return settings.getSettingsAtIndex(0);
            else return null;
        }
    }
}
