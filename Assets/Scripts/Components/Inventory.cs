using UnityEngine;
using System.Collections;
using SupHero;
using SupHero.Model;

namespace SupHero.Components {

    public class Inventory : MonoBehaviour {

        public GameObject weaponPlacement;
        public GameObject primaryPrefab; // For adjusting models
        public GameObject secondaryPrefab; // For adjusting models

        public WeaponController primary { get; private set; }
        public WeaponController secondary { get; private set; }

        private PlayerController owner;

        // Use this for initialization
        void Start() {
            owner = GetComponent<PlayerController>();

            setupWeapons();
        }

        public void setupWeapons() {
            // Primary weapon
            int primaryId = owner.player.primaryId;
            WeaponData primaryData = Data.instance.getWeaponById(primaryId);
            if (primaryPrefab == null) primaryPrefab = primaryData.weaponPrefab;
            GameObject primaryInstance = Instantiate(primaryPrefab) as GameObject;
            primaryInstance.transform.SetParent(weaponPlacement.transform);
            primaryInstance.transform.position = weaponPlacement.transform.position;
            primary = primaryInstance.GetComponent<WeaponController>();
            primary.weapon = primaryData;
            primaryInstance.SetActive(false);

            int secondaryId = owner.player.primaryId;
            WeaponData secondaryData = Data.instance.getWeaponById(primaryId);
            if (secondaryPrefab == null) secondaryPrefab = secondaryData.weaponPrefab;
            GameObject secondaryInstance = Instantiate(secondaryPrefab) as GameObject;
            secondaryInstance.transform.SetParent(weaponPlacement.transform);
            secondaryInstance.transform.position = weaponPlacement.transform.position;
            secondary = secondaryInstance.GetComponent<WeaponController>();
            secondary.weapon = primaryData;
            secondaryInstance.SetActive(false);
        }
        
    }
}
