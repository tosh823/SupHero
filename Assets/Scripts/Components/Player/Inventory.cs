using SupHero.Components.Weapon;
using SupHero.Components.Item;
using UnityEngine;
using System.Collections.Generic;

namespace SupHero.Components.Character {

    public class Inventory : MonoBehaviour {

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
        private List<Transform> firstItemVisuals;
        public ItemController secondItem { get; private set; }
        private List<Transform> secondItemVisuals;

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
            firstItemVisuals = new List<Transform>();
            secondItemVisuals = new List<Transform>();
            if (owner.player.firstItemId >= 0) {
                equipItem(owner.player.firstItemId);
            }
            if (owner.player.secondItemId >= 0 && owner.player.secondItemId != firstItemId) {
                equipItem(owner.player.secondItemId);
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
            // If we are hero, we can't equip melee weapon
            if (weaponData.slot == WeaponSlot.SECONDARY && owner.isHero()) return;
            if (weaponData != null) {
                if (weaponData.weaponType == WeaponType.LEFT_HAND || weaponData.weaponType == WeaponType.RIGHT_HAND) {
                    Transform placement = ((weaponData.weaponType == WeaponType.RIGHT_HAND) ? rightHandPlace : leftHandPlace);
                    GameObject instance = Instantiate(weaponData.prefab, placement.position, Quaternion.identity) as GameObject;
                    instance.transform.SetParent(placement);
                    // I spent the whole 06.04.2016 of fixing fucking weapon rotation
                    // Still don't know how it works, damn you Unity
                    instance.transform.localEulerAngles = weaponData.prefab.transform.rotation.eulerAngles;
                    instance.SetActive(false);
                    switch (weaponData.slot) {
                        case WeaponSlot.PRIMARY:
                            if (primaryWeapon != null) primaryWeapon.unequipWeapon();
                            primaryWeapon = instance.GetComponent<WeaponController>();
                            primaryWeapon.weapon = weaponData;
                            if (draw) owner.drawPrimary();
                            break;
                        case WeaponSlot.SECONDARY:
                            if (secondaryWeapon != null) secondaryWeapon.unequipWeapon();
                            secondaryWeapon = instance.GetComponent<WeaponController>();
                            secondaryWeapon.weapon = weaponData;
                            if (draw) owner.drawSecondary();
                            break;
                        default:
                            break;
                    }
                }
                else if (weaponData.weaponType == WeaponType.BOTH) {
                    // For the sake of duettes
                    // Left instance
                    GameObject leftGun = Instantiate(weaponData.prefab, leftHandPlace.position, Quaternion.identity) as GameObject;
                    leftGun.transform.SetParent(leftHandPlace);
                    leftGun.transform.localEulerAngles = weaponData.prefab.transform.rotation.eulerAngles;
                    WeaponController leftWC = leftGun.GetComponent<WeaponController>();
                    leftWC.weapon = weaponData;
                    leftGun.SetActive(false);
                    // Right instance
                    GameObject rightGun = Instantiate(weaponData.prefab, rightHandPlace.position, Quaternion.identity) as GameObject;
                    rightGun.transform.SetParent(rightHandPlace);
                    rightGun.transform.localEulerAngles = weaponData.prefab.transform.rotation.eulerAngles;
                    // Setting events for using with left weapon
                    WeaponController rightWC = rightGun.GetComponent<WeaponController>();
                    rightWC.weapon = weaponData;
                    rightWC.OnDraw += leftWC.drawWeapon;
                    rightWC.OnHide += leftWC.hideWeapon;
                    rightWC.OnTrigger += leftWC.useWeapon;
                    rightWC.OnUnEquip += leftWC.unequipWeapon;
                    rightGun.SetActive(false);

                    switch (weaponData.slot) {
                        case WeaponSlot.PRIMARY:
                            if (primaryWeapon != null) primaryWeapon.unequipWeapon();
                            primaryWeapon = rightWC;
                            if (draw) owner.drawPrimary();
                            break;
                        case WeaponSlot.SECONDARY:
                            if (secondaryWeapon != null) secondaryWeapon.unequipWeapon();
                            secondaryWeapon = rightWC;
                            if (draw) owner.drawSecondary();
                            break;
                        default:
                            break;
                    }
                }
            }
        }

        public ItemController equipItem(int id) {
            ItemData itemData = Data.Instance.getItemById(id);
            if (itemData != null) {
                ItemController ic = null;
                // Let's make make some checks before equiping a new item
                if (firstItem != null && itemData.slot == ItemSlot.FIRST) {
                    // If item is the same as first, abort
                    if (firstItem.item.id == itemData.id) {
                        Debug.Log("Trying to equip the same first item, aborting");
                        return firstItem;
                    }
                    // Unequip item to disable passive effects and destroy visuals
                    firstItem.Unequip();
                    foreach (Transform visual in firstItemVisuals) {
                        Destroy(visual.gameObject);
                    }
                }
                if (secondItem != null && itemData.slot == ItemSlot.SECOND) {
                    // If item is the same as second, abort
                    if (secondItem.item.id == itemData.id) {
                        Debug.Log("Trying to equip the same second item, aborting");
                        return secondItem;
                    }
                    // Unequip item to disable passive effects and destroy visuals
                    secondItem.Unequip();
                    foreach (Transform visual in secondItemVisuals) {
                        Destroy(visual.gameObject);
                    }
                }
                // Finally create new visual if needed and new ref
                foreach (BodySlot slot in itemData.placement) {
                    Transform placement = getPlacement(slot);
                    GameObject instance = Instantiate(itemData.prefab, placement.position, Quaternion.identity) as GameObject;
                    instance.transform.SetParent(placement);
                    instance.transform.localEulerAngles = itemData.prefab.transform.rotation.eulerAngles;
                    ic = instance.GetComponent<ItemController>();
                    ic.owner = owner;
                    ic.item = itemData;
                    switch (itemData.slot) {
                        case ItemSlot.FIRST:
                            firstItem = ic;
                            firstItemVisuals.Add(instance.transform);
                            break;
                        case ItemSlot.SECOND:
                            secondItem = ic;
                            secondItemVisuals.Add(instance.transform);
                            break;
                        default:
                            break;
                    }
                }
                if (owner.playerUI != null) {
                    if (itemData.slot == ItemSlot.FIRST) owner.playerUI.updateFirstItem();
                    else owner.playerUI.updateSecondItem();
                }
                return ic;
            }
            else return null;
        }

        public void useSupply(int id) {
			SupplyData data = Data.Instance.getSupplyById(id);
			if (data != null) {
				switch (data.kind) {
					case SupplyType.HEALTH:
						owner.player.applyhealth(data.value);
						break;
					case SupplyType.ARMOR:
						owner.player.armor += data.value;
						break;
					default:
						break;
				}
			}
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
                    return rightHandPlace;
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
