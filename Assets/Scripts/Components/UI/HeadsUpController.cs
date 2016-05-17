using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using SupHero.Components.Character;

namespace SupHero.Components.UI {
    public class HeadsUpController : MonoBehaviour {

        public Image healthBar;
        public Text armorText;

        protected Camera mainCamera;
        protected PlayerController carry;

        protected float maxHealth;

        public virtual void Start() {
            mainCamera = Camera.main;
            carry = GetComponentInParent<PlayerController>();
            transform.Translate(0f, 2.4f, 0f);
        }

        public virtual void Update() {
            transform.LookAt(transform.position + mainCamera.transform.rotation * Vector3.forward, mainCamera.transform.rotation * Vector3.up);
        }

        protected virtual void updateHealth() {
            float ratio = carry.player.health / maxHealth;
            healthBar.fillAmount = ratio;
        }

        protected virtual void updateArmor() {
            armorText.text = carry.player.armor.ToString();
        }
    }
}
