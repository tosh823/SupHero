using SupHero.Components.Weapon;
using SupHero.Components.Item;
using UnityEngine;

namespace SupHero.Components.Character {

    public class Inventory : MonoBehaviour {

        // InGame weapon representation
        public Transform rightHand;
        // Parts of the body
        public Transform hairPlace;
        public Transform eyesPlace;
        public Transform leftForearmPlace;
        public Transform leftHandPlace;
        public Transform rightForearmPlace;
        public Transform rightHandPlace;
        public Transform chestPlace;
        public Transform backPlace;
        public Transform leftLegPlace;
        public Transform leftShinPlace;
        public Transform rightLegPlace;
        public Transform rightShinPlace;

        public WeaponController primaryWeapon { get; private set; }
        public WeaponController secondaryWeapon { get; private set; }

        public ItemController firstItem { get; private set; }
        public ItemController secondItem { get; private set; }

        // For item tests
        public int firstItemId;
        public int secondItemId;

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
            if (firstItemId >= 0) {
                firstItem = equipItem(firstItemId);
            }
            if (secondItemId >= 0 && secondItemId != firstItemId) {
                secondItem = equipItem(secondItemId);
            }
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
                //owner.setAnimator(weaponData.controller);
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

        public ItemController equipItem(int id) {
            ItemData itemData = Data.Instance.getItemById(id);
            if (itemData != null) {
                ItemController ic = null;
                foreach (BodySlot slot in itemData.placement) {
                    Transform placement = getPlacement(slot);
                    GameObject instance = Instantiate(itemData.prefab, placement.position, Quaternion.identity) as GameObject;
                    instance.transform.SetParent(placement);
                    instance.transform.localEulerAngles = itemData.prefab.transform.rotation.eulerAngles;
                    if (slot == BodySlot.NONE) instance.SetActive(false);
                    ic = instance.GetComponent<ItemController>();
                    ic.owner = owner;
                    ic.item = itemData;
                }
                return ic;
            }
            else return null;
        }

        public void useSupply(int id) {

        }

        private Transform getPlacement(BodySlot slot) {
            switch (slot) {
                case BodySlot.BACK:
                    return backPlace;
                case BodySlot.CHEST:
                    return chestPlace;
                case BodySlot.HEAD:
                    return hairPlace;
                case BodySlot.LEFT_FOREARM:
                    return leftForearmPlace;
                case BodySlot.LEFT_HAND:
                    return leftHandPlace;
                case BodySlot.LEFT_LEG:
                    return leftLegPlace;
                case BodySlot.LEFT_SHIN:
                    return leftShinPlace;
                case BodySlot.NOSE:
                    return eyesPlace;
                case BodySlot.RIGHT_FOREARM:
                    return rightForearmPlace;
                case BodySlot.RIGHT_HAND:
                    return rightHand;
                case BodySlot.RIGHT_LEG:
                    return rightLegPlace;
                case BodySlot.RIGHT_SHIN:
                    return rightShinPlace;
                default:
                    return gameObject.transform;
            }
        }
    }
}
