using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;

namespace SupHero.Assets {

    public class DataEditor : EditorWindow {

        private enum SHOW {
            NOTHING,
            SETTINGS,
            WEAPONS,
            ITEMS
        }

        public WeaponDatabase weaponDB;
        public ItemDatabase itemDB;
        public SettingsDatabase settingsDB;
        public string dir;

        private int viewIndex;
        private SHOW toShow;

        [MenuItem("Window/Data Editor %#e")]
        static void Init() {
            GetWindow(typeof(DataEditor), false, "Data", true);
        }

        void OnEnable() {
            dir = "Assets/Data/";
            toShow = SHOW.NOTHING;
            viewIndex = 1;
            EditorPrefs.SetString("ObjectPath", dir);

            if (EditorPrefs.HasKey("SettingsPath")) {
                string path = EditorPrefs.GetString("SettingsPath");
                settingsDB = AssetDatabase.LoadAssetAtPath<SettingsDatabase>(path);
            }
            if (EditorPrefs.HasKey("ItemsPath")) {
                string path = EditorPrefs.GetString("ItemsPath");
                itemDB = AssetDatabase.LoadAssetAtPath<ItemDatabase>(path);
            }
            if (EditorPrefs.HasKey("WeaponsPath")) {
                string path = EditorPrefs.GetString("WeaponsPath");
                weaponDB = AssetDatabase.LoadAssetAtPath<WeaponDatabase>(path);
            }
        }

        void OnGUI() {
            GUILayout.BeginVertical();
            GUILayout.Label("SHT Data Editor", EditorStyles.boldLabel);
            if (GUILayout.Button("Game Settings", GUILayout.ExpandWidth(false))) {
                EditorUtility.FocusProjectWindow();
                Selection.activeObject = settingsDB;
                toShow = SHOW.SETTINGS;
            }
            if (GUILayout.Button("Weapons DB", GUILayout.ExpandWidth(false))) {
                EditorUtility.FocusProjectWindow();
                Selection.activeObject = weaponDB;
                toShow = SHOW.WEAPONS;
            }
            if (GUILayout.Button("Items DB", GUILayout.ExpandWidth(false))) {
                EditorUtility.FocusProjectWindow();
                Selection.activeObject = itemDB;
                toShow = SHOW.ITEMS;
            }
            GUILayout.EndVertical();

            GUILayout.Space(20);

            switch (toShow) {
                case SHOW.SETTINGS:
                    if (settingsDB == null) {
                        GUILayout.BeginHorizontal();
                        GUILayout.Space(10);
                        if (GUILayout.Button("Create New Settings List", GUILayout.ExpandWidth(false))) {
                            createNewSettingsDB();
                        }
                        /*if (GUILayout.Button("Open Existing Item List", GUILayout.ExpandWidth(false))) {
                            OpenItemList();
                        }*/
                        GUILayout.EndHorizontal();
                    }
                    GUILayout.Space(20);
                    if (settingsDB != null) {
                        GUILayout.BeginHorizontal();
                        GUILayout.Space(10);
                        if (GUILayout.Button("Prev", GUILayout.ExpandWidth(false))) {
                            if (viewIndex > 1)
                                viewIndex--;
                        }
                        GUILayout.Space(5);
                        if (GUILayout.Button("Next", GUILayout.ExpandWidth(false))) {
                            if (viewIndex < settingsDB.settings.Count) {
                                viewIndex++;
                            }
                        }
                        GUILayout.Space(60);
                        if (GUILayout.Button("Add Item", GUILayout.ExpandWidth(false))) {
                            settingsDB.add();
                            viewIndex = itemDB.items.Count;
                        }
                        if (GUILayout.Button("Delete Item", GUILayout.ExpandWidth(false))) {
                            settingsDB.removeSettingsAtIndex(viewIndex - 1);
                            viewIndex = settingsDB.settings.Count;
                        }
                        GUILayout.EndHorizontal();
                        if (settingsDB.settings.Count > 0) {
                            GUILayout.BeginHorizontal();
                            viewIndex = Mathf.Clamp(EditorGUILayout.IntField("Current Item", viewIndex, GUILayout.ExpandWidth(false)), 1, settingsDB.settings.Count);
                            EditorGUILayout.LabelField("of   " + settingsDB.settings.Count.ToString() + "  items", "", GUILayout.ExpandWidth(false));
                            GUILayout.EndHorizontal();
                            int dbIndex = viewIndex - 1;
                            settingsDB.getSettingsAtIndex(dbIndex).turnTime = EditorGUILayout.FloatField("Turn Time", settingsDB.getSettingsAtIndex(dbIndex).turnTime);
                            GUILayout.Space(10);
                        }
                        else {
                            GUILayout.Label("The settings list is empty");
                        }
                    }
                    break;
                case SHOW.WEAPONS:
                    if (weaponDB == null) {
                        GUILayout.BeginHorizontal();
                        GUILayout.Space(10);
                        if (GUILayout.Button("Create New Weapon List", GUILayout.ExpandWidth(false))) {
                            createNewWeaponDB();
                        }
                        /*if (GUILayout.Button("Open Existing Weapon List", GUILayout.ExpandWidth(false))) {
                            OpenItemList();
                        }*/
                        GUILayout.EndHorizontal();
                    }

                    GUILayout.Space(20);

                    if (weaponDB != null) {
                        GUILayout.BeginHorizontal();
                        GUILayout.Space(10);
                        if (GUILayout.Button("Prev", GUILayout.ExpandWidth(false))) {
                            if (viewIndex > 1)
                                viewIndex--;
                        }
                        GUILayout.Space(5);
                        if (GUILayout.Button("Next", GUILayout.ExpandWidth(false))) {
                            if (viewIndex < weaponDB.weapons.Count) {
                                viewIndex++;
                            }
                        }
                        GUILayout.Space(60);
                        if (GUILayout.Button("Add Item", GUILayout.ExpandWidth(false))) {
                            weaponDB.add();
                            viewIndex = weaponDB.weapons.Count;
                        }
                        if (GUILayout.Button("Delete Item", GUILayout.ExpandWidth(false))) {
                            weaponDB.removeWeaponAtIndex(viewIndex - 1);
                            viewIndex = weaponDB.weapons.Count;
                        }
                        GUILayout.EndHorizontal();
                        if (weaponDB.weapons.Count > 0) {
                            GUILayout.BeginHorizontal();
                            viewIndex = Mathf.Clamp(EditorGUILayout.IntField("Current Item", viewIndex, GUILayout.ExpandWidth(false)), 1, weaponDB.weapons.Count);
                            EditorGUILayout.LabelField("of   " + weaponDB.weapons.Count.ToString() + "  items", "", GUILayout.ExpandWidth(false));
                            GUILayout.EndHorizontal();
                            int dbIndex = viewIndex - 1;
                            weaponDB.getWeaponAtIndex(dbIndex).name = EditorGUILayout.TextField("Weapon Name", weaponDB.getWeaponAtIndex(dbIndex).name as string);
                            weaponDB.getWeaponAtIndex(dbIndex).description = EditorGUILayout.TextField("Weapon Description", weaponDB.getWeaponAtIndex(dbIndex).description as string);
                            weaponDB.getWeaponAtIndex(dbIndex).damage = EditorGUILayout.FloatField("Weapon Damage", weaponDB.getWeaponAtIndex(dbIndex).damage);
                            weaponDB.getWeaponAtIndex(dbIndex).rate = EditorGUILayout.FloatField("Weapon Rate", weaponDB.getWeaponAtIndex(dbIndex).rate);
                            weaponDB.getWeaponAtIndex(dbIndex).weaponPrefab = EditorGUILayout.ObjectField("Weapon Prefab", weaponDB.getWeaponAtIndex(dbIndex).weaponPrefab, typeof(GameObject), true) as GameObject;
                            GUILayout.Space(10);
                        }
                        else {
                            GUILayout.Label("The weapon list is empty");
                        }
                    }
                    break;
                case SHOW.ITEMS:
                    if (itemDB == null) {
                        GUILayout.BeginHorizontal();
                        GUILayout.Space(10);
                        if (GUILayout.Button("Create New Item List", GUILayout.ExpandWidth(false))) {
                            createNewItemDB();
                        }
                        /*if (GUILayout.Button("Open Existing Item List", GUILayout.ExpandWidth(false))) {
                            OpenItemList();
                        }*/
                        GUILayout.EndHorizontal();
                    }
                    GUILayout.Space(20);
                    if (itemDB != null) {
                        GUILayout.BeginHorizontal();
                        GUILayout.Space(10);
                        if (GUILayout.Button("Prev", GUILayout.ExpandWidth(false))) {
                            if (viewIndex > 1)
                                viewIndex--;
                        }
                        GUILayout.Space(5);
                        if (GUILayout.Button("Next", GUILayout.ExpandWidth(false))) {
                            if (viewIndex < itemDB.items.Count) {
                                viewIndex++;
                            }
                        }
                        GUILayout.Space(60);
                        if (GUILayout.Button("Add Item", GUILayout.ExpandWidth(false))) {
                            itemDB.add();
                            viewIndex = itemDB.items.Count;
                        }
                        if (GUILayout.Button("Delete Item", GUILayout.ExpandWidth(false))) {
                            itemDB.removeItemAtIndex(viewIndex - 1);
                            viewIndex = itemDB.items.Count;
                        }
                        GUILayout.EndHorizontal();
                        if (itemDB.items.Count > 0) {
                            GUILayout.BeginHorizontal();
                            viewIndex = Mathf.Clamp(EditorGUILayout.IntField("Current Item", viewIndex, GUILayout.ExpandWidth(false)), 1, itemDB.items.Count);
                            EditorGUILayout.LabelField("of   " + itemDB.items.Count.ToString() + "  items", "", GUILayout.ExpandWidth(false));
                            GUILayout.EndHorizontal();
                            int dbIndex = viewIndex - 1;
                            itemDB.getItemAtIndex(dbIndex).name = EditorGUILayout.TextField("Item Name", itemDB.getItemAtIndex(dbIndex).name as string);
                            itemDB.getItemAtIndex(dbIndex).description = EditorGUILayout.TextField("Weapon Description", itemDB.getItemAtIndex(dbIndex).description as string);
                            itemDB.getItemAtIndex(dbIndex).itemPrefab = EditorGUILayout.ObjectField("Weapon Prefab", itemDB.getItemAtIndex(dbIndex).itemPrefab, typeof(GameObject), true) as GameObject;
                            GUILayout.Space(10);
                        }
                        else {
                            GUILayout.Label("The item list is empty");
                        }
                    }
                    break;
                case SHOW.NOTHING:
                    GUILayout.Label("Press on content button you want to edit");
                    break;
                default:
                    break;
            }
            if (GUI.changed) {
                if (weaponDB != null) EditorUtility.SetDirty(weaponDB);
                if (itemDB != null) EditorUtility.SetDirty(itemDB);
                if (settingsDB != null) EditorUtility.SetDirty(settingsDB);
            }
        }

        void createNewWeaponDB(){
            // There is no overwrite protection here!
            // There is No "Are you sure you want to overwrite your existing object?" if it exists.
            // This should probably get a string from the user to create a new name and pass it ...
            viewIndex = 1;
            weaponDB = CreateInstance<WeaponDatabase>();
            string destintation = dir + "WeaponDB.asset";
            AssetDatabase.CreateAsset(weaponDB, destintation);
            AssetDatabase.SaveAssets();
            if (weaponDB) {
                weaponDB.weapons = new List<WeaponData>();
                string relPath = AssetDatabase.GetAssetPath(weaponDB);
                EditorPrefs.SetString("WeaponsPath", relPath);
                EditorUtility.FocusProjectWindow();
                Selection.activeObject = weaponDB;
            }
        }

        void createNewItemDB() {
            // There is no overwrite protection here!
            // There is No "Are you sure you want to overwrite your existing object?" if it exists.
            // This should probably get a string from the user to create a new name and pass it ...
            viewIndex = 1;
            itemDB = CreateInstance<ItemDatabase>();
            string destintation = dir + "ItemDB.asset";
            AssetDatabase.CreateAsset(itemDB, destintation);
            AssetDatabase.SaveAssets();
            if (itemDB) {
                itemDB.items = new List<ItemData>();
                string relPath = AssetDatabase.GetAssetPath(itemDB);
                EditorPrefs.SetString("ItemsPath", relPath);
                EditorUtility.FocusProjectWindow();
                Selection.activeObject = itemDB;
            }
        }

        void createNewSettingsDB() {
            // There is no overwrite protection here!
            // There is No "Are you sure you want to overwrite your existing object?" if it exists.
            // This should probably get a string from the user to create a new name and pass it ...
            viewIndex = 1;
            settingsDB = CreateInstance<SettingsDatabase>();
            string destintation = dir + "SettingsDB.asset";
            AssetDatabase.CreateAsset(settingsDB, destintation);
            AssetDatabase.SaveAssets();
            if (settingsDB) {
                settingsDB.settings = new List<SettingsData>();
                string relPath = AssetDatabase.GetAssetPath(settingsDB);
                EditorPrefs.SetString("SettingsPath", relPath);
                EditorUtility.FocusProjectWindow();
                Selection.activeObject = settingsDB;
            }
        }

        /*void OpenItemList() {
            string absPath = EditorUtility.OpenFilePanel("Select Inventory Item List", "", "");
            if (absPath.StartsWith(Application.dataPath)) {
                string relPath = absPath.Substring(Application.dataPath.Length - "Assets".Length);
                weaponDB = AssetDatabase.LoadAssetAtPath(relPath, typeof(WeaponDatabase)) as WeaponDatabase;
                if (weaponDB.weapons == null)
                    weaponDB.weapons = new List<WeaponData>();
                if (weaponDB) {
                    EditorPrefs.SetString("ObjectPath", relPath);
                }
            }
        }*/
    }
}
