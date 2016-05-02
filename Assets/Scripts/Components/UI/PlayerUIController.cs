using UnityEngine;
using UnityEngine.UI;
using System.Collections;

using SupHero;
using SupHero.Model;
using SupHero.Components.Character;
using SupHero.Components.Weapon;

namespace SupHero.Components.UI {
    public class PlayerUIController : MonoBehaviour {

        public PlayerController pc { get; protected set; }

        public GameObject shieldPanel;
        public Text shieldText;
        public Text armorText;
        public Text healthText;
        public Text playerName;
        public Text playerPoints;
        public Text ammo;

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

                WeaponController weapon = pc.activeWeapon();
                if (weapon != null) {
                    if (weapon.weapon.slot == WeaponSlot.PRIMARY) ammo.text = weapon.ammo.ToString();
                    else ammo.text = "Unlimited";
                }
            }
        }

        public void setPlayer(PlayerController pc) {
            this.pc = pc;
        }
    }
}
