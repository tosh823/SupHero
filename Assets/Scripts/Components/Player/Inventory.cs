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

        public void processDrop(Entity type, int id) {
            switch (type) {
                case Entity.WEAPON:
                    equipWeapon(id);
                    break;
                case Entity.ITEM:
                    equipItem(id);
                    break;
                case Entity.SUPPLY:
                    useSupply(id);
                    break;
                default:
                    break;
            }
        }

        private void equipWeapon(int id) {
            WeaponData weaponData = Data.Instance.getWeaponById(id);
            if (weaponData != null) {
                GameObject instance = Instantiate(weaponData.prefab) as GameObject;
                instance.transform.SetParent(weaponPlacement.transform);
                instance.transform.position = weaponPlacement.transform.position;
                instance.SetActive(false);
                if (weaponData.slot == WeaponSlot.PRIMARY) {
                    Destroy(primary.gameObject);
                    primary = instance.GetComponent<WeaponController>();
                    primary.weapon = weaponData;
                    //owner.drawPrimary();
                }
                else if (weaponData.slot == WeaponSlot.SECONDARY) {
                    Destroy(secondary.gameObject);
                    secondary = instance.GetComponent<WeaponController>();
                    secondary.weapon = weaponData;
                    //owner.drawSecondary();
                }
            }
        }

        private void equipItem(int id) {

        }

        private void useSupply(int id) {

        }
    }
}
