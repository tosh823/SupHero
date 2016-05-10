using UnityEngine;
using UnityEngine.UI;
using System.Collections;

using SupHero;
using SupHero.Model;
using SupHero.Components.Character;
using SupHero.Components.Weapon;
using SupHero.Components.Item;

namespace SupHero.Components.UI {
    public class PlayerUIController : MonoBehaviour {

        public PlayerController pc { get; protected set; }

        public GameObject shieldPanel;
        public Text shieldText;
        public Text armorText;
        public Text playerName;
        public Text playerPoints;
        public Image weapon;
        public Image ammoBar;
        public Image firstItem;
        public Image firstItemCooldown;
        public Image secondItem;
        public Image secondItemCooldown;

        void Start() {

        }

        void Update() {
            if (pc != null) {
                if (pc.player is Hero) {
                    Hero hero = pc.player as Hero;
                    shieldText.text = string.Format("{0}%", ((int)hero.shieldPercentage).ToString());
                }
                else {
                    shieldPanel.SetActive(false);
                }
                armorText.text = pc.player.armor.ToString();
                playerName.text = pc.player.playerName;
                playerPoints.text = pc.player.points.ToString();
                updateAmmo();
            }
        }

        public void updateWeapon() {
            WeaponController wc = pc.activeWeapon();
            weapon.sprite = wc.weapon.image;
            updateAmmo();
        }

        public void updateAmmo() {
            WeaponController wc = pc.activeWeapon();
            if (wc != null && wc.weapon.slot == WeaponSlot.PRIMARY) {
                float ratio = (float)wc.ammo / (float)wc.weapon.ammo;
                ammoBar.fillAmount = ratio;
            }
            else {
                ammoBar.fillAmount = 1f;
            }
        }

        public void updateFirstItem() {
            ItemController ic = pc.firstItem();
            if (ic != null) {
                firstItem.sprite = ic.item.image;
                firstItemCooldown.sprite = ic.item.image;
            }
        }

        public void updateItemCooldown(ItemController ic, float time) {
            float ratio = (ic.item.activeData.cooldown - time) / ic.item.activeData.cooldown;
            if (ic.item.slot == ItemSlot.FIRST) {
                firstItemCooldown.fillAmount = ratio;
            }
            else {
                secondItemCooldown.fillAmount = ratio;
            }
        }

        public void updateSecondItem() {
            ItemController ic = pc.secondItem();
            if (ic != null) {
                secondItem.sprite = ic.item.image;
                secondItemCooldown.sprite = ic.item.image;
            }
        }

        public void setPlayer(PlayerController pc) {
            this.pc = pc;
        }
    }
}
