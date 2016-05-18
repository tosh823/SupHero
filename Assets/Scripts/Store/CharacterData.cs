using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace SupHero {

    public enum Gender {
        MALE,
        FEMALE
    }

    public enum CharColor {
        RED,
        BLUE,
        YELLOW,
        PINK
    }

    [System.Serializable]
    public struct CharacterData {
        public string name;
        public Gender gender;
        public CharColor color;
        public Texture texture;
        public Sprite avatar;
        public Sprite arrow;
    }

    [System.Serializable]
    public class CharactersInfo {

        public string name = "Default list name";
        public int id;

        public GameObject maleChar;
        public GameObject femaleChar;

        public List<CharacterData> chars;

        public CharactersInfo() {

        }

        public CharactersInfo(int id) {
            this.id = id;
        }
    }
}
