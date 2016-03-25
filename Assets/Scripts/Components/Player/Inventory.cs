using UnityEngine;
using System.Collections;
using SupHero;
using SupHero.Model;

namespace SupHero.Components {

    public class Inventory : MonoBehaviour {

        // InGame weapon representation
        public GameObject weaponPlacement;
        public GameObject primaryPrefab; // For adjusting models
        public GameObject secondaryPrefab; // For adjusting models

        public WeaponController primary { get; private set; }
        public WeaponController secondary { get; private set; }

        // InGame item representation
        public GameObject firstItemPrefab;
        public GameObject secondItemPrefab;

        public ItemController firstItem { get; private set; }
        public ItemController secondItem { get; private set; }

        private PlayerController owner;

        void Awake() {
            owner = GetComponent<PlayerController>();
        }

        void Start() {
            
        }

        public void setupWeapons() {
            // Primary weapon
            int primaryId = owner.player.primaryId;
            WeaponData primaryData = Data.Instance.getWeaponById(primaryId);
            if (primaryPrefab == null) primaryPrefab = primaryData.prefab;
            GameObject primaryInstance = Instantiate(primaryPrefab) as GameObject;
            primaryInstance.transform.SetParent(weaponPlacement.transform);
            primaryInstance.transform.position = weaponPlacement.transform.position;
            primary = primaryInstance.GetComponent<WeaponController>();
            primary.weapon = primaryData;
            primaryInstance.SetActive(false);

            // Secondary weapon
            int secondaryId = owner.player.secondaryId;
            WeaponData secondaryData = Data.Instance.getWeaponById(secondaryId);
            if (secondaryPrefab == null) secondaryPrefab = secondaryData.prefab;
            GameObject secondaryInstance = Instantiate(secondaryPrefab) as GameObject;
            secondaryInstance.transform.SetParent(weaponPlacement.transform);
            secondaryInstance.transform.position = weaponPlacement.transform.position;
            secondary = secondaryInstance.GetComponent<WeaponController>();
            secondary.weapon = secondaryData;
            secondaryInstance.SetActive(false);
        }

        public void setupItems() {

        }
        
    }
}
