using UnityEngine;
using UnityEngine.UI;
using System.Collections;

using SupHero;
using SupHero.Model;

namespace SupHero.Controllers {
    public class PlayerUIController : MonoBehaviour {

        public Player player { get; protected set; }

        public GameObject shieldPanel;
        public Text shieldText;
        public Text armorText;
        public Text healthText;
        public Text playerName;
        public Text playerPoints;

        // Use this for initialization
        void Start() {

        }

        // Update is called once per frame
        void Update() {
            if (player != null) {
                if (player is Hero) {
                    Hero hero = player as Hero;
                    shieldText.text = hero.shield.ToString();
                }
                else {
                    shieldPanel.SetActive(false);
                }
                armorText.text = player.armor.ToString();
                healthText.text = player.health.ToString();
                playerName.text = player.playerName;
                playerPoints.text = player.points.ToString();
            }
        }

        public void setPlayer(Player player) {
            this.player = player;
        }
    }
}
