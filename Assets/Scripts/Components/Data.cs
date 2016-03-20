using UnityEngine;

namespace SupHero {
    public class Data : MonoBehaviour {

        public static Data instance = null;
        public WeaponDatabase weaponsDB;
        public ItemDatabase itemsDB;
        public SettingsDatabase settings;
        public SettingsData mainSettings {
            get {
                if (settings != null) return settings.getSettingsAtIndex(0);
                else return null;
            }
        }

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

        public WeaponData getWeaponById(int id) {
            foreach (WeaponData data in weaponsDB.weapons) {
                if (data.id == id) {
                    return data;
                }
            }
            return null;
        }

        public ItemData getItemById(int id) {
            foreach (ItemData data in itemsDB.items) {
                if (data.id == id) {
                    return data;
                }
            }
            return null;
        }
    }
}
