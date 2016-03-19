using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace SupHero.Assets {
    public class SettingsDatabase : ScriptableObject {

        // Storage
        public List<SettingsData> settings;

        // Methods
        public void add(SettingsData setting) {
            settings.Add(setting);
        }

        public void add() {
            SettingsData setting = new SettingsData();
            settings.Add(setting);
        }

        public SettingsData getSettingsAtIndex(int index) {
            return settings[index];
        }

        public void removeSettingsAtIndex(int index) {
            settings.RemoveAt(index);
        }
    }
}
