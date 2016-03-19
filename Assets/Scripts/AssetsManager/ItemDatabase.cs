using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace SupHero.Assets {
    public class ItemDatabase : ScriptableObject {

        // Storage
        public List<ItemData> items;

        // Methods
        public void add(ItemData item) {
            items.Add(item);
        }

        public void add() {
            ItemData item = new ItemData();
            items.Add(item);
        }

        public ItemData getItemAtIndex(int index) {
            return items[index];
        }

        public void removeItemAtIndex(int index) {
            items.RemoveAt(index);
        }
    }
}
