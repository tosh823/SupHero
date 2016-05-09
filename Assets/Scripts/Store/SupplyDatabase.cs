using UnityEngine;
using System.Collections.Generic;

namespace SupHero {
    public class SupplyDatabase : ScriptableObject {
        // Storage
        public List<SupplyData> supplies;

        // Methods
        public void add(SupplyData supply) {
            supplies.Add(supply);
        }

        public void add() {
            SupplyData supply = new SupplyData(supplies.Count);
            supplies.Add(supply);
        }

        public SupplyData getSupplyAtIndex(int index) {
            return supplies[index];
        }

        public void removeSupplyAtIndex(int index) {
            supplies.RemoveAt(index);
        }
    }
}
