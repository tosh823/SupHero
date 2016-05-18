using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace SupHero {

    public class CharacterDatabase : ScriptableObject {

        // Storage
        public List<CharactersInfo> lists;

        // Methods
        public void add(CharactersInfo item) {
            lists.Add(item);
        }

        public void add() {
            CharactersInfo item = new CharactersInfo(lists.Count);
            lists.Add(item);
        }

        public CharactersInfo getListAtIndex(int index) {
            return lists[index];
        }

        public void removeListAtIndex(int index) {
            lists.RemoveAt(index);
        }
    }
}
