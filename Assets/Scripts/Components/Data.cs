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
        public SupplyDatabase supplyDB;
        public CharacterDatabase charDB;
        public SettingsData mainSettings {
            get {
                if (settings != null) return settings.getSettingsAtIndex(0);
                else return null;
            }
        }
        public CharactersInfo mainChars {
            get {
                if (charDB != null) return charDB.getListAtIndex(0);
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

        public CharacterData getRandomChar() {
            return Utils.getRandomElement(mainChars.chars);
        }

        public CharacterData getCharByGenderColor(Gender gender, CharColor color) {
            CharactersInfo list = mainChars;
            CharacterData data = list.chars.Find(x => (x.gender == gender && x.color == color));
            return data;
        }

        public CharacterData getCharByGender(Gender gender) {
            CharactersInfo list = mainChars;
            List<CharacterData> findings = list.chars.FindAll(x => (x.gender == gender));
            return Utils.getRandomElement(findings);
        }

        public CharacterData getCharByColor(CharColor color) {
            CharactersInfo list = mainChars;
            List<CharacterData> findings = list.chars.FindAll(x => (x.color == color));
            return Utils.getRandomElement(findings);
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

        public int getRandomItemId() {
            int id = Utils.getRandomRange(0, itemsDB.items.Count);
            return id;
        }

        public SupplyData getSupplyById(int id) {
            foreach (SupplyData data in supplyDB.supplies) {
                if (data.id == id) {
                    return data;
                }
            }
            return null;
        }

        public int getRandomSupplyId() {
            int id = Utils.getRandomRange(0, supplyDB.supplies.Count);
            return id;
        }

        public void createSession() {
            string[] gamepads = Input.GetJoystickNames();
            session.players = new List<Player>();
            // Firstly create a hero
            Hero hero = new Hero(1);
            if (gamepads.Length >= 4) {
                hero.inputType = InputType.GAMEPAD;
                hero.gamepadNumber = 1;
                hero.gamepadName = gamepads[0];
            }
            else {
                hero.inputType = InputType.KEYBOARD;
                hero.gamepadNumber = 0;
                hero.gamepadName = "None";
            }
            hero.character = Instance.getRandomChar();
            session.players.Add(hero);
            // Then, create guards
            for (int index = 1; index < 4; index++) {
                Guard guard = new Guard(index + 1);
                if (gamepads.Length >= (hero.gamepadNumber + index)) {
                    guard.inputType = InputType.GAMEPAD;
                    guard.gamepadNumber = hero.gamepadNumber + index;
                    guard.gamepadName = gamepads[hero.gamepadNumber + index - 1];
                }
                else {
                    guard.inputType = InputType.NONE;
                    guard.gamepadNumber = 0;
                    guard.gamepadName = "None";
                }
                guard.character = Instance.getRandomChar();
                session.players.Add(guard);
            }
        }

        public void updateSession(List<Player> players) {
            session.players = players;
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
