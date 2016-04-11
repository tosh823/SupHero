using SupHero.Components.Weapon;
using UnityEngine;

namespace SupHero.Components.Character {

    public class Inventory : MonoBehaviour {

        // InGame weapon representation
        public Transform rightHand;

        public WeaponController primaryWeapon { get; private set; }
        public WeaponController secondaryWeapon { get; private set; }

        public ItemController firstItem { get; private set; }
        public ItemController secondItem { get; private set; }

        private PlayerController owner;

        void Awake() {
            owner = GetComponent<PlayerController>();
        }

        void Start() {

        }

        public void setupWeapons() {
            // Secondary weapon
            equipWeapon(owner.player.secondaryId);
            // Primary weapon
            equipWeapon(owner.player.primaryId);
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

        public void equipWeapon(int id, bool draw = false) {
            WeaponData weaponData = Data.Instance.getWeaponById(id);
            if (weaponData != null) {
                owner.setAnimator(weaponData.controller);
                GameObject instance = Instantiate(weaponData.prefab, rightHand.position, Quaternion.identity) as GameObject;
                instance.transform.SetParent(rightHand);
                // I spent the whole 06.04.2016 of fixing fucking weapon rotation
                // Still don't know how it works, damn you Unity
                instance.transform.localEulerAngles = weaponData.prefab.transform.rotation.eulerAngles;
                instance.SetActive(false);
                switch (weaponData.slot) {
                    case WeaponSlot.PRIMARY:
                        if (primaryWeapon != null) Destroy(primaryWeapon.gameObject);
                        primaryWeapon = instance.GetComponent<WeaponController>();
                        primaryWeapon.weapon = weaponData;
                        if (draw) owner.drawPrimary();
                        break;
                    case WeaponSlot.SECONDARY:
                        if (secondaryWeapon != null) Destroy(secondaryWeapon.gameObject);
                        secondaryWeapon = instance.GetComponent<WeaponController>();
                        secondaryWeapon.weapon = weaponData;
                        if (draw) owner.drawSecondary();
                        break;
                    default:
                        break;
                }
            }
        }

        public void equipItem(int id) {

        }

        public void useSupply(int id) {

        }
    }
}
