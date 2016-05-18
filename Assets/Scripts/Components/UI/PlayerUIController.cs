using UnityEngine;
using UnityEngine.UI;
using System.Collections;

using SupHero;
using SupHero.Model;
using SupHero.Components.Level;
using SupHero.Components.Character;
using SupHero.Components.Weapon;
using SupHero.Components.Item;

namespace SupHero.Components.UI {
    public class PlayerUIController : MonoBehaviour {

        public PlayerController pc { get; protected set; }

        public Image avatar;
        public Text playerName;
        public Text playerPoints;
        public Image weapon;
        public Image ammoBar;
        public Image firstItem;
        public Image firstItemCooldown;
        public Image secondItem;
        public Image secondItemCooldown;
        public Image direction;
        private bool tracing = false;
        private float scaleMultiplier;

        void Start() {
            scaleMultiplier = (transform.localScale.y >= 0f ? -1f : 1f);
        }

        void Update() {
            if (pc != null) {
                playerPoints.text = pc.player.points.ToString();
                updateAmmo();
                if (tracing) {
                    Vector3 where = pc.zone.getHero().transform.position - pc.transform.position;
                    Quaternion rotation = Quaternion.LookRotation(where);
                    float y = -rotation.eulerAngles.y;
                    if (scaleMultiplier <= 0f) y -= 180f;
                    Vector3 ang = new Vector3(0f, 0f, y);
                    direction.rectTransform.rotation = Quaternion.Lerp(direction.rectTransform.rotation, Quaternion.Euler(ang), 10f * Time.deltaTime);
                }
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

        public void startTracing() {
            tracing = true;
            direction.gameObject.SetActive(true);
        }

        public void stopTracing() {
            tracing = false;
            direction.gameObject.SetActive(false);
        }

        public void setPlayer(PlayerController pc) {
            this.pc = pc;
            playerName.text = pc.player.playerName;
            avatar.sprite = pc.player.character.avatar;
            direction.sprite = pc.player.character.arrow;
        }
    }
}
