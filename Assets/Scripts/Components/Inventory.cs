using UnityEngine;
using System.Collections;
using SupHero;
using SupHero.Model;

namespace SupHero.Components {

    public class Inventory : MonoBehaviour {

        public GameObject weaponPlacement;
        public GameObject primaryPrefab;
        public GameObject secondaryPrefab;

        public WeaponController primary { get; private set; }
        public WeaponController secondary { get; private set; }

        private PlayerController owner;

        // Use this for initialization
        void Start() {
            owner = GetComponent<PlayerController>();

            if (primaryPrefab == null) {
                primaryPrefab = Data.instance.weapons.getWeaponAtIndex(0).weaponPrefab;
            }
            if (secondaryPrefab == null) {
                secondaryPrefab = Data.instance.weapons.getWeaponAtIndex(1).weaponPrefab;
            }

            GameObject primaryInstance = Instantiate(primaryPrefab) as GameObject;
            primaryInstance.transform.SetParent(weaponPlacement.transform);
            primaryInstance.transform.position = weaponPlacement.transform.position;
            primary = primaryInstance.GetComponent<WeaponController>();
            primaryInstance.SetActive(false);

            GameObject secondaryInstance = Instantiate(secondaryPrefab, weaponPlacement.transform.position, secondaryPrefab.transform.rotation) as GameObject;
            secondaryInstance.transform.SetParent(weaponPlacement.transform);
            secondary = secondaryInstance.GetComponent<WeaponController>();
            secondaryInstance.SetActive(false);
        }

        
    }
}
