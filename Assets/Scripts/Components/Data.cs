using System.Collections.Generic;
using UnityEngine;
using SupHero.Model;

namespace SupHero {

    public enum Entity {
        WEAPON,
        ITEM,
        SUPPLY,
        NONE
    }

    public struct SessionData {
        public List<Player> players;
    }

    public class Data : MonoBehaviour {

        public static Data Instance = null;
        public WeaponDatabase weaponsDB;
        public ItemDatabase itemsDB;
        public EnvironmentDatabase envDB;
        public SettingsDatabase settings;
        public SettingsData mainSettings {
            get {
                if (settings != null) return settings.getSettingsAtIndex(0);
                else return null;
            }
        }

        public SessionData session;

        // Singleton realization
        void Awake() {
            if (Instance == null) {
                Instance = this;
            }
            else if (Instance != this) {
                Destroy(gameObject);
            }
            DontDestroyOnLoad(gameObject);
        }

        public WeaponData getWeaponById(int id) {
            foreach (WeaponData data in weaponsDB.weapons) {
                if (data.id == id) {
                    return data;
                }
            }
            return null;
        }

        public int getRandomWeaponId() {
            int id = Utils.getRandomRange(2, weaponsDB.weapons.Count);
            return id;
        }

        public EnvironmentData getEnvByTheme(Theme theme) {
            EnvironmentData data = envDB.environments.Find(x => x.theme == theme);
            return data;
        }

        public ItemData getItemById(int id) {
            foreach (ItemData data in itemsDB.items) {
                if (data.id == id) {
                    return data;
                }
            }
            return null;
        }

        public Dictionary<string, int> getStatistics() {
            Dictionary<string, int> stats = new Dictionary<string, int>();
            foreach (Player player in session.players) {
                stats.Add(player.playerName, player.points);
            }
            return stats;
        }
    }
}
