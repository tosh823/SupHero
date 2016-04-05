using UnityEngine;
using System.Collections.Generic;

namespace SupHero {
    public class EnvironmentDatabase : ScriptableObject {

        // Storage
        public List<EnvironmentData> environments;

        // Methods
        public void add(EnvironmentData environment) {
            environments.Add(environment);
        }

        public void add() {
            EnvironmentData environment = new EnvironmentData();
            environments.Add(environment);
        }

        public EnvironmentData getEnvAtIndex(int index) {
            return environments[index];
        }

        public void removeEnvAtIndex(int index) {
            environments.RemoveAt(index);
        }
    }
}
